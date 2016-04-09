using System;
using System.Security.Principal;

namespace FrameworkLibrary
{
    [Serializable]
    public class CustomIdentity : IIdentity
    {
        private string name;
        private bool isAuthenticated;
        private string authenticationType;

        public CustomIdentity(string name, bool isAuthenticated, string authenticationType)
        {
            this.name = name;
            this.isAuthenticated = isAuthenticated;
            this.authenticationType = authenticationType.ToString();
        }

        public string AuthenticationType
        {
            get { return authenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}