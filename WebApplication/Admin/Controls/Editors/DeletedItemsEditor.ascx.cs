using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Admin.Controls
{
    public partial class DeletedItemsEditor : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var items = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.IsDeleted);  //MediaDetailsMapper.FilterByIsHistoryStatus(MediaDetailsMapper.FilterByDeletedStatus(MediaDetailsMapper.GetAll(), true), false);
            MediaDetailsGrid.SetItems(items);
        }
    }
}