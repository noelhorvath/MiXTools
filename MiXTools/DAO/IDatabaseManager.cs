namespace MiXTools.Database
{
    /// <summary>
    /// <c>DatabaseManager</c> is an interface for managing and initializning database related settings, informations from application's config file.
    /// </summary>
    internal interface IDatabaseManager
    {
        /// <summary>
        /// Name of the database file
        /// </summary>
        string DBFileName { get; }
        /// <summary>
        /// Path of the database's folder
        /// </summary>
        string DBFilePath { get; }
        /// <summary>
        /// Gets the connection string
        /// </summary>
        string GetConnectionString();

        void ResetDatabase();
    }
}
