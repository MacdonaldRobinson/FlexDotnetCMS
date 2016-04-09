using FrameworkLibrary;
using System.Linq;

namespace WebApplication.Controls.EmailTemplates.CreateAccount
{
    public partial class Notification : System.Web.UI.UserControl, IEmailTemplate
    {
        private User user;

        public User User
        {
            get { return user; }
        }

        public void SetParams(object[] parameters)
        {
            this.user = (User)parameters.Where(i => i.GetType() == typeof(User)).SingleOrDefault();
        }
    }
}