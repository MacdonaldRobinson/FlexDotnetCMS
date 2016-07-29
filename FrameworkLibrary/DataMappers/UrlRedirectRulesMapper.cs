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
                virtualPath = virtualPath.Replace("~","");

            var enumName = MediaTypeEnum.UrlRedirectRule.ToString();

            var rule = BaseMapper.GetDataModel().MediaDetails.FirstOrDefault(i => i.HistoryVersionNumber == 0 && !i.IsDeleted && i.PublishDate <= DateTime.Now && (i.ExpiryDate == null || i.ExpiryDate >= DateTime.Now) && i.MediaType.Name == enumName && (i as UrlRedirectRule).VirtualPathToRedirect == virtualPath);

            return (UrlRedirectRule)rule;
        }
    }
}