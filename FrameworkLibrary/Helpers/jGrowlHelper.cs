using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class jGrowlHelper
    {
        public static string GenerateCode(IEnumerable<jGrowlMessage> messages, bool showTechnicalDetails = false)
        {
            string html = "";

            foreach (jGrowlMessage message in messages)
                html += GenerateCode(message, showTechnicalDetails);

            return html;
        }

        public static string GenerateCode(jGrowlMessage message, bool showTechnicalDetails = false)
        {
            string html = "";

            string msg = "\"" + message.GetMessage(showTechnicalDetails).Replace("\r\n", "").Replace("\n", " ").Replace("\"", "'") + "\"";
            string title = "\"" + message.Title + "\"";
            string errorDisplayTime = message.MessageDisplayTime.ToString();
            string theme = "\"" + message.Type.ToString() + "\"";

            html += @"jQuery.jGrowl(" + msg + @", {
                            header: " + title + @",
                            theme: " + theme + @",
                            life: " + errorDisplayTime + @",
                            position: 'top-right'
                        });";

            return html;
        }
    }
}