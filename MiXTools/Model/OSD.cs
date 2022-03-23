namespace MiXTools.Model
{
    /// <summary>
    /// Mi OSD Utility information 
    /// </summary>
    public class OSD : IOSD
    {
        private string _path = null!;
        /// <summary>
        /// Path where Mi OSD Utility is installed.
        /// </summary>
        public string Path
        {
            get => _path;
            set => _path = String.IsNullOrWhiteSpace(value) ? "-" : value;
        }
        /// <summary>
        /// Current version of Mi OSD Utility
        /// </summary>
        public string _version = null!;
        public string Version
        {
            get => _version;
            set => _version = String.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        /// <summary>
        /// File (path) or URL that opens when the assistant button is pressed
        /// </summary>
        private string _currentURLOrFilePathForAssistantButton = null!;
        public string CurrentURLOrFilePathForAssistantButton
        {
            get => _currentURLOrFilePathForAssistantButton;
            set => _currentURLOrFilePathForAssistantButton = String.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        /// <summary>
        /// URLAndFileLauncher mode
        /// </summary>
        private string _uaflMode = null!;
        public string UAFLMode
        {
            get => _uaflMode;
            set => _uaflMode = String.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        public OSD() : this("", "", "", "") { }
        public OSD(string path) : this(path, "", "", "") { }
        public OSD(string path, string version) : this(path, version, "", "") { }
        public OSD(string path, string version, string currentURLOrFilePathForAssistantButton) : this(path, version, currentURLOrFilePathForAssistantButton, "") { }

        public OSD(string path, string version, string currentURLOrFilePathForAssistantButton, string uaflMode)
        {
            Path = path;
            Version = version;
            CurrentURLOrFilePathForAssistantButton = currentURLOrFilePathForAssistantButton;
            UAFLMode = uaflMode;
        }

        /// <summary>
        /// Gets the path where the latest version of Mi OSD Utility is installed.
        /// </summary>
        /// <returns>Full path to that directory</returns>
        /// <exception cref="OSDException">Thrown when path or/and version is null or empty</exception>
        public string GetCurrentVersionPath()
        {
            if (Path == "-" || Version == "-")
            {
                throw new OSDException("No available version or/and path!");
            }

            return Path + '\\' + Version;
        }

        /// <summary>
        /// Creates a string from the OSD class.
        /// </summary>
        /// <returns>OSD class as a string</returns>
        public override string ToString()
        {
            return $"OSD path: {Path}, OSD version: {Version}, current URL or file path for assistant button: {CurrentURLOrFilePathForAssistantButton}, URLAndFileLauncher mode: {UAFLMode}";
        }

        public bool IsEmpty()
        {
            return Path == "-"
                && Version == "-"
                && CurrentURLOrFilePathForAssistantButton == "-"
                && UAFLMode == "-";

        }
    }

    class OSDException : Exception
    {
        public OSDException(string msg) : base(msg) { }
    }

}
