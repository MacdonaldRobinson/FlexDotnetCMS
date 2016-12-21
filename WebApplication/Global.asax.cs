using FrameworkLibrary;
using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using WebApplication.Services;

namespace WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        private static bool isFirstApplicationRequest = true;

        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RegisterRoutes(RouteTable.Routes);
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var installerPath = "~/Installer/";
            var absInstallerUrl = URIHelper.ConvertToAbsUrl(installerPath);
            var absInstallerPath = URIHelper.ConvertToAbsPath(installerPath);

            if (Request.CurrentExecutionFilePathExtension == "" && !Request.Url.AbsoluteUri.StartsWith(absInstallerUrl))
            {
                if (Directory.Exists(absInstallerPath) && AppSettings.EnableInstaller)
                    Response.Redirect(installerPath);
            }

            if (!AppSettings.IsRunningOnDev && !Request.Url.Host.StartsWith("www."))
                Response.RedirectPermanent(Request.Url.AbsoluteUri.Replace("://", "://www."));

            BaseService.AddResponseHeaders();

            var virtualPathRequest = HttpContext.Current.Request.Path.EndsWith("/");

            if (virtualPathRequest)
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetNoStore();

            }

            if (isFirstApplicationRequest)
            {
                ContextHelper.ClearAllMemoryCache();
                FrameworkBaseMedia.InitConnectionSettings(AppSettings.GetConnectionSettings());
                isFirstApplicationRequest = false;
            }
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            ErrorHelper.LogException(Server.GetLastError());
        }

        private void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            string sessionId = Session.SessionID;
        }

        private void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends.
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer
            // or SQLServer, the event is not raised.
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");
            routes.Ignore("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.Ignore("Telerik.Web.UI.DialogHandler.aspx");

            routes.Ignore("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.Ignore("{*allsvc}", new { allsvc = @".*\.svc(/.*)?" });
            routes.Ignore("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });

            routes.Ignore("{*staticfile}", new { staticfile = @".*\.(jpg|gif|jpeg|png|js|css|htm|html|htc)$" });

            routes.Add("CatchAll", new Route("{*virtualPath}", new Handlers.CustomRouteHandler()));
        }
    }
}