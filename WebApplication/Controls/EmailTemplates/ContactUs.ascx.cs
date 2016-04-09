using FrameworkLibrary;
using System.Linq;
using System.Web.Services.Description;

namespace WebApplication.Controls.EmailTemplates
{
    public partial class ContactUs : System.Web.UI.UserControl, IEmailTemplate
    {
        public void SetParams(object[] parameters)
        {
            var obj = (EmailLog)parameters.Where(i => i.GetType() == typeof(EmailLog)).SingleOrDefault();

            SenderName.Text = obj.SenderName;
            SenderEmailAddress.Text = obj.SenderEmailAddress;
            Message.Text = obj.Message;
        }
    }
}