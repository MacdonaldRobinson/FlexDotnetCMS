using FrameworkLibrary;
using System;
using System.IO;
using System.Web.Security;

namespace WebApplication.Admin
{
    public class AdminBasePage : BasePage
    {
        public AdminBasePage()
        {
            if (AppSettings.ForceSSL)
            {
                URIHelper.ForceSSL();
            }

            this.masterFilesDirPath = "~/Admin/Views/MasterPages/";
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            var currentUser = this.CurrentUser;
            if (currentUser == null)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                CurrentUser = null;

                return;
            }

            if (!currentUser.HasPermission(PermissionsEnum.AccessCMS))
                Response.Redirect("~/");

            if (this.MasterPageFile != null)
            {
                var masterFilePath = GetMasterPageFilePath();

                if (File.Exists(URIHelper.ConvertToAbsPath(masterFilePath)))
                    MasterPageFile = masterFilePath;
            }
        }

        public new static Language CurrentLanguage
        {
            get
            {
                var currentLanguage = (Language)ContextHelper.GetFromSession("CurrentAdminLanguage");

                if (currentLanguage == null)
                {
                    currentLanguage = LanguagesMapper.GetDefaultLanguage();

                    ContextHelper.SetToSession("CurrentAdminLanguage", currentLanguage);
                }

                return currentLanguage;
            }
            set
            {
                ContextHelper.SetToSession("CurrentAdminLanguage", value);
            }
        }

        public static IMediaDetail SelectedMediaDetail
        {
            get
            {
                return (IMediaDetail)ContextHelper.GetFromSession("SelectedMediaDetail");
            }
            set
            {
                ContextHelper.SetToSession("SelectedMediaDetail", value);
            }
        }

        public static Media SelectedMedia
        {
            get
            {
                return (Media)ContextHelper.GetFromSession("SelectedMedia");
            }
            set
            {
                ContextHelper.SetToSession("SelectedMedia", value);
            }
        }

        public static Media ParentMedia
        {
            get
            {
                return (Media)ContextHelper.Get("ParentMedia", ContextType.Session);
            }
            set
            {
                ContextHelper.Set("ParentMedia", value, ContextType.Session);
            }
        }

        public static MediaTypeEnum MediaTypeEnum
        {
            get
            {
                MediaTypeEnum mediaTypeEnum = FrameworkLibrary.MediaTypeEnum.Page;

                if (SelectedMediaDetail != null)
                    mediaTypeEnum = MediaTypesMapper.GetEnumByID(SelectedMediaDetail.MediaTypeID);
                else if (ContextHelper.Get("SelectedMediaTypeEnum", ContextType.Session) != null)
                    mediaTypeEnum = (MediaTypeEnum)ContextHelper.Get("SelectedMediaTypeEnum", ContextType.Session);

                return mediaTypeEnum;
            }
            set
            {
                ContextHelper.Set("SelectedMediaTypeEnum", value, ContextType.Session);
            }
        }
    }
}