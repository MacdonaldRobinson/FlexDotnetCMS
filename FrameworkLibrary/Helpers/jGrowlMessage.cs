namespace FrameworkLibrary
{
    public class jGrowlMessage
    {
        private string title;
        private string message;
        private Elmah.Error error = null;
        private jGrowlMessageType type;
        private long messageDisplayTime;

        public enum jGrowlMessageType
        {
            Error,
            Success,
            Feedback
        }

        public jGrowlMessage(string title, string message, jGrowlMessageType type, Elmah.Error error = null, long messageDisplayTime = 20000)
        {
            this.title = title;
            this.message = message;
            this.type = type;
            this.messageDisplayTime = messageDisplayTime;
            this.error = error;
        }

        public string Title
        {
            get
            {
                return this.title;
            }
        }

        public string GetMessage(bool showTechnicalDetails = false)
        {
            string msg = this.message;

            if ((showTechnicalDetails) && (error != null))
            {
                if (!string.IsNullOrEmpty(error.Exception.Message))
                {
                    msg += "<br /><br /><strong>Error Details -</strong><br /><br />" + error.Exception.Message;

                    var exception = error.Exception;
                    var innerExceptionMessages = "";

                    while (exception !=null && exception?.InnerException?.Message !="")
                    {
                        exception = exception?.InnerException;

                        if(exception != null)
                            innerExceptionMessages = exception.Message + "<br /><br />";
                    }

                    msg += "<br /><br />Inner Exception:<br />" + innerExceptionMessages;
                }
            }

            return msg;
        }

        public jGrowlMessageType Type
        {
            get
            {
                return this.type;
            }
        }

        public long MessageDisplayTime
        {
            get
            {
                return this.messageDisplayTime;
            }
        }

        public Elmah.Error Error
        {
            get
            {
                return this.error;
            }
        }
    }
}