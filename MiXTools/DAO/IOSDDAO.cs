using MiXTools.Model;

namespace MiXTools.DAO
{
    /// <summary>
    /// DAO interface for OSD class.
    /// </summary>
    internal interface IOSDDAO
    {
        /// <summary>
        /// Gets the OSD from the database.
        /// </summary>
        /// <returns><see cref="OSD"/> from the database.</returns>
        OSD GetOSD();

        /// <summary>
        /// Adds <paramref name="osd"/> to the database.
        /// </summary>
        /// <param name="osd"></param>
        void AddOSD(OSD osd);

        /// <summary>
        /// Updates the current OSD in the database with new data from <paramref name="osd"/>.
        /// </summary>
        /// <param name="osd"></param>
        void UpdateOSD(OSD osd);
    }
}
