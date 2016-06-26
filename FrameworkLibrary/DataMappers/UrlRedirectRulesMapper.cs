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

        public static UrlRedirectRule GetRuleForUrl(string virtualPath)
        {
            var slug = StringHelper.CreateSlug(HttpContext.Current.Request.Url.Host);

            if (virtualPath.StartsWith("~"))
                virtualPath = virtualPath.Replace("~","");

            //var rule = BaseMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.Name == MediaTypeEnum.UrlRedirectRule.ToString() && ((UrlRedirectRule)i).VirtualPathToRedirect.Trim() == virtualPath).ToList().FirstOrDefault(i=>i.CanRender);

            var rule = GetAll().Cast<UrlRedirectRule>().FirstOrDefault(i=> !i.IsHistory && i.CanRender && i.VirtualPathToRedirect == virtualPath);

            return (UrlRedirectRule)rule;
        }
    }
}