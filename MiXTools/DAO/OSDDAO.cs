using Microsoft.Data.Sqlite;
using MiXTools.Database;
using MiXTools.Model;
using Serilog;

namespace MiXTools.DAO
{
    /// <summary>
    /// Database Access Object for OSD class implementing <see cref="IOSDDAO"/>.
    /// </summary>
    internal class OSDDAO : IOSDDAO
    {
        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string connection_string;
        /// <summary>
        /// Singleton instance of <see cref="DatabaseManager"/>
        /// </summary>
        private readonly DatabaseManager databaseManager;

        /// <summary>
        /// Constructor for initializing database related values and class properties.
        /// </summary>
        public OSDDAO()
        {
            databaseManager = DatabaseManager.Instance;
            connection_string = databaseManager.GetConnectionString();
        }
        /// <inheritdoc/>
        /// <exception cref="SqliteException">Thrown when SQL related error occurs.</exception>
        public void AddOSD(OSD osd)
        {
            try
            {
                using SqliteConnection connection = new(connection_string);
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO OSD (id, path, version, currentURLOrFilePathForAssistantButton, uaflMode) VALUES (@id, @path, @version, @currentURLOrFilePathForAssistantButton, @uaflMode)";
                command.Parameters.AddWithValue("id", 1);
                command.Parameters.AddWithValue("path", osd.Path);
                command.Parameters.AddWithValue("version", osd.Version);
                command.Parameters.AddWithValue("currentURLOrFilePathForAssistantButton", osd.CurrentURLOrFilePathForAssistantButton);
                command.Parameters.AddWithValue("uaflMode", osd.UAFLMode);
                Log.Debug("SQL insert command result: " + command.ExecuteNonQuery());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="SqliteException">Thrown when SQL related error occurs.</exception>
        public OSD GetOSD()
        {
            try
            {
                using SqliteConnection connection = new(connection_string);
                connection.Open();
                OSD result = new();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM OSD WHERE id=1";

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Path = reader.GetString(reader.GetOrdinal("path"));
                    result.Version = reader.GetString(reader.GetOrdinal("version"));
                    result.CurrentURLOrFilePathForAssistantButton = reader.GetString(reader.GetOrdinal("currentURLOrFilePathForAssistantButton"));
                    result.UAFLMode = reader.GetString(reader.GetOrdinal("uaflMode"));
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="SqliteException">Thrown when SQL related error occurs.</exception>
        public void UpdateOSD(OSD osd)
        {
            try
            {
                using SqliteConnection connection = new(connection_string);
                connection.OpenAsync();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE OSD SET path=@path, version=@version, " +
                    "currentURLOrFilePathForAssistantButton=@currentURLOrFilePathForAssistantButton, uaflMode=@uaflMode WHERE id=1";
                command.Parameters.AddWithValue("path", osd.Path);
                command.Parameters.AddWithValue("version", osd.Version);
                command.Parameters.AddWithValue("currentAssistantButtonFile", osd.CurrentURLOrFilePathForAssistantButton);
                command.Parameters.AddWithValue("uaflMode", osd.UAFLMode);
                Log.Debug("SQL update command result: " + command.ExecuteNonQuery());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
