using System;

namespace FrameworkLibrary
{
    public class ErrorHelper
    {
        public static void LogException(Exception ex)
        {
            Elmah.ErrorLog.GetDefault(null).Log(ErrorHelper.CreateError(ex));
        }

        public static Elmah.Error CreateError(string errorMessage, string innerErrorMessage = "")
        {
            return new Elmah.Error(new Exception(errorMessage, new Exception(innerErrorMessage)));
        }

        public static Elmah.Error CreateError(Exception ex)
        {
            return new Elmah.Error(ex);
        }
    }
}