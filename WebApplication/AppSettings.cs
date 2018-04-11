using FrameworkLibrary;
using System;
using System.Configuration;

namespace WebApplication
{
    /// <summary>
    /// Summary description for AppSettings
    /// </summary>
    public class AppSettings
    {
        public enum ConnectionStringKey
        {
            Dev,
            Stage,
            Prod
        }

        public static string MetaIncludesPlaceHolderID
        {
            get
            {
                return ConfigurationManager.AppSettings["MetaIncludesPlaceHolderID"];
            }
        }

        public static string JsIncludesPlaceHolderID
        {
            get
            {
                return ConfigurationManager.AppSettings["JsIncludesPlaceHolderID"];
            }
        }

        public static string CssIncludesPlaceHolderID
        {
            get
            {
                return ConfigurationManager.AppSettings["CssIncludesPlaceHolderID"];
            }
        }

        public static bool EnableWebRequestCaching
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableWebRequestCaching"]);
            }
        }

        public static long WebRequestCacheDurationInSeconds
        {
            get
            {
                return long.Parse(ConfigurationManager.AppSettings["WebRequestCacheDurationInSeconds"]);
            }
        }

        public static bool EnableGeoLocation
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableGeoLocation"]);
            }
        }

        public static bool EnableOutputCaching
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableOutputCaching"]);
            }
        }

        public static bool EnableLevel1MemoryCaching
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableLevel1MemoryCaching"]);
            }
        }

        public static bool EnableLevel2FileCaching
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableLevel2FileCaching"]);
            }
        }

        public static bool EnableLevel3RedisCaching
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableLevel3RedisCaching"]);
            }
        }

        public static string FileSystemCacheDirPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FileSystemCacheDirPath"];
            }
        }

        public static string RedisCacheConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisCacheConnectionString"];
            }
        }

        public static string WeatherApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherApiKey"];
            }
        }

        public static string CompressUriRequestPatterns
        {
            get
            {
                return ConfigurationManager.AppSettings["CompressUriRequestPatterns"];
            }
        }

        public static string DBBackupPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DBBackupPath"];
            }
        }

        public static bool AttemptCompression
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["AttemptCompression"]);
            }
        }

        public static ConnectionStringKey CurrentConnectionStringKey
        {
            get
            {
                return (ConnectionStringKey)Enum.Parse(typeof(ConnectionStringKey), ConfigurationManager.AppSettings["ConnectionStringKeyName"]);
            }
        }        

        public static bool EnableMobileDetection
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableMobileDetection"]);
            }
        }

        public static bool IsRunningOnProd
        {
            get
            {
                if(CurrentConnectionStringKey == ConnectionStringKey.Prod)
                {
                    return true;
                }

                return false;
            }
        }

        public static string GetCurrentFlashUrl()
        {
            return URIHelper.BaseUrl + URIHelper.GetCurrentVirtualPath().Replace("~", "#");
        }

        public static ConnectionStringSettings GetConnectionSettingsByName(string connectionStringName)
        {
            string name = Convert.ToString(connectionStringName);
            return ConfigurationManager.ConnectionStrings[name];
        }

        public static ConnectionStringSettings GetConnectionSettings()
        {
            string name = Convert.ToString(CurrentConnectionStringKey);
            return ConfigurationManager.ConnectionStrings[name];
        }

        public static string LoadJsIncludesUriSegment
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadJsIncludesUriSegment"];
            }
        }

        public static string LoadCssIncludesUriSegment
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadCssIncludesUriSegment"];
            }
        }

        public static string LoadFileUriSegment
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadFileUriSegment"];
            }
        }

        public static string LoadJsIncludesUrl
        {
            get
            {
                return FileServiceHandlerUrl + LoadJsIncludesUriSegment;
            }
        }

        public static string LoadCssIncludesUrl
        {
            get
            {
                return FileServiceHandlerUrl + LoadCssIncludesUriSegment;
            }
        }

        public static string FileServiceHandlerUrl
        {
            get
            {
                return URIHelper.ConvertToAbsUrl(ConfigurationManager.AppSettings["FileServiceHandlerUrl"]);
            }
        }

        public static string GeoLocationAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["GeoLocationAPIKey"];
            }
        }

        public static string EnableAutoIPLocationTracking
        {
            get
            {
                return ConfigurationManager.AppSettings["EnableAutoIPLocationTracking"];
            }
        }

        public static bool UseLoadFileServiceUrl
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["UseLoadFileServiceUrl"]);
            }
        }

        public static bool MinifyOutput
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["MinifyOutput"]);
            }
        }

        public static bool CombineCssAndJsIncludes
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["CombineCssAndJsIncludes"]);
            }
        }

        public static bool AutoRedirectToFlashVersion
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["AutoRedirectToFlashVersion"]);
            }
        }

        public static bool AllowWindowsLogin
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["AllowWindowsLogin"]);
            }
        }

        public static string SiteName
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteName"];
            }
        }

        public static string SystemName
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemName"];
            }
        }

        public static string FlashVersionBaseUri
        {
            get
            {
                return ConfigurationManager.AppSettings["FlashVersionBaseUri"];
            }
        }

        public static string FlickerAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["FlickerAPIKey"];
            }
        }

        public static string FlickerAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FlickerAppSecret"];
            }
        }

        public static string TwitterScreenName
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterScreenName"];
            }
        }

        public static string TwitterApiConsumerKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiConsumerKey"];
            }
        }

        public static string TwitterApiConsumerSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiConsumerSecret"];
            }
        }

        public static string TwitterApiAccessToken
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiAccessToken"];
            }
        }

        public static string TwitterApiAccessTokenSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiAccessTokenSecret"];
            }
        }

        public static string TwitterApiUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiUsername"];
            }
        }

        public static string TwitterApiPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterApiPassword"];
            }
        }

        public static string FlickerUserID
        {
            get
            {
                return ConfigurationManager.AppSettings["FlickerUserID"];
            }
        }

        public static string FacebookAppID
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppID"];
            }
        }

        public static string FacebookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppSecret"];
            }
        }

        public static string SystemEmailAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemEmailAddress"];
            }
        }

        public static string PageNotFoundHandler
        {
            get
            {
                return URIHelper.BaseUrlWithLanguage + "page-not-found/";
            }
        }

        public static string PayPalBuisnessID
        {
            get
            {
                return ConfigurationManager.AppSettings["PayPalBuisnessID"];
            }
        }

        public static string InstagramClientID
        {
            get
            {
                return ConfigurationManager.AppSettings["InstagramClientID"];
            }
        }

        public static string MailChimpApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["MailChimpApiKey"];
            }
        }

        public static bool EnableUrlRedirectRules
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableUrlRedirectRules"]);
            }
        }

        public static bool AttemptSMTPMailer
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["AttemptSMTPMailer"]);
            }
        }

        public static bool EnableInstaller
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableInstaller"]);
            }
        }

        public static bool ForceSSL
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["ForceSSL"]);
            }
        }

        public static bool ForceWWWRedirect
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["ForceWWWRedirect"]);
            }
        }        

        public static string GoogleAPIKey
        {
            get
            {
                string googleApiKey = "GoogleAPIKey-" + FrameworkSettings.GetCurrentLanguage() + "-" + CurrentConnectionStringKey;
                return ConfigurationManager.AppSettings[googleApiKey];
            }
        }
    }
}