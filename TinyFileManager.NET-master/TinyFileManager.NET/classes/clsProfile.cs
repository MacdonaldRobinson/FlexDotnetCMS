using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TinyFileManager.NET
{
    public class clsProfile
    {
        #region Private Variables

        private string pstrAllowedImageExtensions = "";
        private string pstrAllowedFileExtensions = "";
        private string pstrAllowedVideoExtensions = "";
        private string pstrAllowedMusicExtensions = "";
        private string pstrAllowedMiscExtensions = "";
        private string pstrUploadPath = "";
        private string pstrThumbPath = "";
        private bool pboolAllowUploadFile = false;
        private bool pboolAllowDeleteFile = false;
        private bool pboolAllowCreateFolder = false;
        private bool pboolAllowDeleteFolder = false;
        private int pintMaxUploadSizeMb = 0;
        private string pstrRootPath = "";
        private string pstrRootURL = "";
        private string pstrFillSelector = "";
        private string pstrPopupCloseCode = "";

        #endregion Private Variables

        #region Settings Properties

        /// <summary>
        ///  Max upload filesize in Mb
        /// </summary>
        public int MaxUploadSizeMb
        {
            get
            {
                return pintMaxUploadSizeMb;
            }
            set
            {
                pintMaxUploadSizeMb = value;
            }
        }

        /// <summary>
        ///  Allowed image file extensions
        /// </summary>
        public string AllowedImageExtensions
        {
            get
            {
                return pstrAllowedImageExtensions;
            }
            set
            {
                pstrAllowedImageExtensions = value;
            }
        }

        /// <summary>
        ///  Allowed document file extensions
        /// </summary>
        public string AllowedFileExtensions
        {
            get
            {
                return pstrAllowedFileExtensions;
            }
            set
            {
                pstrAllowedFileExtensions = value;
            }
        }

        /// <summary>
        ///  Allowed video file extensions
        /// </summary>
        public string AllowedVideoExtensions
        {
            get
            {
                return pstrAllowedVideoExtensions;
            }
            set
            {
                pstrAllowedVideoExtensions = value;
            }
        }

        /// <summary>
        ///  Allowed music file extensions
        /// </summary>
        public string AllowedMusicExtensions
        {
            get
            {
                return pstrAllowedMusicExtensions;
            }
            set
            {
                pstrAllowedMusicExtensions = value;
            }
        }

        /// <summary>
        ///  Allowed misc file extensions
        /// </summary>
        public string AllowedMiscExtensions
        {
            get
            {
                return pstrAllowedMiscExtensions;
            }
            set
            {
                pstrAllowedMiscExtensions = value;
            }
        }

        /// <summary>
        /// Returns document root
        /// </summary>
        public string RootPath
        {
            get
            {
                return pstrRootPath;
            }
            set
            {
                pstrRootPath = value;
            }
        }

        /// <summary>
        /// Returns the base url of the site
        /// </summary>
        public string RootURL
        {
            get
            {
                return pstrRootURL;
            }
            set
            {
                pstrRootURL = value;
            }
        }

        /// <summary>
        /// Returns the full upload drive path
        /// </summary>
        public string UploadPath
        {
            get
            {
                return pstrUploadPath;
            }
            set
            {
                pstrUploadPath = value;
            }
        }

        /// <summary>
        /// Returns the full thumb drive path
        /// </summary>
        public string ThumbPath
        {
            get
            {
                return pstrThumbPath;
            }
            set
            {
                pstrThumbPath = value;
            }
        }

        /// <summary>
        /// Returns the setting for allowing upload of file
        /// </summary>
        public bool AllowUploadFile
        {
            get
            {
                return pboolAllowUploadFile;
            }
            set
            {
                pboolAllowUploadFile = value;
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of file
        /// </summary>
        public bool AllowDeleteFile
        {
            get
            {
                return pboolAllowDeleteFile;
            }
            set
            {
                pboolAllowDeleteFile = value;
            }
        }

        /// <summary>
        /// Returns the setting for allowing creation of folder
        /// </summary>
        public bool AllowCreateFolder
        {
            get
            {
                return pboolAllowCreateFolder;
            }
            set
            {
                pboolAllowCreateFolder = value;
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of folder
        /// </summary>
        public bool AllowDeleteFolder
        {
            get
            {
                return pboolAllowDeleteFolder;
            }
            set
            {
                pboolAllowDeleteFolder = value;
            }
        }

        /// <summary>
        /// Returns the setting for a custom element to fill the selected item url
        /// </summary>
        public string FillSelector
        {
            get
            {
                var targetId = HttpContext.Current.Request["targetId"];
                if (!string.IsNullOrEmpty(targetId))
                {
                    return pstrFillSelector.Replace("demotextbox", targetId);
                }

                return pstrFillSelector;
            }
            set
            {
                pstrFillSelector = value;
            }
        }

        /// <summary>
        /// Returns the setting for custom code to close the popup
        /// </summary>
        public string PopupCloseCode
        {
            get
            {
                return pstrPopupCloseCode;
            }
            set
            {
                pstrPopupCloseCode = value;
            }
        }

        #endregion Settings Properties
    }
}