using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace WebApplication.Admin.Views.PageHandlers.AdminTools
{
    public partial class Default : AdvanceOptionsBasePage
    {
        private List<EmailLog> emailLogEntries;
        private List<Elmah.Error> errorLogEntries;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            emailLogEntries = EmailsMapper.GetAll().OrderByDescending(i => i.DateCreated).ToList();

            var errors = new List<Elmah.ErrorLogEntry>();
            var newErrors = new List<Elmah.ErrorLogEntry>();

            Elmah.AccessErrorLog.GetDefault(null).GetErrors(0, 10, errors);

            foreach (var error in errors)
            {
                newErrors.Add(Elmah.AccessErrorLog.GetDefault(null).GetError(error.Id));
            }

            errorLogEntries = newErrors.Select(i => i.Error).ToList();

            Bind();
        }

        private void Bind()
        {
            EmailLog.DataSource = emailLogEntries;
            EmailLog.DataBind();

            ErrorLog.DataSource = errorLogEntries;
            ErrorLog.DataBind();

            DBBackupPath.Text = AppSettings.DBBackupPath;
        }

        protected void BackupNow_OnClick(object sender, EventArgs e)
        {
            //DisplayErrorMessage("This functionality is not implemented");
            if (!canAccessSection)
            {
                DisplayAccessError();
                return;
            }

            try
            {
                Return returnObj = BackupHelper.BackupDatabase(AppSettings.GetConnectionSettings().ConnectionString);

                if (returnObj.IsError)
                    DisplayErrorMessage("Error backing up DB", returnObj.Error);
                else
                    DisplaySuccessMessage("Successfully backed up DB");
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
                DisplayErrorMessage("Error backing up DB", ErrorHelper.CreateError(ex));
            }
        }

        protected void ClearAllCache_OnClick(object sender, EventArgs e)
        {
            var webserviceRequests = FrameworkLibrary.WebserviceRequestsMapper.GetAll();

            foreach (var item in webserviceRequests)
            {
                var context = BaseMapper.GetObjectFromContext(item);

                if (context != null)
                    BaseMapper.DeleteObjectFromContext(context);
            }

            try
            {
                BaseMapper.SaveDataModel();
                DisplaySuccessMessage("Successfully Cleared All Cache");
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Error Clearing All Cache", ErrorHelper.CreateError(ex));
            }

            ContextHelper.ClearAllMemoryCache();
            FileCacheHelper.ClearAllCache();
        }

        protected void DeleteAllHistoryAndClearAllCache_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<IMediaDetail> historyMediaDetails = new List<IMediaDetail>();

                if (DeleteSavedDrafts.Checked)
                    historyMediaDetails = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber > 0);
                else
                    historyMediaDetails = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber > 0 && !i.IsDraft);

                foreach (var item in historyMediaDetails)
                {
                    var returnObj = MediaDetailsMapper.DeletePermanently((MediaDetail)item);
                }                

                ClearAllCache_OnClick(sender, e);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);

                DisplayErrorMessage("Error Clearing All Cache", ErrorHelper.CreateError(ex));
            }
        }

        protected void EmailLog_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            EmailLog.PageIndex = e.NewPageIndex;
            EmailLog.DataBind();
        }

        protected void EmailLog_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            EmailLog.DataSource = emailLogEntries.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }

        protected void ErrorLog_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ErrorLog.PageIndex = e.NewPageIndex;
            ErrorLog.DataBind();
        }

        protected void ErrorLog_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            var sortDirection = ((e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending) ? "ASC" : "DESC");
            ErrorLog.DataSource = errorLogEntries.OrderBy(e.SortExpression + " " + sortDirection).ToList();
            Bind();
        }
    }
}