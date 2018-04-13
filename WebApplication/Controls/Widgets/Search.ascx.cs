using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls
{
    public partial class Search : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			//if (Request["q"] != null)
			//{
			//	SearchForTerm(Request["q"]);
			//}
        }

		private void SearchForTerm(string searchTerm)
		{
			//var items = MediaDetailsMapper.SearchForTerm(searchTerm);
			//var rssItems = items.Select(item => item.GetRssItem());

			//SearchResults.SearchResultItems = rssItems;
			//SearchResults.SearchResultText = SearchTerms.Text;
		}

        protected void SearchBtn_OnClick(object sender, EventArgs e)
        {

            //Response.Redirect("~/search/?q=" + SearchTerms.Text);
        }
    }
}