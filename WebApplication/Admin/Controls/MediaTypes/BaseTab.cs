using FrameworkLibrary;
using System.Collections.Generic;

namespace WebApplication.Admin
{
    public class BaseTab : System.Web.UI.UserControl
    {
        protected IMediaDetail selectedItem = null;

        protected AdminBasePage BasePage
        {
            get { return (AdminBasePage)this.Page; }
        }
    }
}