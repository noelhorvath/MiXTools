using MiXTools.DAO;
using MiXTools.Model;
using MiXTools.Shared;
using Serilog;
using System.Text.RegularExpressions;

namespace MiXTools.Controller
{
    /// <summary>
    /// Controller for OSD
    /// </summary>
    internal class OSDController : IOSDController
    {
        /// <summary>
        /// Instance of an Data Access Object for OSD
        /// </summary>
        private readonly OSDDAO DAO;

        /// <summary>
        /// Constructor that initializes <see cref="DAO"/> with <paramref name="dao"/>.
        /// </summary>
        /// <param name="dao"></param>
        public OSDController(OSDDAO dao)
        {
            DAO = dao;
        }

        /// <inheritdoc/>
        /// <param name="osd"></param>
        /// /// <exception cref="Exception">Thrown when <see cref="OSDDAO.AddOSD"/> fails.</exception>
        public void AddOSD(OSD osd)
        {
            try
            {
                DAO.AddOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <inheritdoc/>
        public OSD GetOSD()
        {
            try
            {
                return DAO.GetOSD();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <param name="osd"></param>
        /// <exception cref="Exception">Thrown when <see cref="OSDDAO.UpdateOSD"/> fails.</exception>
        public void UpdateOSD(OSD osd)
        {
            try
            {
                DAO.UpdateOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <param name="osd"></param>
        /// <exception cref="Exception">Thrown when <see cref="OSDDAO.UpdateOSD"/> fails.</exception>
        public void UpdateOSD(string? path = null, string? version = null, string? currentAssistantButtonFile = null, string? flMode = null)
        {
            try
            {
                OSD osd = DAO.GetOSD();
                DAO.UpdateOSD(new OSD(path ?? osd.Path, version ?? osd.Version, currentAssistantButtonFile ?? osd.CurrentURLOrFilePathForAssistantButton, flMode ?? osd.UAFLMode));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the newest version of Mi OSD Utility from it's sub folders.
        /// </summary>
        /// <param name="dirs">Directories in Mi OSD Utility's folder.</param>
        /// <returns>Newest version as a <see cref="string"/> or <see cref="null"/>.</returns>
        public static string GetNewestOSDVersionFromDirs(string osdPath)
        {
            if (String.IsNullOrWhiteSpace(osdPath) || String.IsNullOrEmpty(osdPath) || osdPath.Equals("-")) { return "unknown"; }
            // get versions
            string[] dirs = Directory.GetDirectories(osdPath);
            string[] versions = new string[dirs.Length];
            int numOfValidVersions = 0;
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] splitted = dirs[i].Split('\\');
                if (Regex.IsMatch(splitted[^1], "^([0-9]+([/.][0-9]+)*)$") && Directory.GetFiles(dirs[i]).ToList().Contains(dirs[i] + '\\' + "OSDUtility.exe"))
                {
                    versions[numOfValidVersions++] = splitted[^1];
                    Log.Debug("Valid version: " + splitted[^1]);
                }
                else
                {
                    Log.Debug("Invalid version: " + splitted[^1]);
                }
            }
            Array.Resize(ref versions, numOfValidVersions);
            return Utils.GetNewestVersion(versions, true, false, "^([0-9]+([/.][0-9]+)*)$");
        }

        public void UpdateOSDCurrentURLOrFilePathForAssistantButton(string urlOrfilePath)
        {
            try
            {
                OSD osd = GetOSD();
                osd.CurrentURLOrFilePathForAssistantButton = urlOrfilePath;
                UpdateOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateOSDPath(string path)
        {
            try
            {
                OSD osd = GetOSD();
                osd.Path = path;
                UpdateOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateOSDVersion(string version)
        {
            try
            {
                OSD osd = GetOSD();
                osd.Version = version;
                UpdateOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateOSDFLMode(string flMode)
        {
            try
            {
                OSD osd = GetOSD();
                osd.UAFLMode = flMode;
                UpdateOSD(osd);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
