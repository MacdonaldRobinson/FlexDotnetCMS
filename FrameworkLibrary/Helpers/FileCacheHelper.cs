using System;
using System.IO;
using System.Linq;

namespace FrameworkLibrary
{
    public class FileCacheHelper
    {
        private static string baseDir = "";
        private static string htmlFileName = "index.html";

        public static string BaseCacheDir
        {
            get
            {
                return baseDir;
            }
        }

        public static void SetFileSystemCacheDirPath(string dirPath)
        {
            baseDir = dirPath;
        }

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
                //return new Return();
                var fileInfo = GetFileInfoFromUrl(url);

                if (!fileInfo.Directory.Exists)
                {
                    var directoryInfo = Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                else
                {
                    /*if ((CacheDurationInSeconds != null) && (DateTime.Now < fileInfo.LastWriteTime.AddSeconds((double)CacheDurationInSeconds)))
                    {
                        return new Return() { Error = ErrorHelper.CreateError("Cache hasn't expired yet") };
                    }*/

                    //File.Delete(fileInfo.FullName);
                }
                File.WriteAllText(fileInfo.FullName, html);
                //ContextHelper.SetToCache(url, html);                

                return new Return();
            }
            catch (Exception ex)
            {
                var error = ErrorHelper.CreateError($"Error Creating FileSystem Cache: '{url}", ex.Message);

                ErrorHelper.LogException(error.Exception);

                return new Return(ex, error);
            }
        }

        public static FileInfo GetFileInfoFromUrl(string url)
        {
            var invalid = Path.GetInvalidPathChars().ToList();

            foreach (var c in invalid)
            {
                if (url.Contains(c))
                    url = url.Replace(c.ToString(), "_");
            }

            url = url.Replace("?", "_");
            url = url.Replace(":", "-");

            if (!url.EndsWith("/"))
                url = url + "/";

            var absPath = URIHelper.ConvertToAbsPath($"{baseDir}{url.ToLower()}{htmlFileName}");
            var fileInfo = new FileInfo(absPath);

            return fileInfo;
        }

        public static void SaveGenerateToNav(string html)
        {
            FileCacheHelper.SaveToCache("generatenav", html);
        }

        public static Return GetGenerateNavCache()
        {
            return FileCacheHelper.GetFromCache("generatenav");
        }

        public static void DeleteGenerateNavCache()
        {
            FileCacheHelper.ClearCacheDir("generatenav");
        }

        public static void SaveSettingsCache(string setting, string html)
        {
            FileCacheHelper.SaveToCache("settings/"+ setting, html);
        }

        public static Return GetSettingsCache(string setting)
        {
            return FileCacheHelper.GetFromCache("settings/"+ setting);
        }

        public static void DeleteSettingsCache()
        {
            FileCacheHelper.ClearCacheDir("settings");
        }
        
        public static void DeleteHTMLCache()
        {
            FileCacheHelper.ClearCacheDir("html_");            
        }

        public static Return ClearCacheDir(string url)
        {
            try
            {
                url = url + "/";
                var fileInfo = GetFileInfoFromUrl(url);

                if (fileInfo.Directory.Exists)
                {                    
                    Directory.Delete(fileInfo.Directory.FullName, true);
                    ContextHelper.ClearAllMemoryCache();
                }

                return new Return();
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                return new Return(ex, ErrorHelper.CreateError(ex));
            }
        }

        public static Return RemoveFromCache(string url)
        {
            try
            {
                var fileInfo = GetFileInfoFromUrl(url);

                if (fileInfo.Exists)
                {
                    File.Delete(fileInfo.FullName);
                    ContextHelper.RemoveFromCache(url);

                    fileInfo.Directory.Delete(true);
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
            if (string.IsNullOrEmpty(baseDir))
                return new Return();

            //FileCacheHelper.DeleteGenerateNav();
            //FileCacheHelper.DeleteSettings();
            //FileCacheHelper.DeleteHTMLCache();

            var directoryInfo = new DirectoryInfo(URIHelper.ConvertToAbsPath(baseDir));

            var subDirectories = directoryInfo.GetDirectories();
            var allFiles = directoryInfo.GetFiles();

            foreach (var file in allFiles)
            {
                try
                {
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);
                }
            }

            foreach (var directory in subDirectories)
            {
                try
                {
                    Directory.Delete(directory.FullName, true);
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);
                }
            }

            ContextHelper.ClearAllMemoryCache();

            return new Return();
        }

        public static Return GetFromCache(string url)
        {
            var returnObj = new Return();
            try
            {
                //var inContext = (string)ContextHelper.GetFromCache(url);

                //if (inContext != null && inContext != "")
                //{                    
                //    returnObj.SetRawData(inContext);

                //    return returnObj;
                //}
                var fileInfo = GetFileInfoFromUrl(url);

                if (fileInfo.Exists)
                {
                    if ((CacheDurationInSeconds != null) && (DateTime.Now > fileInfo.LastWriteTime.AddSeconds((double)CacheDurationInSeconds)))
                    {
                        File.Delete(fileInfo.FullName);
                        returnObj.Error = new Elmah.Error(new Exception("Cache has expired"));                        
                        return returnObj;
                    }

                    using (FileStream fs = new FileStream(fileInfo.FullName,
                                                            FileMode.Open,
                                                            FileAccess.Read,
                                                            FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            var data = sr.ReadToEnd();
                            returnObj.SetRawData(data);
                            return returnObj;
                        }
                    }
                }        
                else
                {
                    returnObj.Error = new Elmah.Error(new Exception("Cache does not exist"));
                    return returnObj;
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                returnObj.Error = new Elmah.Error(ex);
                return returnObj;
            }
        }
    }
}