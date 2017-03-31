using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using FrameworkLibrary;

namespace TinyFileManager.NET
{
    public class clsConfig
    {
        #region Private Variables

        private clsProfile pobjProfile = new clsProfile();

        #endregion Private Variables

        #region Settings Properties

        /// <summary>
        ///  Max upload filesize in Mb
        /// </summary>
        public int intMaxUploadSizeMb
        {
            get
            {
                if (HttpContext.Current.Session["TFM_MaxUploadSizeMb"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["TFM_MaxUploadSizeMb"]);
                }
                else
                {
                    return Convert.ToInt32(this.pobjProfile.MaxUploadSizeMb);
                }
            }
        }

        /// <summary>
        ///  Allowed image file extensions
        /// </summary>
        public string strAllowedImageExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedImageExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedImageExtensions"]);
                }
                else
                {
                    return this.pobjProfile.AllowedImageExtensions;
                }
            }
        }

        /// <summary>
        ///  Allowed image file extensions as an array
        /// </summary>
        public string[] arrAllowedImageExtensions
        {
            get
            {
                return getArrayFromString(this.strAllowedImageExtensions);
            }
        }

        /// <summary>
        ///  Allowed document file extensions
        /// </summary>
        public string strAllowedFileExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedFileExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedFileExtensions"]);
                }
                else
                {
                    return this.pobjProfile.AllowedFileExtensions;
                }
            }
        }

        /// <summary>
        ///  Allowed document file extensions as an array
        /// </summary>
        public string[] arrAllowedFileExtensions
        {
            get
            {
                return getArrayFromString(this.strAllowedFileExtensions);
            }
        }

        /// <summary>
        ///  Allowed video file extensions
        /// </summary>
        public string strAllowedVideoExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedVideoExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedVideoExtensions"]);
                }
                else
                {
                    return this.pobjProfile.AllowedVideoExtensions;
                }
            }
        }

        /// <summary>
        ///  Allowed video file extensions as an array
        /// </summary>
        public string[] arrAllowedVideoExtensions
        {
            get
            {
                return getArrayFromString(this.strAllowedVideoExtensions);
            }
        }

        /// <summary>
        ///  Allowed music file extensions
        /// </summary>
        public string strAllowedMusicExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedMusicExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedMusicExtensions"]);
                }
                else
                {
                    return this.pobjProfile.AllowedMusicExtensions;
                }
            }
        }

        /// <summary>
        ///  Allowed music file extensions as an array
        /// </summary>
        public string[] arrAllowedMusicExtensions
        {
            get
            {
                return getArrayFromString(this.strAllowedMusicExtensions);
            }
        }

        /// <summary>
        ///  Allowed misc file extensions
        /// </summary>
        public string strAllowedMiscExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedMiscExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedMiscExtensions"]);
                }
                else
                {
                    return this.pobjProfile.AllowedMiscExtensions;
                }
            }
        }

        /// <summary>
        ///  Allowed misc file extensions as an array
        /// </summary>
        public string[] arrAllowedMiscExtensions
        {
            get
            {
                return getArrayFromString(this.strAllowedMiscExtensions);
            }
        }

        /// <summary>
        ///  All allowed file extensions
        /// </summary>
        public string strAllowedAllExtensions
        {
            get
            {
                string strRet = "";

                if (this.strAllowedImageExtensions.Length > 0)
                {
                    strRet = this.strAllowedImageExtensions;
                }
                if (this.strAllowedFileExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + this.strAllowedFileExtensions;
                    }
                    else
                    {
                        strRet = this.strAllowedFileExtensions;
                    }
                }
                if (this.strAllowedVideoExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + this.strAllowedVideoExtensions;
                    }
                    else
                    {
                        strRet = this.strAllowedVideoExtensions;
                    }
                }
                if (this.strAllowedMusicExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + this.strAllowedMusicExtensions;
                    }
                    else
                    {
                        strRet = this.strAllowedMusicExtensions;
                    }
                }
                if (this.strAllowedMiscExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + this.strAllowedMiscExtensions;
                    }
                    else
                    {
                        strRet = this.strAllowedMiscExtensions;
                    }
                }

                return strRet;
            }
        }

        /// <summary>
        /// Returns document root
        /// </summary>
        public string strDocRoot
        {
            get
            {
                if (HttpContext.Current.Session["TFM_RootPath"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_RootPath"]).TrimEnd('\\');
                }
                else
                {
                    if (this.pobjProfile.RootPath != "")
                    {
                        return this.pobjProfile.RootPath.TrimEnd('\\');
                    }
                    else
                    {
                        return URIHelper.BasePath.TrimEnd('\\');
                    }
                }
            }
        }

        /// <summary>
        /// Returns the base url of the site
        /// </summary>
        public string strBaseURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_RootURL"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_RootURL"]).TrimEnd('/');
                }
                else
                {
                    if (this.pobjProfile.RootURL != "")
                    {
                        return this.pobjProfile.RootURL.TrimEnd('/');
                    }
                    else
                    {
                        return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority.TrimEnd('/');
                    }
                }
            }
        }

        /// <summary>
        /// Returns the full upload drive path
        /// </summary>
        public string strUploadPath
        {
            get
            {
                if (HttpContext.Current.Session["TFM_UploadPath"] != null)
                {
                    return this.strDocRoot + "\\" + Convert.ToString(HttpContext.Current.Session["TFM_UploadPath"]).TrimEnd('\\');
                }
                else
                {
                    return this.strDocRoot + "\\" + this.pobjProfile.UploadPath.TrimEnd('\\');
                }
            }
        }

        /// <summary>
        /// Returns the full thumb drive path
        /// </summary>
        public string strThumbPath
        {
            get
            {
                if (HttpContext.Current.Session["TFM_ThumbPath"] != null)
                {
                    return this.strDocRoot + "\\" + Convert.ToString(HttpContext.Current.Session["TFM_ThumbPath"]).TrimEnd('\\') + "\\";
                }
                else
                {
                    return this.strDocRoot + "\\" + this.pobjProfile.ThumbPath.TrimEnd('\\') + "\\";
                }
            }
        }

        /// <summary>
        /// Returns the full upload url
        /// </summary>
        public string strUploadURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_UploadPath"] != null)
                {
                    return this.strBaseURL + "/" + Convert.ToString(HttpContext.Current.Session["TFM_UploadPath"]).Replace('\\', '/');
                }
                else
                {
                    return this.strBaseURL + "/" + this.pobjProfile.UploadPath.Replace('\\', '/');
                }
            }
        }

        /// <summary>
        /// Returns the full thumb url
        /// </summary>
        public string strThumbURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_ThumbPath"] != null)
                {
                    return this.strBaseURL + "/" + Convert.ToString(HttpContext.Current.Session["TFM_ThumbPath"]).Replace('\\', '/');
                }
                else
                {
                    return this.strBaseURL + "/" + this.pobjProfile.ThumbPath.Replace('\\', '/');
                }
            }
        }

        /// <summary>
        /// Returns the setting for a custom element to fill the selected item url
        /// </summary>
        public string strFillSelector
        {
            get
            {
                if (HttpContext.Current.Session["TFM_FillSelector"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_FillSelector"]);
                }
                else
                {
                    return this.pobjProfile.FillSelector;
                }
            }
        }

        /// <summary>
        /// Returns the setting for custom code to close the popup
        /// </summary>
        public string strPopupCloseCode
        {
            get
            {
                if (HttpContext.Current.Session["TFM_PopupCloseCode"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_PopupCloseCode"]);
                }
                else
                {
                    return this.pobjProfile.PopupCloseCode;
                }
            }
        }

        /// <summary>
        /// Returns the setting for allowing upload of file
        /// </summary>
        public bool boolAllowUploadFile
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowUploadFile"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowUploadFile"]); ;
                }
                else
                {
                    return Convert.ToBoolean(this.pobjProfile.AllowUploadFile);
                }
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of file
        /// </summary>
        public bool boolAllowDeleteFile
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowDeleteFile"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowDeleteFile"]); ;
                }
                else
                {
                    return Convert.ToBoolean(this.pobjProfile.AllowDeleteFile);
                }
            }
        }

        /// <summary>
        /// Returns the setting for allowing creation of folder
        /// </summary>
        public bool boolAllowCreateFolder
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowCreateFolder"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowCreateFolder"]); ;
                }
                else
                {
                    return Convert.ToBoolean(this.pobjProfile.AllowCreateFolder);
                }
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of folder
        /// </summary>
        public bool boolAllowDeleteFolder
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowDeleteFolder"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowDeleteFolder"]); ;
                }
                else
                {
                    return Convert.ToBoolean(this.pobjProfile.AllowDeleteFolder);
                }
            }
        }

        #endregion Settings Properties

        #region Constructors

        public clsConfig()
        {
            this.LoadConfig("Default");
        }

        public clsConfig(string strProfile)
        {
            if (strProfile == "")
            {
                this.LoadConfig("Default");
            }
            else
            {
                this.LoadConfig(strProfile);
            }
        }

        #endregion Constructors

        #region Private Routines

        private string[] getArrayFromString(string strInput)
        {
            string[] arrExt;
            string strTemp;

            //remove lead and trail single quotes so we can SPLIT the hell out of it
            strTemp = strInput.Trim('\'');
            arrExt = strTemp.Split(new string[] { "'", ",", "'" }, StringSplitOptions.RemoveEmptyEntries);

            return arrExt;
        }   // getArrayFromString

        private void LoadConfig(string strProfile)
        {
            string strConfig;
            XDocument objDoc;
            XElement objProfiles;

            strConfig = HttpContext.Current.Server.MapPath("web.config");
            objDoc = XDocument.Load(strConfig);
            objProfiles = objDoc.Element("configuration").Element("TFMProfiles");
            foreach (XElement objProfile in objProfiles.Descendants("profile"))
            {
                if (Convert.ToString(objProfile.Attribute("name").Value).ToLower() == strProfile.ToLower())
                {
                    this.pobjProfile = new clsProfile();
                    this.pobjProfile.AllowCreateFolder = Convert.ToBoolean(objProfile.Element("AllowCreateFolder").Value);
                    this.pobjProfile.AllowDeleteFile = Convert.ToBoolean(objProfile.Element("AllowDeleteFile").Value);
                    this.pobjProfile.AllowDeleteFolder = Convert.ToBoolean(objProfile.Element("AllowDeleteFolder").Value);
                    this.pobjProfile.AllowUploadFile = Convert.ToBoolean(objProfile.Element("AllowUploadFile").Value);
                    this.pobjProfile.AllowedFileExtensions = objProfile.Element("AllowedFileExtensions").Value;
                    this.pobjProfile.AllowedImageExtensions = objProfile.Element("AllowedImageExtensions").Value;
                    this.pobjProfile.AllowedMiscExtensions = objProfile.Element("AllowedMiscExtensions").Value;
                    this.pobjProfile.AllowedMusicExtensions = objProfile.Element("AllowedMusicExtensions").Value;
                    this.pobjProfile.AllowedVideoExtensions = objProfile.Element("AllowedVideoExtensions").Value;
                    this.pobjProfile.FillSelector = objProfile.Element("FillSelector").Value;
                    this.pobjProfile.MaxUploadSizeMb = Convert.ToInt16(objProfile.Element("MaxUploadSizeMb").Value);
                    this.pobjProfile.PopupCloseCode = objProfile.Element("PopupCloseCode").Value;
                    this.pobjProfile.RootPath = objProfile.Element("RootPath").Value;
                    this.pobjProfile.RootURL = objProfile.Element("RootURL").Value;
                    this.pobjProfile.ThumbPath = objProfile.Element("ThumbPath").Value;
                    this.pobjProfile.UploadPath = objProfile.Element("UploadPath").Value;
                    break;
                }
            }   // foreach
        }   // LoadConfig

        #endregion Private Routines
    }   // class
}   // namespace