using System;
using System.Collections;
using System.IO;
using System.Web;
using FrameworkLibrary;

namespace TinyFileManager.NET
{
    public partial class dialog : System.Web.UI.Page
    {
        public string strType;
        public string strApply;
        public string strCmd;
        public string strFolder;
        public string strFile;
        public string strLang;
        public string strEditor;
        public string strCurrPath;
        public string strCurrLink;      // dialog.aspx?editor=.... for simplicity
        public ArrayList arrLinks = new ArrayList();
        public string strAllowedFileExt;
        public clsConfig objConfig = null;

        private int intColNum;
        private string[] arrFolders;
        private string[] arrFiles;
        private TinyFileManager.NET.clsFileItem objFItem;
        private bool boolOnlyImage;
        private bool boolOnlyVideo;
        private string strProfile;
        private string baseUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FrameworkSettings.CurrentUser == null)
            {
                throw new Exception("You must be logged into the CMS inorder to access the file manager");
            }

            strCmd = Request.QueryString["cmd"] + "";
            strType = Request.QueryString["type"] + "";
            strFolder = Request.QueryString["folder"] + "";
            strFile = Request.QueryString["file"] + "";
            strLang = Request.QueryString["lang"] + "";      //not used right now, but grab it
            strEditor = Request.QueryString["editor"] + "";
            strCurrPath = Request.QueryString["currpath"] + "";
            strProfile = Request.QueryString["profile"] + "";
            baseUrl = Request.QueryString["BaseUrl"] + "";
            var defaultRootPath = "/media/uploads/";


            if (!FrameworkSettings.CurrentUser.HasPermission(PermissionsEnum.Delete))
            {
                var requestPath = URIHelper.ConvertAbsUrlToTilda(strCurrPath).Replace("\\", "/").Replace("//", "/").Replace("~/", "/");

                if (!requestPath.Contains(defaultRootPath))
                {
                    strCurrPath = defaultRootPath;
                }
            }

            if (string.IsNullOrEmpty(baseUrl))
            {
                var protocal = (Request.IsSecureConnection) ? "https://" : "http://";

                baseUrl = protocal + Request.Url.Authority;
            }

            if (!strCurrPath.StartsWith("http") && File.Exists(Server.MapPath(strCurrPath)))
            {
                var fileInfo = new FileInfo(Server.MapPath(strCurrPath));
                var dir = fileInfo.Directory.FullName;

                var absUrl = URIHelper.ConvertAbsPathToAbsUrl(dir);                

                strCurrPath = URIHelper.ConvertAbsUrlToTilda(absUrl);
            }

            if(strCurrPath.StartsWith("~/"))
            {
                strCurrPath = strCurrPath.Replace("~/", "/");
            }

            if ((strCurrPath == "") || strCurrPath.Contains("//") || !Directory.Exists(Server.MapPath(strCurrPath)))
                strCurrPath = defaultRootPath;            

            if (strCurrPath.Contains(baseUrl))
            {
                strCurrPath = strCurrPath.Replace(baseUrl, "");
            }

            strCurrPath = strCurrPath.Replace("/", "\\").Replace("\\\\", "\\");

            // load config
            this.objConfig = new clsConfig(strProfile);

            //check inputs
            if (this.strCurrPath.Length > 0)
            {
                this.strCurrPath = this.strCurrPath.TrimEnd('\\');

                if (!this.strCurrPath.EndsWith("\\"))
                    this.strCurrPath = this.strCurrPath + "\\";
            }

            //set the apply string, based on the passed type
            if (this.strType == "")
            {
                this.strType = "0";
            }
            switch (this.strType)
            {
                case "1":
                    this.strApply = "apply_img";
                    this.boolOnlyImage = true;
                    this.strAllowedFileExt = this.objConfig.strAllowedImageExtensions;
                    break;

                case "2":
                    this.strApply = "apply_link";
                    this.strAllowedFileExt = this.objConfig.strAllowedAllExtensions;
                    break;

                default:
                    if (Convert.ToInt32(this.strType) >= 3)
                    {
                        this.strApply = "apply_video";
                        this.boolOnlyVideo = true;
                        this.strAllowedFileExt = this.objConfig.strAllowedVideoExtensions;
                    }
                    else
                    {
                        this.strApply = "apply";
                        this.strAllowedFileExt = this.objConfig.strAllowedAllExtensions;
                    }
                    break;
            }

            //setup current link
            strCurrLink = "dialog.aspx?type=" + this.strType + "&editor=" + this.strEditor + "&lang=" + this.strLang + "&profile=" + this.strProfile + "&targetId=" + Request["targetId"];

            switch (strCmd)
            {
                case "debugsettings":
                    Response.Write("<style>");
                    Response.Write("body {font-family: Verdana; font-size: 10pt;}");
                    Response.Write(".table {display: table; border-collapse: collapse; margin: 20px; background-color: #e7e5e5;}");
                    Response.Write(".tcaption {display: table-caption; padding: 5px; font-size: 14pt; font-weight: bold; background-color: #9fcff7;}");
                    Response.Write(".tr {display: table-row;}");
                    Response.Write(".tr:hover {background-color: #f0f2f3;}");
                    Response.Write(".td {display: table-cell; padding: 5px; border: 1px solid #a19e9e;}");
                    Response.Write("</style>");

                    Response.Write("<div class=\"table\">");   //start table

                    Response.Write("<div class=\"tcaption\">Operating Settings</div>");  //caption

                    Response.Write("<div class=\"tbody\">");  //start body

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowCreateFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.boolAllowCreateFolder + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFile:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.boolAllowDeleteFile + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.boolAllowDeleteFolder + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowUploadFile:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.boolAllowUploadFile + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>MaxUploadSizeMb:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.intMaxUploadSizeMb + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedAllExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedAllExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedFileExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedFileExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedImageExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedImageExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedMiscExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedMiscExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedMusicExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedMusicExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedVideoExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strAllowedVideoExtensions + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>BaseURL:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strBaseURL + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>DocRoot:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strDocRoot + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>ThumbPath:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strThumbPath + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>ThumbURL:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strThumbURL + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>UploadPath:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strUploadPath + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>UploadURL:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strUploadURL + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>FillSelector:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strFillSelector + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>PopupCloseCode:</b></div>");
                    Response.Write("<div class=\"td\">" + this.objConfig.strPopupCloseCode + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("</div>");   //end body
                    Response.Write("</div>");   //end table

                    Response.End();
                    break;

                case "createfolder":
                    try
                    {
                        strFolder = Request.Form["folder"] + "";
                        //forge ahead without checking for existence
                        //catch will save us
                        Directory.CreateDirectory(this.objConfig.strUploadPath + "\\" + strFolder);
                        Directory.CreateDirectory(this.objConfig.strThumbPath + "\\" + strFolder);

                        // end response, since it's an ajax call
                        Response.End();
                    }
                    catch
                    {
                        //TODO: write error
                    }
                    break;

                case "upload":
                    strFolder = Request.Form["folder"] + "";
                    HttpPostedFile filUpload = Request.Files["file"];
                    string strTargetFile;
                    string strThumbFile;

                    //check file was submitted
                    if ((filUpload != null) && (filUpload.ContentLength > 0))
                    {
                        var name = new FileInfo(filUpload.FileName).Name.ToLower();

                        strTargetFile = this.objConfig.strUploadPath + this.strFolder + name;
                        strThumbFile = this.objConfig.strThumbPath + this.strFolder + name;
                        filUpload.SaveAs(strTargetFile);

                        /*if (this.isImageFile(strTargetFile))
                        {
                            this.createThumbnail(strTargetFile, strThumbFile);
                        }*/
                    }

                    // end response
                    if (Request.Form["fback"] == "true")
                    {
                        Response.Redirect(this.strCurrLink);
                    }
                    else
                    {
                        Response.End();
                    }

                    break;

                case "download":
                    FileInfo objFile = new FileInfo(this.objConfig.strUploadPath + "\\" + this.strFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Pragma", "private");
                    Response.AddHeader("Cache-control", "private, must-revalidate");
                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Length", objFile.Length.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(this.strFile));
                    Response.WriteFile(this.objConfig.strUploadPath + "\\" + this.strFile);
                    break;

                case "delfile":
                    try
                    {
                        File.Delete(this.objConfig.strUploadPath + "\\" + this.strFile);
                        if (File.Exists(this.objConfig.strThumbPath + "\\" + this.strFile))
                        {
                            File.Delete(this.objConfig.strThumbPath + "\\" + this.strFile);
                        }
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;

                case "delfolder":
                    try
                    {
                        Directory.Delete(this.objConfig.strUploadPath + "\\" + strFolder, true);
                        Directory.Delete(this.objConfig.strThumbPath + "\\" + strFolder, true);
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;

                default:    //just a regular page load
                    if (this.strCurrPath != "")
                    {
                        // add "up one" folder
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = "..";
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.boolIsFolderUp = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.getUpOneDir(this.strCurrPath);
                        this.objFItem.strClassType = "dir";
                        this.objFItem.strDeleteLink = "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        this.objFItem.strThumbImage = "img/ico/folder_return.png";
                        this.objFItem.strLink = "<a title=\"Open\" href=\"" + this.strCurrLink + "&currpath=" + this.objFItem.strPath + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>..</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    if (this.strCurrPath.Contains(this.objConfig.strUploadPath))
                        this.strCurrPath = defaultRootPath;

                    //load folders
                    arrFolders = Directory.GetDirectories(this.objConfig.strUploadPath + this.strCurrPath);

                    var renameLink = "<a href='javascript:void(0)' class='btn btn-rename'><i class='fa fa-pencil' aria-hidden='true'></i></a>";

                    foreach (string strF in arrFolders)
                    {
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = Path.GetFileName(strF);
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.strCurrPath + Path.GetFileName(strF);
                        this.objFItem.strClassType = "dir";
                        if (this.objConfig.boolAllowDeleteFolder)
                        {
                            this.objFItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfolder&folder=" + this.objFItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button top-right\" onclick=\"return confirm('Are you sure you want to delete this folder? NOTE: All the subfolders and files under this folder will also be deleted and there is no way of getting them back.');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                            this.objFItem.strDeleteLink = this.objFItem.strDeleteLink + renameLink;
                        }
                        else
                        {
                            this.objFItem.strDeleteLink = "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        this.objFItem.strThumbImage = "img/ico/folder.png";
                        this.objFItem.strLink = "<a title=\"Open\" href=\"" + this.strCurrLink + "&currpath=" + this.objFItem.strPath + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>" + this.objFItem.strName + "</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    // load files
                    arrFiles = Directory.GetFiles(this.objConfig.strUploadPath + this.strCurrPath);
                    foreach (string strF in arrFiles)
                    {
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = Path.GetFileNameWithoutExtension(strF);
                        this.objFItem.boolIsFolder = false;
                        this.objFItem.strPath = this.strCurrPath + Path.GetFileName(strF);
                        this.objFItem.boolIsImage = this.isImageFile(Path.GetFileName(strF));
                        this.objFItem.boolIsVideo = this.isVideoFile(Path.GetFileName(strF));
                        this.objFItem.boolIsMusic = this.isMusicFile(Path.GetFileName(strF));
                        this.objFItem.boolIsMisc = this.isMiscFile(Path.GetFileName(strF));

                        // check to see if it's the type of file we are looking at
                        if ((this.boolOnlyImage && this.objFItem.boolIsImage) || (this.boolOnlyVideo && this.objFItem.boolIsVideo) || (!this.boolOnlyImage && !this.boolOnlyVideo))
                        {
                            this.objFItem.intColNum = this.getNextColNum();
                            // get display class type
                            if (this.objFItem.boolIsImage)
                            {
                                this.objFItem.strClassType = "2";
                            }
                            else
                            {
                                if (this.objFItem.boolIsMisc)
                                {
                                    this.objFItem.strClassType = "3";
                                }
                                else
                                {
                                    if (this.objFItem.boolIsMusic)
                                    {
                                        this.objFItem.strClassType = "5";
                                    }
                                    else
                                    {
                                        if (this.objFItem.boolIsVideo)
                                        {
                                            this.objFItem.strClassType = "4";
                                        }
                                        else
                                        {
                                            this.objFItem.strClassType = "1";
                                        }
                                    }
                                }
                            }
                            // get delete link
                            if (this.objConfig.boolAllowDeleteFile)
                            {
                                this.objFItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfile&file=" + this.objFItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure you want to delete this file? NOTE: There is no way to get this file back once you delete it');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                            }
                            else
                            {
                                this.objFItem.strDeleteLink = "<a class=\"btn erase-button disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                            }
                            // get thumbnail image
                            if (this.objFItem.boolIsImage)
                            {
                                // first check to see if thumb exists
                                if (!File.Exists(this.objConfig.strThumbPath + this.objFItem.strPath))
                                {
                                    // thumb doesn't exist, create it
                                    strTargetFile = this.objConfig.strUploadPath + this.objFItem.strPath;
                                    strThumbFile = this.objConfig.strThumbPath + this.objFItem.strPath;
                                    this.createThumbnail(strTargetFile, strThumbFile);
                                }

                                if (this.objFItem.strPath.StartsWith("\\"))
                                    this.objFItem.strPath = this.objFItem.strPath.Substring(1);

                                this.objFItem.strThumbImage = (this.objConfig.strThumbURL + this.objFItem.strPath.Replace('\\', '/'));
                            }
                            else
                            {
                                if (File.Exists(Directory.GetParent(Request.PhysicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png"))
                                {
                                    this.objFItem.strThumbImage = "img/ico/" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png";
                                }
                                else
                                {
                                    this.objFItem.strThumbImage = "img/ico/Default.png";
                                }
                            }
                            this.objFItem.strDownFormOpen = "<form action=\"dialog.aspx?cmd=download&file=" + this.objFItem.strPath + "\" method=\"post\" class=\"download-form\">";


                            var path = strF.Replace(Server.MapPath("~/"), "").Replace("\\", "/").Replace("///", "/");

                            if (!path.StartsWith("/"))
                                path = "/" + path;

                            if (this.objFItem.boolIsImage)
                            {
                                this.objFItem.strPreviewLink = "<a class=\"btn preview\" title=\"Preview\" data-url=\"" + this.objConfig.strUploadURL + "/" + this.objFItem.strPath + "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a><a class='btn' href='"+ baseUrl + path + "' target='_blank'><i class='fa fa-external-link'></i></a><a class='btn' href='/Admin/Views/PageHandlers/FileEditor/Default.aspx?LoadFile=" + path + "' target='_blank'><i class='fa fa-external-link'></i></a>";
                            }
                            else
                            {
                                this.objFItem.strPreviewLink = "<a class=\"btn preview disabled\" title=\"Preview\"><i class=\"icon-eye-open\"></i></a> <a class='btn' href='" + baseUrl + path + "' target='_blank'><i class='fa fa-external-link'></i></a>";
                            }

                            this.objFItem.strPreviewLink = this.objFItem.strPreviewLink + renameLink;
                            
                            this.objFItem.strLink = "<a href=\"#\" title=\"Select\" onclick=\"" + this.strApply + "('" + baseUrl + path + "'," + this.strType + ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" + this.objFItem.strThumbImage + "\" height=\"100\"><h4>" + this.objFItem.strName + new FileInfo(this.objFItem.strPath).Extension + "</h4></a>";

                            this.arrLinks.Add(objFItem);
                        }
                    } // foreach

                    break;
            }   // switch
        }   // page load

        public string getBreadCrumb()
        {
            string strRet;
            string[] arrFolders;
            string strTempPath = "";
            int intCount = 0;

            strRet = "<li><a href=\"" + this.strCurrLink + "&currpath=\"><i class=\"icon-home\"></i></a>";
            arrFolders = this.strCurrPath.Split('\\');

            foreach (string strFolder in arrFolders)
            {
                if (strFolder != "")
                {
                    strTempPath += strFolder + "\\";
                    intCount++;

                    if (intCount == (arrFolders.Length - 1))
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li class=\"active\">" + strFolder + "</li>";
                    }
                    else
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li><a href=\"" + this.strCurrLink + "&currpath=\\" + strTempPath + "\">" + strFolder + "</a>";
                    }
                }
            }   // foreach

            return strRet;
        }   // getBreadCrumb

        private bool isImageFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(this.objConfig.arrAllowedImageExtensions, Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of image file extensions
        } // isImageFile

        private bool isVideoFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(this.objConfig.arrAllowedVideoExtensions, Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of video file extensions
        } // isVideoFile

        private bool isMusicFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(this.objConfig.arrAllowedMusicExtensions, Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of music file extensions
        } // isMusicFile

        private bool isMiscFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(this.objConfig.arrAllowedMiscExtensions, Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of misc file extensions
        } // isMiscFile

        private void createThumbnail(string strFilename, string strThumbFilename)
        {
            System.Drawing.Image.GetThumbnailImageAbort objCallback;
            System.Drawing.Image objFSImage;
            System.Drawing.Image objTNImage;
            System.Drawing.RectangleF objRect;
            System.Drawing.GraphicsUnit objUnits = System.Drawing.GraphicsUnit.Pixel;
            int intHeight = 0;
            int intWidth = 0;

            // open image and get dimensions in pixels
            objFSImage = System.Drawing.Image.FromFile(strFilename);
            objRect = objFSImage.GetBounds(ref objUnits);

            // what are we going to resize to, to fit inside 156x78
            getProportionalResize(Convert.ToInt32(objRect.Width), Convert.ToInt32(objRect.Height), ref intWidth, ref intHeight);

            // create thumbnail
            objCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            objTNImage = objFSImage.GetThumbnailImage(intWidth, intHeight, objCallback, IntPtr.Zero);

            // finish up
            objFSImage.Dispose();
            objTNImage.Save(strThumbFilename);
            objTNImage.Dispose();
        } // createThumbnail

        private void getProportionalResize(int intOldWidth, int intOldHeight, ref int intNewWidth, ref int intNewHeight)
        {
            int intHDiff = 0;
            int intWDiff = 0;
            decimal decProp = 0;
            int intTargH = 78;
            int intTargW = 156;

            if ((intOldHeight <= intTargH) && (intOldWidth <= intTargW))
            {
                // no resize needed
                intNewHeight = intOldHeight;
                intNewWidth = intOldWidth;
                return;
            }

            //get the differences between desired and current height and width
            intHDiff = intOldHeight - intTargH;
            intWDiff = intOldWidth - intTargW;

            //whichever is the bigger difference is the chosen proportion
            if (intHDiff > intWDiff)
            {
                decProp = (decimal)intTargH / (decimal)intOldHeight;
                intNewHeight = intTargH;
                intNewWidth = Convert.ToInt32(Math.Round(intOldWidth * decProp, 0));
            }
            else
            {
                decProp = (decimal)intTargW / (decimal)intOldWidth;
                intNewWidth = intTargW;
                intNewHeight = Convert.ToInt32(Math.Round(intOldHeight * decProp, 0));
            }
        } // getProportionalResize

        private bool ThumbnailCallback()
        {
            return false;
        } // ThumbnailCallback

        public string getEndOfLine(int intColNum)
        {
            if (intColNum == 6)
            {
                return "</div><div class=\"space10\"></div>";
            }
            else
            {
                return "";
            }
        } // getEndOfLine

        public string getStartOfLine(int intColNum)
        {
            if (intColNum == 1)
            {
                return "<div class=\"row-fluid\">";
            }
            else
            {
                return "";
            }
        } // getStartOfLine

        private int getNextColNum()
        {
            this.intColNum++;
            if (this.intColNum > 6)
            {
                this.intColNum = 1;
            }
            return this.intColNum;
        } // getNextColNum

        private string getUpOneDir(string strInput)
        {
            string[] arrTemp;

            arrTemp = strInput.TrimEnd('\\').Split('\\');
            arrTemp[arrTemp.Length - 1] = "";
            return String.Join("\\", arrTemp);
        }
    }   // class
}   // namespace