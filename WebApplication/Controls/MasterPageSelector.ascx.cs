using FrameworkLibrary;
using System;
using System.Linq;

namespace WebApplication.Admin.Controls.Selectors
{
    public partial class MasterPageSelector : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            MasterPages.DataSource = MasterPagesMapper.GetAll().ToList();
            MasterPages.DataTextField = "Name";
            MasterPages.DataValueField = "ID";
            MasterPages.DataBind();
        }

        public string SelectedValue
        {
            get
            {
                return MasterPages.SelectedValue;
            }
            set
            {
                MasterPages.SelectedValue = value;
            }
        }
    }
}