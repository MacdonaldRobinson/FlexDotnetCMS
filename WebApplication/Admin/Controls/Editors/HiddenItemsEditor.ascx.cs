using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Admin.Controls
{
    public partial class HiddenItemsEditor : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var items = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => !i.IsHistory && !i.ShowInMenu); //MediaDetailsMapper.FilterByIsHistoryStatus(MediaDetailsMapper.FilterByShowInMenuStatus(MediaDetailsMapper.GetAll(), false), false);
            MediaDetailsGrid.SetItems(items);
        }
    }
}