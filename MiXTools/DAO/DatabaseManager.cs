using Microsoft.Data.Sqlite;
using MiXTools.Shared;
using Serilog;

namespace MiXTools.Database
{
    /// <inheritdoc/>
    /// <remarks>
    /// Singleton implementation of <see cref="IDatabaseManager"/>
    /// </remarks>
    public sealed class DatabaseManager : IDatabaseManager
    {
        /// <summary>
        /// Lazy instance
        /// </summary>
        private static readonly Lazy<DatabaseManager> lazy = new(() => { return new DatabaseManager(); });
        /// <summary>
        /// Lazy initialized class instance for Singleton use
        /// </summary>
        public static DatabaseManager Instance => lazy.Value;
        public string DBFileName { get; }
        public string DBFilePath { get; }
        /// <summary>
        /// Dictionary of SQLite CREATE commands for each table
        /// </summary>
        /// <remarks>
        /// Key is the table name, Value = SQL command
        /// </remarks>
        private readonly Dictionary<string, string> DBTables = new()
        {
            { "OSD", "CREATE TABLE OSD(id INTEGER PRIMARY KEY, path text NOT NULL, version text, currentURLOrFilePathForAssistantButton text, uaflMode text)" }
        };
        public const string DBFILENAME_IS_MISSING = "Database file name is missing from Settings.settings!";
        public const string DB_INIT_ERROR = "Database initialization error!";
        public const string ERROR_TITLE = "Error";
        public const string CREATE_DB_ERROR = "Database creation error";
        public const int DB_INIT_ERROR_CODE = 21;
        public const int DB_CREATE_ERROR_CODE = 22;

        /// <inheritdoc/>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the app directory is not accessible
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Thrown when <see cref="DBFileName"/> or <see cref="DBFolder"/> is missing from the application's config file
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when <see cref="Directory.GetCurrentDirectory"/> is not supported
        /// </exception>
        private DatabaseManager()
        {
            string? dbFile = Properties.Settings.Default.DBFileName;

            if (string.IsNullOrEmpty(dbFile))
            {
                Utils.ErrorHandler(DBFILENAME_IS_MISSING, DB_INIT_ERROR, ERROR_TITLE, DB_INIT_ERROR_CODE, false);
            }

            DBFileName = dbFile;
            DBFilePath = Application.UserAppDataPath + '\\' + DBFileName;

            // initialize database file
            InitDB();
        }

        /// <summary>
        /// Initializes the database file.
        /// </summary>
        private void InitDB()
        {
            try
            {
                if (File.Exists(DBFilePath))
                {
                    FileInfo file = new(DBFilePath);
                    Log.Debug($"DB file size: {file.Length}");
                    if (file.Length == 0)
                    {
                        InitializeDBFile();
                    }
                    else if (IsDBUpdateNeeded())
                    {
                        Log.Debug("Updaing database...");
                        InitializeDBFile(true);
                    }
                }
                else
                {
                    InitializeDBFile();
                }
                Log.Debug($"{DBFileName} path: {DBFilePath}");
            }
            catch (SqliteException sql_error)
            {
                Log.Error("SQLite table creation failed");
                Utils.ErrorHandler(sql_error.Message, CREATE_DB_ERROR, ERROR_TITLE, DB_CREATE_ERROR_CODE, false);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates a database file in <see cref="DatabaseManager.DBFilePath"/>.
        /// and deletes the <see cref="DatabaseManager.DBFolder"/>. 
        /// </summary>
        private void InitializeDBFile(bool canResetTables = false)
        {
            // create db file and open connection
            using SqliteConnection connection = new(GetConnectionString());
            connection.Open();

            // create tables
            foreach (var table in DBTables)
            {
                var cmd = connection.CreateCommand();
                if (canResetTables && GetDBTables().Contains(table.Key))
                {
                    Log.Information($"Resetting table {table.Key} before updating...");
                    cmd.CommandText = $"DROP TABLE '{table.Key}';";
                    cmd.ExecuteNonQuery();
                }
                Log.Information($"Creating updated version of table {table.Key.ToLower()}...");
                cmd.CommandText = table.Value;
                cmd.ExecuteNonQuery();
            }
        }

        private bool IsDBUpdateNeeded()
        {
            var tables = GetDBTables();
            foreach (string table in tables)
            {
                if (!DBTables.ContainsKey(table) || GetDBTableSQL(table) != DBTables[table])
                {
                    Log.Debug("Database is out-dated");
                    return true;
                }
            }

            Log.Debug("Database is up-to-date");
            return false;
        }
        private string GetDBTableSQL(string tableName)
        {
            using SqliteConnection connection = new(GetConnectionString());
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT sql FROM sqlite_master WHERE name=@name AND type ='table';";
            cmd.Parameters.AddWithValue("name", tableName);
            string sql = "";
            using SqliteDataReader reader = cmd.ExecuteReader();
            reader.Read();
            sql = reader.GetString(reader.GetOrdinal("sql"));
            return sql;
        }
        private List<string> GetDBTables()
        {
            using SqliteConnection connection = new(GetConnectionString());
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";

            List<string> tables = new();
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(reader.GetOrdinal("name")));
            }
            return tables;
        }

        /// <summary>
        /// Creates the connection string containing the <see cref="DatabaseManager.DBFileName"/> for creating database connection.
        /// </summary>
        /// <returns>The connection string</returns>
        public string GetConnectionString()
        {
            return $@"Data Source={DBFilePath}";
        }

        public void ResetDatabase()
        {
            try
            {
                InitializeDBFile(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

    }
}
