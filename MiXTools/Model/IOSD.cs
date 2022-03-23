namespace MiXTools.Model
{
    public interface IOSD
    {
        string Path { get; set; }
        string Version { get; set; }
        string CurrentURLOrFilePathForAssistantButton { get; set; }
        string UAFLMode { get; set; }
        string GetCurrentVersionPath();
        string ToString();
        bool IsEmpty();
    }
}
