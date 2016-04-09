using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Admin.Controls
{
    public partial class UnpublishedAndExpiredItems : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var items = new List<IMediaDetail>();
            items.AddRange(MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && (i.PublishDate > DateTime.Now || i.ExpiryDate <= DateTime.Now))); //MediaDetailsMapper.FilterByIsHistoryStatus(MediaDetailsMapper.FilterByPublishedStatus(MediaDetailsMapper.GetAll(), false), false)

            MediaDetailsGrid.SetItems(items.Distinct());
        }
    }
}