using FrameworkLibrary;
using System;
using System.Collections.Generic;

namespace WebApplication.Controls
{
    public partial class SearchResults : System.Web.UI.UserControl
    {
        public static IEnumerable<RssItem> SearchResultItems
        {
            get
            {
                return (IEnumerable<RssItem>)ContextHelper.Get("SearchResultItems", ContextType.Session);
            }
            set
            {
                ContextHelper.Set("SearchResultItems", value, ContextType.Session);
            }
        }

        public static string SearchResultText
        {
            get
            {
                return (string)ContextHelper.Get("SearchResultText", ContextType.Session);
            }
            set
            {
                ContextHelper.Set("SearchResultText", value, ContextType.Session);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Bind();
        }

        private void Bind()
        {
            //RssListView.Items = SearchResultItems;
        }
    }
}