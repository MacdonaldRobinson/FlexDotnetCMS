using FrameworkLibrary;
using System.Linq;

namespace WebApplication.Controls.EmailTemplates.CreateAccount
{
    public partial class AutoResponder : System.Web.UI.UserControl, IEmailTemplate
    {
        private User user;
        private AutoResponderMode mode;

        public enum AutoResponderMode
        {
            NewAccount,
            RetrievePassword
        }

        public string GetHeading()
        {
            switch (this.mode)
            {
                case AutoResponderMode.RetrievePassword:
                    return "Your information for " + URIHelper.BaseUrl + " is below";
                    break;

                default:
					return "Your information for " + URIHelper.BaseUrl + " is below";
					break;
            }
        }

        public string GetPassword()
        {
            if (User.Password == "")
                return "Your Windows Login";
            else
                return User.Password;
        }

        public User User
        {
            get { return user; }
        }

        public void SetParams(object[] parameters)
        {
            this.user = (User)parameters.Where(i => i.GetType() == typeof(User) || i.GetType().BaseType == typeof(User) ).SingleOrDefault();
            this.mode = (AutoResponderMode)parameters.Where(i => i.GetType() == typeof(AutoResponderMode)).SingleOrDefault();
        }
    }
}