using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Controls
{
    public partial class Search : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SearchBtn_OnClick(object sender, EventArgs e)
        {
            /*var items = MediaDetailsMapper.SearchForTerm(SearchTerms.Text);
            var rssItems = items.Select(item => item.GetRssItem());

            SearchResults.SearchResultItems = rssItems;
            SearchResults.SearchResultText = SearchTerms.Text;*/

            //Response.Redirect("~/search/?q=" + SearchTerms.Text);
        }
    }
}