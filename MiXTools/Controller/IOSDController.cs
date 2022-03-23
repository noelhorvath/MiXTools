using MiXTools.Model;

namespace MiXTools.Controller
{
    /// <summary>
    /// Controller interface for <see cref="OSD"/>
    /// </summary>
    public interface IOSDController
    {
        /// <summary>
        /// Adds <paramref name="osd"/> to the database.
        /// </summary>
        /// <param name="osd"></param>
        void AddOSD(OSD osd);

        /// <summary>
        /// Updatess <paramref name="osd"/> in the database.
        /// </summary>
        /// <param name="osd"></param>
        void UpdateOSD(OSD osd);

        /// <summary>
        /// Gets <see cref="OSD"/> from the database.
        /// </summary>
        OSD GetOSD();
        void UpdateOSDPath(string path);
        void UpdateOSDVersion(string version);
        void UpdateOSDCurrentURLOrFilePathForAssistantButton(string urlOrfilePath);
        void UpdateOSDFLMode(string flMode);
    }
}
