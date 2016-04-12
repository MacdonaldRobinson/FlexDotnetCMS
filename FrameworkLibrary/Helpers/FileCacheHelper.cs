using System;
using System.IO;
using System.Linq;

namespace FrameworkLibrary
{
    public class FileCacheHelper
    {
        private static string baseDir = "~/App_Data/Cache/";
        private static string htmlFileName = "index.html";

        public static double? CacheDurationInSeconds
        {
            get
            {
                return SettingsMapper.GetSettings()?.OutputCacheDurationInSeconds;
            }
        }

        public static Return SaveToCache(string url, string html)
        {
            try
            {
                var fileInfo = GetFileInfoFromUrl(url);

                if (!fileInfo.Directory.Exists)
                {
                    var directoryInfo = Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                else
                {
                    if ((CacheDurationInSeconds != null) && (DateTime.Now < fileInfo.LastWriteTime.AddSeconds((double)CacheDurationInSeconds)))
                    {
                        return new Return() { Error = ErrorHelper.CreateError("Cash hasn't expired yet") };
                    }

                    //File.Delete(fileInfo.FullName);
                    File.WriteAllText(fileInfo.FullName, html);
                }

                return new Return();
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return new Return(ex, ErrorHelper.CreateError(ex));
            }
        }

        private static FileInfo GetFileInfoFromUrl(string url)
        {
            var invalid = Path.GetInvalidPathChars().ToList();

            foreach (var c in invalid)
            {
                if (url.Contains(c))
                    url = url.Replace(c.ToString(), "_");
            }

            url = url.Replace("?", "_");

            return new FileInfo(URIHelper.ConvertToAbsPath(baseDir + url.ToLower() + htmlFileName));
        }

        public static Return ClearCache(string url)
        {
            try
            {
                var fileInfo = GetFileInfoFromUrl(url);

                if (fileInfo.Exists)
                {
                    File.Delete(fileInfo.FullName);
                }

                return new Return();
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return new Return(ex, ErrorHelper.CreateError(ex));
            }
        }

        public static Return ClearAllCache()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(URIHelper.ConvertToAbsPath(baseDir));

                var subDirectories = directoryInfo.GetDirectories();
                var allFiles = directoryInfo.GetFiles();

                foreach (var directory in subDirectories)
                {
                    Directory.Delete(directory.FullName, true);
                }

                foreach (var file in allFiles)
                {
                    File.Delete(file.FullName);
                }

                return new Return();
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return new Return(ex, ErrorHelper.CreateError(ex));
            }
        }

        public static string GetFromCache(string url)
        {
            try
            {
                var fileInfo = GetFileInfoFromUrl(url);

                if (fileInfo.Exists)
                {
                    if ((CacheDurationInSeconds != null) && (DateTime.Now > fileInfo.LastWriteTime.AddSeconds((double)CacheDurationInSeconds)))
                    {
                        File.Delete(fileInfo.FullName);
                        return "";
                    }

                    using (FileStream fs = new FileStream(fileInfo.FullName,
                                                            FileMode.Open,
                                                            FileAccess.Read,
                                                            FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return "";
            }
        }
    }
}