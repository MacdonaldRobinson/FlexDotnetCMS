using System;

namespace FrameworkLibrary
{
    [Serializable]
    public class ValidationError
    {
        private string message;

        public ValidationError(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get
            {
                return message;
            }
        }
    }
}