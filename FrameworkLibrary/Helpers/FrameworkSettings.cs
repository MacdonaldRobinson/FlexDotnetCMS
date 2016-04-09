using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;

namespace FrameworkLibrary
{
    public class FrameworkSettings
    {
        public static Language GetCurrentLanguage()
        {
            Language language = (Language)ContextHelper.Get(SelectedLanguageKey, ContextType.Session);

            if (language == null)
            {
                language = (Language)ContextHelper.Get(SelectedLanguageKey, ContextType.RequestContext);

                if (language == null)
                {
                    language = LanguagesMapper.GetDefaultLanguage();
                    ContextHelper.Set(SelectedLanguageKey, language, ContextType.RequestContext);
                }
            }

            if ((language != null) &&  (Thread.CurrentThread.CurrentCulture.Name != language.CultureCode))
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language.CultureCode);

            return language;
        }

        public static bool SetCurrentLanguage(Language language)
        {
            bool wasSet = ContextHelper.Set(SelectedLanguageKey, language, ContextType.Session);

            if (!wasSet)
                ContextHelper.Set(SelectedLanguageKey, language, ContextType.RequestContext);

            return wasSet;
        }

        /*public static IMediaDetail RootMediaDetail
        {
            get
            {
                var rootMediaDetail = (IMediaDetail)ContextHelper.GetFromRequestContext("RootMediaDetail");

                if (rootMediaDetail != null)
                    return rootMediaDetail;
                else
                {
                    rootMediaDetail = MediaDetailsMapper.GetByMedia(RootMedia, GetCurrentLanguage());

                    if (rootMediaDetail == null)
                        rootMediaDetail = MediaDetailsMapper.GetByMedia(RootMedia, LanguagesMapper.GetDefaultLanguage());

                    ContextHelper.SetToRequestContext("RootMediaDetail", rootMediaDetail);

                    return rootMediaDetail;
                }
            }
        }*/

        public static Dictionary<IMediaDetail, User> CheckedOutItems
        {
            get
            {
                Dictionary<IMediaDetail, User> items = (Dictionary<IMediaDetail, User>)ContextHelper.Get("CheckedOutItems", ContextType.Cache);

                if (items == null)
                {
                    items = new Dictionary<IMediaDetail, User>();
                    ContextHelper.Set("CheckedOutItems", items, ContextType.Cache);
                }

                return items;
            }
        }

        public static FrameworkBaseMedia CurrentFrameworkBaseMedia
        {
            get
            {
                return (FrameworkBaseMedia)ContextHelper.Get("CurrentFrameworkBaseMedia", ContextType.RequestContext);
            }
            set
            {
                ContextHelper.Set("CurrentFrameworkBaseMedia", value, ContextType.RequestContext);
            }
        }

        public static User CurrentUser
        {
            get
            {
                User user = (User)ContextHelper.Get("CurrentUser", ContextType.Session);

                if (user != null)
                    user = UsersMapper.GetByUserName(user.UserName);

                if (user == null)
                {
                    if (HttpContext.Current.User != null)
                    {
                        user = new User(new CustomIdentity(HttpContext.Current.User.Identity.Name, HttpContext.Current.User.Identity.IsAuthenticated, HttpContext.Current.User.Identity.AuthenticationType));

                        if (user != null)
                            user = UsersMapper.GetByUserName(user.UserName);

                        ContextHelper.Set("CurrentUser", user, ContextType.Session);
                    }
                }

                user = BaseMapper.GetObjectFromContext(user);

                if (System.Web.HttpContext.Current.User == null)
                    user = null;

                if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    user = null;

                return user;
            }
            set
            {
                ContextHelper.Set("CurrentUser", value, ContextType.Session);
            }
        }

        public static string GetPageNotFoundHandler(Language language)
        {
            return WebsitesMapper.GetWebsite(0, language).AutoCalculatedVirtualPath + "page-not-found/";
        }

        /*public static Media RootMedia
        {
            get
            {
                var rootMedia = (Media)ContextHelper.GetFromRequestContext("RootMedia");

                if (rootMedia != null)
                    return rootMedia;
                else
                {
                    rootMedia = MediasMapper.GetRootMedia();
                    ContextHelper.SetToRequestContext("RootMedia", rootMedia);
                    return rootMedia;
                }
            }
        }*/

        public static string SelectedLanguageKey
        {
            get
            {
                return "language";
            }
        }

        public static string ConnectionStringName
        {
            get
            {
                return "Entities";
            }
        }
    }
}