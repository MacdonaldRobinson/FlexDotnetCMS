using FrameworkLibrary;
using System;
using System.ComponentModel;
using System.Web;

namespace WebApplication.Controls
{
    public partial class ContactForm : System.Web.UI.UserControl
    {
        private string formTitle = "Contact Form";
        private string thankYouPage = "";
        private string subject = "";
        private string toEmailAddresses = "";
        private string emailTemplatePath = "~/Controls/EmailTemplates/ContactUs.ascx";
        private string submitButtonText = "Submit";

        protected void Page_Load(object sender, EventArgs e)
        {
            TitleLiteral.Text = formTitle;
            Submit.Text = submitButtonText;

            if (!String.IsNullOrEmpty(Request.QueryString.Get("emailAddress")))
                Email.Text = Request.QueryString.Get("emailAddress");
        }

        protected void Submit_OnClick(object sender, EventArgs e)
        {
            EmailLog obj = new EmailLog();
            obj.SenderName = Name.Text;
            obj.SenderEmailAddress = Email.Text;
            obj.Message = Message.Text;
            obj.ToEmailAddresses = ToEmailAddresses;
            obj.Subject = subject;
            obj.VisitorIP = HttpContext.Current.Request.UserHostAddress;
            obj.RequestUrl = URIHelper.GetCurrentVirtualPath(true);

            Return returnObj = EmailHelper.SendTemplate(EmailHelper.GetMailAddressesFromString(obj.ToEmailAddresses), obj.Subject, obj.SenderName, obj.SenderEmailAddress, emailTemplatePath, obj);

            if (returnObj.IsError)
                this.BasePage.DisplayErrorMessage("Error sending email", returnObj.Error);
            else
                Response.Redirect(thankYouPage);
        }

        private BasePage BasePage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string ThankYouPage
        {
            get
            {
                return this.thankYouPage;
            }
            set
            {
                this.thankYouPage = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                this.subject = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string ToEmailAddresses
        {
            get
            {
                return toEmailAddresses;
            }
            set
            {
                toEmailAddresses = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string EmailTemplatePath
        {
            get
            {
                return emailTemplatePath;
            }
            set
            {
                emailTemplatePath = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string FormTitle
        {
            get
            {
                return formTitle;
            }
            set
            {
                formTitle = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Category("ContactForm")]
        public string SubmitButtonText
        {
            get
            {
                return submitButtonText;
            }
            set
            {
                submitButtonText = value;
            }
        }
    }
}