using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace FrameworkLibrary
{
    public class LoaderHelper
    {
        public static string RenderControl(Control ctrl)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            ctrl.RenderControl(hw);
            return sb.ToString();
        }

        public static string RenderPage(System.Web.UI.Page page, string html)
        {
            var str = new System.IO.StringWriter();
            var wrt = new HtmlTextWriter(str);

            page.ParseControl(html).RenderControl(wrt);

            html = str.ToString();

            return html;
        }

        public static string RenderControlWithPostBack(string virtualPath)
        {
            System.Web.UI.Page page = new System.Web.UI.Page();
            page.EnableViewState = false;
            page.EnableEventValidation = false;

            Control control = page.LoadControl(virtualPath);
            StringWriter tw = new StringWriter();

            HtmlForm form = new HtmlForm();
            form.ID = "__t";
            page.Controls.Add(form);
            form.Controls.Add(control);

            HttpContext.Current.Server.Execute(page, tw, true);

            return tw.ToString();
        }

        public static string ReadUrl(string url, bool enableCaching = false, long cacheDurationInSeconds = 86400)
        {
            if (url == null)
                return "";

            var absUrl = URIHelper.ConvertToAbsUrl(url).ToLower();

            var data = "";
            string absPath = URIHelper.ConvertToAbsPath(URIHelper.ConvertAbsUrlToTilda(url));

            using (FileStream fs = File.Open(absPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                data = sr.ReadToEnd();
            }

            if (enableCaching)
            {
                ContextHelper.SaveToCache(absUrl, data, DateTime.Now.AddSeconds(cacheDurationInSeconds));
            }

            return data;
        }

        public static Type GetClassFromString(string className)
        {
            IEnumerable<Type> assemblyClasses = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsClass);

            foreach (var assemblyClass in assemblyClasses)
            {
                if (assemblyClass.Name == className)
                {
                    return assemblyClass;
                }
            }

            return null;
        }

        public static Control LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            var constParamTypes = new List<System.Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            if (!File.Exists(HttpContext.Current.Server.MapPath(UserControlPath)))
                return null;

            dynamic ctl = new System.Web.UI.Page().LoadControl(UserControlPath);

            if (ctl is IEmailTemplate || ctl is IParamControl)
            {
                ctl.SetParams(constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }

        public static string RenderControl(string UserControlPath, params object[] constructorParameters)
        {
            return RenderControl(LoadControl(UserControlPath, constructorParameters));
        }
    }
}