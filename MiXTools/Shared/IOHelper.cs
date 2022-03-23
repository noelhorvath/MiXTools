

using Serilog;
using System.Text;

namespace MiXTools.Shared
{
    internal static class IOHelper
    {
        public static bool CopyFile(string srcFile, string destFile, bool overwrite = false, bool deleteSrcFile = false)
        {
            bool isSuccess;
            try
            {
                Log.Debug("CopyFile destFile: " + File.Exists(destFile));
                Log.Debug("CopyFile srcFile: " + File.Exists(srcFile));
                if (File.Exists(srcFile))
                {
                    if (deleteSrcFile)
                        DeleteFile(destFile);

                    File.Copy(srcFile, destFile, overwrite);
                    isSuccess = deleteSrcFile || IsFileSizeSame(srcFile, destFile);
                }
                else
                    isSuccess = false;
            }
            catch (Exception ex)
            {
                Log.Error($"srcFile: {srcFile}, destFile: {destFile}");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                isSuccess = false;
            }
            return isSuccess;
        }


        public static void DeleteFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static bool IsFileSizeSame(string srcFile, string destFile)
        {
            try
            {
                if (File.Exists(srcFile) && File.Exists(destFile))
                {
                    FileInfo srcInfo = new(srcFile);
                    FileInfo destInfo = new(destFile);
                    Log.Debug("srcFile size: " + srcInfo.Length);
                    Log.Debug("destFile size: " + destInfo.Length);
                    return srcFile.Length - destFile.Length == 0;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void WriteDataToFile<T>(string file, IEnumerable<T> data, bool canDeleteFileContent = false)
        {

            try
            {
                if (canDeleteFileContent)
                {
                    File.WriteAllText(file, ""); // delete file content
                    using var writer = new StreamWriter(file);
                    foreach (var item in data)
                        writer.WriteLine(item);
                }
                else
                {
                    using var writer = new StreamWriter(file);
                    foreach (var item in data)
                        writer.WriteLine(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task WriteDataToFileAsync<T>(string file, IEnumerable<T> data, bool canDeleteFileContent = false)
        {
            try
            {
                if (canDeleteFileContent)
                {
                    File.WriteAllText(file, ""); // delete file's content
                    using var writer = new StreamWriter(file);
                    foreach (var item in data)
                        if (item != null)
                            await writer.WriteLineAsync(item.ToString());
                }
                else
                {
                    using var writer = new StreamWriter(file);
                    foreach (var item in data)
                        if (item != null)
                            await writer.WriteLineAsync(item.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static IEnumerable<string> ReadDataFromFile(string file, Encoding? encoding)
        {
            encoding ??= Encoding.UTF8;
            try
            {
                List<string> list = new();
                using var reader = new StreamReader(file, encoding);
                string? line;
                while ((line = reader.ReadLine()) != null)
                    list.Add(line);

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<IEnumerable<string>> ReadDataFromFileAsync(string file, Encoding? encoding)
        {
            encoding ??= Encoding.UTF8;
            try
            {
                List<string> list = new();
                using var reader = new StreamReader(file, encoding);
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                    list.Add(line);

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static FileStream LockFile(string file, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return new FileStream(file, fileMode, fileAccess, fileShare);
        }
    }
}
