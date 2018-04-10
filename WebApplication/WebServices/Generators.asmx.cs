using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FrameworkLibrary;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Generators
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Generators : BaseService
    {
        [WebMethod]
        public void GoogleSiteMap()
        {
            var mediaDetails = BaseMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && !i.IsDeleted && ((i.ShowInMenu) || (i.MediaType.Name =="Blog" || i.MediaType.Name =="BlogPost")) && i.PublishDate < DateTime.Now && i.MediaType.ShowInSiteTree).ToList().Where(i=>i.CanRender);
            var xmlEntries = "";
            foreach (var mediaDetail in mediaDetails)
            {
                xmlEntries+= ConvertMediaDetailToSiteMapXmlEntry(mediaDetail);
            }

            var xml = $@"<?xml version='1.0' encoding='UTF-8'?>
<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'> 
    {xmlEntries}
</urlset>";
            WriteXML(xml);
        }

        private string ConvertMediaDetailToSiteMapXmlEntry(IMediaDetail mediaDetail)
        {
            var entryXML = $@"<url>
    <loc>{mediaDetail.AbsoluteUrl}</loc> 
	<lastmod>{mediaDetail.DateLastModified.ToString("o")}</lastmod>
</url>";

            return entryXML;
        }
    }
}
