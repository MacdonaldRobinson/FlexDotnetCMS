using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkLibrary
{
    public class UrlRedirectRulesMapper : MediaDetailsMapper
    {
        public new static IEnumerable<IMediaDetail> GetAll()
        {
            var name = MediaTypeEnum.UrlRedirectRule.ToString();
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == name); //return FilterByMediaType(MediaDetailsMapper.GetAll(), MediaTypeEnum.UrlRedirectRule);
        }

        public static UrlRedirectRule CreateUrlRedirect(string fromVirtualPath, string toVirtualPath)
        {
            var currentLanuageId = FrameworkSettings.GetCurrentLanguage().ID;
            var urlRedirectList = MediaDetailsMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.MediaType.Name == MediaTypeEnum.UrlRedirectRuleList.ToString() && i.HistoryVersionNumber == 0 && i.LanguageID == currentLanuageId);
            var mediaType = MediaDetailsMapper.GetDataModel().MediaTypes.FirstOrDefault(i => i.Name == MediaTypeEnum.UrlRedirectRule.ToString());

            if (urlRedirectList != null && mediaType != null)
            {
                fromVirtualPath = fromVirtualPath.Replace("~", "");
                toVirtualPath = toVirtualPath.Replace("~", "");

                var urlRewrite = (UrlRedirectRule)MediaDetailsMapper.CreateObject(mediaType.ID, null, urlRedirectList.Media);
                urlRewrite.Is301Redirect = true;
                urlRewrite.VirtualPathToRedirect = fromVirtualPath;
                urlRewrite.RedirectToUrl = toVirtualPath;
                urlRewrite.LinkTitle = urlRewrite.SectionTitle = urlRewrite.Title = urlRewrite.VirtualPathToRedirect + " -> " + urlRewrite.RedirectToUrl;
                urlRewrite.PublishDate = DateTime.Now;

                var user = UsersMapper.GetAllByRoleEnum(RoleEnum.Administrator).FirstOrDefault(i=>i.IsActive);

                if(user != null)
                {
                    urlRewrite.CreatedByUser = urlRewrite.LastUpdatedByUser = user;
                }

                return urlRewrite;
            }

            return null;
        }

        public static IEnumerable<IMediaDetail> GetAllActive()
        {
            return FilterByIsHistoryStatus(FilterOutDeletedAndArchived(FilterByCanRenderStatus(GetAll(), true)), false);
        }

        public static IEnumerable<IMediaDetail> GetAllActiveByParent(IMediaDetail parent)
        {
            return GetAllActive().Where(i => i.VirtualPath.StartsWith(parent.VirtualPath));
        }

        public static UrlRedirectRule GetRuleForUrl(string virtualPath)
        {
            if (virtualPath.StartsWith("~"))
                virtualPath = virtualPath.Replace("~", "");

            var enumName = MediaTypeEnum.UrlRedirectRule.ToString();

            var rule = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate >= DateTime.Now) && i.MediaType.Name == enumName && ((i as UrlRedirectRule).VirtualPathToRedirect.ToLower() == virtualPath || (i as UrlRedirectRule).VirtualPathToRedirect.ToLower() == virtualPath+"/"));

            if (rule != null)
            {
                var urlredirectrule = rule as UrlRedirectRule;

                var toUrl = urlredirectrule.RedirectToUrl;

                if (toUrl.Contains("{"))
                {
                    toUrl = MediaDetailsMapper.ParseSpecialTags(rule, urlredirectrule.RedirectToUrl);
                }

                if (!toUrl.EndsWith(virtualPath))
                    return urlredirectrule;
            }

            return null;
        }
    }
}