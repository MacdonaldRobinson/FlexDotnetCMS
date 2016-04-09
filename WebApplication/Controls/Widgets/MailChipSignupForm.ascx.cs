using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Controls
{
    public partial class MailChipSignupForm : System.Web.UI.UserControl
    {
        //private MailChimpManager MailChimpManager = new MailChimpManager("1168adefb7ccdbaff9e3b04b9fbfbcc1-us9");
        private string _MailChimpButtonText = "Sign Up";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            /*var lists = MailChimpManager.GetLists();

            //  For each list
            foreach (var list in lists.Data)
            {
                var users = MailChimpManager.GetAllMembersForList(list.Id);
            }*/

            Submit.Text = _MailChimpButtonText;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //var email = new EmailParameter() { Email = EmailAddress.Text };

            try
            {
                var webrequestHelper = new WebRequestHelper();
                var returnJson = webrequestHelper.MakeWebRequest("https://us9.api.mailchimp.com/2.0/lists/subscribe.json?apikey=" + AppSettings.MailChimpApiKey + "&id=" + MailChimpListID + "&email[email]=" + EmailAddress.Text);

                var jsonObject = (new JavaScriptSerializer()).Deserialize<dynamic>(returnJson);

                if (jsonObject.ContainsKey("email"))
                {
                    SuccessPanel.Visible = true;
                    ErrorPanel.Visible = false;
                }
                else if (jsonObject.ContainsKey("error"))
                {
                    SuccessPanel.Visible = false;
                    ErrorPanel.Visible = true;

                    var message = jsonObject["error"] + " ( email address provided: " + EmailAddress.Text + " ) ";

                    var exception = new Exception(message);
                    ErrorHelper.LogException(exception);
                }
            }
            catch (Exception ex)
            {
                SuccessPanel.Visible = false;
                ErrorPanel.Visible = true;

                // MessageFromServer.Text = ex.Message;

                ErrorHelper.LogException(ex);
            }
        }

        public string MailChimpListID { get; set; }

        public string MailChimpButtonText
        {
            get
            {
                return _MailChimpButtonText;
            }
            set
            {
                _MailChimpButtonText = value;
            }
        }
    }
}