using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TinyFileManager.NET
{
    public class clsFileItem
    {
        public string strName;
        public int intColNum;       // list is 6 columns wide, which one is this?
        public bool boolIsFolder;
        public bool boolIsFolderUp;
        public bool boolIsImage;
        public bool boolIsVideo;
        public bool boolIsMusic;
        public bool boolIsMisc;
        public string strPath;
        public string strLink;
        public string strClassType;
        public string strDeleteLink;
        public string strPreviewLink;
        public string strThumbImage;
        public string strDownFormOpen;
    }
}