using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkLibrary
{
    public class UrlRedirectRulesMapper : MediaDetailsMapper
    {
        public new static IEnumerable<IMediaDetail> GetAll()
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == MediaTypeEnum.UrlRedirectRule.ToString()); //return FilterByMediaType(MediaDetailsMapper.GetAll(), MediaTypeEnum.UrlRedirectRule);
        }

        public static IEnumerable<IMediaDetail> GetAllActive()
        {
            return FilterByIsHistoryStatus(FilterOutDeletedAndArchived(FilterByCanRenderStatus(GetAll(), true)), false);
        }

        public static IEnumerable<IMediaDetail> GetAllActiveByParent(IMediaDetail parent)
        {
            return GetAllActive().Where(i => i.VirtualPath.StartsWith(parent.VirtualPath));
        }

        public static UrlRedirectRule GetRuleForUrl(string url)
        {
            var slug = StringHelper.CreateSlug(HttpContext.Current.Request.Url.Host);

            if (url.StartsWith("~/" + slug))
                url = url.Replace("~/" + slug, "~");

            var currentWebsiteVirtualPath = "/";
            var currentWebsite = WebsitesMapper.GetWebsite();

            if (currentWebsite != null)
                currentWebsiteVirtualPath = currentWebsite.VirtualPath;

            url = URIHelper.ConvertToAbsUrl(url);
            var rule = GetAllActiveByParent(currentWebsite).Where(i => URIHelper.ConvertToAbsUrl(((UrlRedirectRule)i).VirtualPathToRedirect) == url).FirstOrDefault();

            return (UrlRedirectRule)rule;
        }
    }
}