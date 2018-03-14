using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Views.PageHandlers.AdminTools
{
    public partial class Default : AdvanceOptionsBasePage
    {
        private List<EmailLog> emailLogEntries;
        private List<Elmah.Error> errorLogEntries;

        protected void Page_Init(object sender, EventArgs e)
        {
            var currentConnectionSetting = AppSettings.GetConnectionSettings();
            var connectionSettings = new List<ConnectionStringSettings>();

            foreach (ConnectionStringSettings item in WebConfigurationManager.ConnectionStrings)
            {
                if (!item.Name.Contains(".") && !item.Name.Contains("Local") && currentConnectionSetting.ConnectionString != item.ConnectionString)
                {
                    connectionSettings.Add(item);
                }
            }

            DeployToEnvironment.DataSource = connectionSettings;
            DeployToEnvironment.DataTextField = "Name";
            DeployToEnvironment.DataValueField = "Name";
            DeployToEnvironment.DataBind();

            BindDbBackUpsList();
        }

        private void BindDbBackUpsList()
        {
            DbBackUps.DataSource = Directory.GetFiles(BackupHelper.DbBackupPath).Select(i => new FileInfo(i));
            DbBackUps.DataBind();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //emailLogEntries = EmailsMapper.GetAll().OrderByDescending(i => i.DateCreated).ToList();

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
            /*EmailLog.DataSource = emailLogEntries;
            EmailLog.DataBind();*/

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
                Return returnObj = BackupHelper.BackupDatabase();

                if (returnObj.IsError)
                    DisplayErrorMessage("Error backing up DB", returnObj.Error);
                else
                {
                    DisplaySuccessMessage("Successfully backed up DB");
                    BindDbBackUpsList();
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
                DisplayErrorMessage("Error backing up DB", ErrorHelper.CreateError(ex));
            }
        }

        protected void ClearAllCache_OnClick(object sender, EventArgs e)
        {
            var returnObj = CacheHelper.ClearAllCache();

            if (returnObj.IsError)
                DisplayErrorMessage("Error clearing cache", returnObj.Error);
            else
                DisplaySuccessMessage("Successfully cleared all cache");

        }

        protected void DeleteAllHistory_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<IMediaDetail> historyMediaDetails = new List<IMediaDetail>();

                if (DeleteSavedDrafts.Checked)
                    historyMediaDetails = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber > 0);
                else
                    historyMediaDetails = MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.HistoryVersionNumber > 0 && !i.IsDraft);

                historyMediaDetails = historyMediaDetails.ToList();

                foreach (var item in historyMediaDetails)
                {
                    var returnObj = MediaDetailsMapper.DeletePermanently((MediaDetail)item);
                }

                //ClearAllCache_OnClick(sender, e);
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

        private Dictionary<string, List<Return>> _deployMessages = new Dictionary<string, List<Return>>();
        public void AddMessage(string key, string message)
        {
            if(!_deployMessages.Keys.Contains(key))
            {
                var obj = new List<Return>();
                obj.Add(BaseMapper.GenerateReturn(message));

                _deployMessages.Add(key, obj);
            }
            else
            {
                if(!_deployMessages[key].Any(i=>i.Error.Exception.Message == message))
                {
                    _deployMessages[key].Add(BaseMapper.GenerateReturn(message));
                }
            }
        }

        private void CompareLocalAndRemoteMediaDetail(MediaDetail localMediaDetail, MediaDetail remoteMediaDetail)
        {
            if (localMediaDetail.CachedVirtualPath != remoteMediaDetail.CachedVirtualPath)
            {
                if(localMediaDetail.ShowInMenu)
                {
                    AddMessage("Url's are different - Show In Menu", $"Local: '{localMediaDetail.CachedVirtualPath}' | Remote: '{remoteMediaDetail.CachedVirtualPath}'");
                }
                else
                {
                    AddMessage("Url's are different", $"Local: '{localMediaDetail.CachedVirtualPath}' | Remote: '{remoteMediaDetail.CachedVirtualPath}'");
                }
            }

            if (localMediaDetail.UseMediaTypeLayouts != remoteMediaDetail.UseMediaTypeLayouts)
            {
                AddMessage("UseMediaTypeLayouts values are different", $"Local: '{localMediaDetail.CachedVirtualPath}' is <strong>'{localMediaDetail.UseMediaTypeLayouts}'</strong> | Remote: '{remoteMediaDetail.CachedVirtualPath}' is <strong>'{remoteMediaDetail.UseMediaTypeLayouts}'</strong>");
            }

            if (localMediaDetail.MediaType.Name != remoteMediaDetail.MediaType.Name)
            {
                AddMessage("Media Types are different", $"Local: '{localMediaDetail.CachedVirtualPath}' is <strong>'{localMediaDetail.MediaType.Name}'</strong> | Remote: '{remoteMediaDetail.CachedVirtualPath}' is <strong>'{remoteMediaDetail.MediaType.Name}'</strong>");
            }

            foreach (var localField in localMediaDetail.Fields)
            {
                var remoteField = remoteMediaDetail.Fields.FirstOrDefault(i => i.FieldCode == localField.FieldCode);

                if (remoteField == null)
                {
                    AddMessage(localMediaDetail.CachedVirtualPath, $"<strong>Missing Field:</strong> {localField.FieldCode}");
                }
                else
                {
                    if (localField.FrontEndLayout != remoteField.FrontEndLayout)
                    {
                        AddMessage(localMediaDetail.CachedVirtualPath, $"<strong>Field</strong> {localField.FieldCode} has a <strong>different FrontEndLayout</strong>");
                    }

                    if (localField.FieldValue != remoteField.FieldValue)
                    {
                        AddMessage(localMediaDetail.CachedVirtualPath, $"<strong>Field</strong> {localField.FieldCode} has a <strong>different Field Value</strong>");
                    }

                    if (localField.FieldSettings != remoteField.FieldSettings)
                    {
                        AddMessage(localMediaDetail.CachedVirtualPath, $"<strong>Field</strong> {localField.FieldCode} has a <strong>different Field FieldSettings</strong>");
                    }
                }
            }

            if (!localMediaDetail.UseMediaTypeLayouts)
            {
                if (localMediaDetail.MainLayout != remoteMediaDetail.MainLayout)
                {
                    AddMessage(localMediaDetail.CachedVirtualPath, $"MainLayout is different");
                }

                if (localMediaDetail.SummaryLayout != remoteMediaDetail.SummaryLayout)
                {
                    AddMessage(localMediaDetail.CachedVirtualPath, $"SummaryLayout is different");
                }

                if (localMediaDetail.FeaturedLayout != remoteMediaDetail.FeaturedLayout)
                {
                    AddMessage(localMediaDetail.CachedVirtualPath, $"FeaturedLayout is different");
                }

                if (localMediaDetail.Media.ParentMediaID != remoteMediaDetail.Media.ParentMediaID)
                {
                    var remoteParent = remoteMediaDetail.Media?.ParentMedia?.GetLiveMediaDetail();

                    if(remoteParent != null)
                    {                        
                        AddMessage("Moved Pages", $"Parent of the local page is {localMediaDetail.Media.ParentMedia?.GetLiveMediaDetail().CachedVirtualPath} | remote parent is '{remoteParent.CachedVirtualPath}'");
                    }
                }
            }
            else
            {
                if (localMediaDetail.MediaType.MainLayout != remoteMediaDetail.MediaType.MainLayout)
                {
                    AddMessage("MediaType Layouts",$"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> MainLayout is different");
                }

                if (localMediaDetail.MediaType.SummaryLayout != remoteMediaDetail.MediaType.SummaryLayout)
                {
                    AddMessage("MediaType Layouts", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> SummaryLayout is different");                    
                }

                if (localMediaDetail.MediaType.FeaturedLayout != remoteMediaDetail.MediaType.FeaturedLayout)
                {
                    AddMessage("MediaType Layouts", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> FeaturedLayout is different");
                }

                foreach (var localField in localMediaDetail.MediaType.Fields)
                {
                    var remoteField = remoteMediaDetail.MediaType.Fields.FirstOrDefault(i => i.FieldCode == localField.FieldCode);

                    if (remoteField == null)
                    {
                        AddMessage("MediaType Fields", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> is <strong>Missing</strong> Media Type Field <strong>'{localField.FieldCode}'</strong>");                        
                    }
                    else
                    {
                        if (localField.FrontEndLayout != remoteField.FrontEndLayout)
                        {
                            AddMessage("MediaType Fields", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> Media Type Field <strong>'{localField.FieldCode}'</strong> has a <strong>different FrontEndLayout</strong>");
                        }

                        if (localField.FieldValue != remoteField.FieldValue)
                        {
                            AddMessage("MediaType Fields", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> Media Type Field <strong>{localField.FieldCode}</strong> has a <strong>different Field Value</strong>");
                        }

                        if (localField.FieldSettings != remoteField.FieldSettings)
                        {
                            AddMessage("MediaType Fields", $"MediaType <strong>'{localMediaDetail.MediaType.Name}'</strong> Media Type Field <strong>{localField.FieldCode}</strong> has a <strong>different Field FieldSettings</strong>");
                        }
                    }
                }
            }
        }

        protected void Deploy_Click(object sender, EventArgs e)
        {            
            var remoteConnectionStringSetting = FrameworkBaseMedia.PrepareConnectionSettings(WebConfigurationManager.ConnectionStrings[DeployToEnvironment.SelectedValue]);
            //var localConnectionStringSetting = FrameworkBaseMedia.PrepareConnectionSettings(AppSettings.GetConnectionSettings());

            var localDataModel = BaseMapper.GetDataModel();
            var remoteDataModel = BaseMapper.GetDataModel(true, true, remoteConnectionStringSetting);

            var localMediaDetails = localDataModel.MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && i.MediaType.Name != MediaTypeEnum.RootPage.ToString()).ToList();
            var remoteMediaDetails = remoteDataModel.MediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && i.MediaType.Name != MediaTypeEnum.RootPage.ToString()).ToList();

            var localSettings = localDataModel.AllSettings.FirstOrDefault();
            var remoteSettings = remoteDataModel.AllSettings.FirstOrDefault();

            if (localSettings.GlobalCodeInBody != remoteSettings.GlobalCodeInBody)
            {
                AddMessage("Settings", $"The setting: 'GlobalCodeInBody' is different");
            }

            if (localSettings.GlobalCodeInHead != remoteSettings.GlobalCodeInHead)
            {
                AddMessage("Settings", $"The setting: 'GlobalCodeInHead' is different");
            }

            foreach (var localMediaDetail in localMediaDetails)
            {
                var remoteMediaDetail = remoteMediaDetails.FirstOrDefault(i => i.MediaID == localMediaDetail.MediaID && i.LanguageID == localMediaDetail.LanguageID);

                if (remoteMediaDetail != null)
                {
                    CompareLocalAndRemoteMediaDetail(localMediaDetail, remoteMediaDetail);
                }
                else
                {
                    var found = remoteMediaDetails.FirstOrDefault(i => i.CachedVirtualPath == localMediaDetail.CachedVirtualPath && i.LanguageID == localMediaDetail.LanguageID);

                    if(found != null)
                    {
                        //AddMessage("Different MediaID's but the same cached virtual path", $"Local: {localMediaDetail.CachedVirtualPath} is '{localMediaDetail.MediaID}' | Remote: {found.CachedVirtualPath}  is '{found.MediaID}'");
                        CompareLocalAndRemoteMediaDetail(localMediaDetail, found);                        
                    }
                    else
                    {
                        AddMessage("Does not exist on remote", $"{localMediaDetail.CachedVirtualPath}");
                    }
                }
            }

            DeployMessages.DataSource = _deployMessages;
            DeployMessages.DataBind();
        }

        protected void DbBackUps_DataBound(object sender, EventArgs e)
        {
            DbBackUps.UseAccessibleHeader = true;
            if (DbBackUps.HeaderRow != null)
            {
                DbBackUps.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void DeleteDbBackUp_Click(object sender, EventArgs e)
        {
            var deleteButton = sender as LinkButton;

            if(!string.IsNullOrEmpty(deleteButton.CommandArgument) && File.Exists(deleteButton.CommandArgument))
            {
                var returnObj = BaseMapper.GenerateReturn();
                try
                {
                    File.Delete(deleteButton.CommandArgument);
                    BindDbBackUpsList();
                    DisplaySuccessMessage($"Successfully deleted file: ( {deleteButton.CommandArgument} )");
                }
                catch(Exception ex)
                {
                    ErrorHelper.LogException(ex);
                    returnObj.Error = ErrorHelper.CreateError(ex);

                    DisplayErrorMessage(returnObj.Error.Message, returnObj.Error);
                }
            }
        }

        protected void RestoreBackUp_Click(object sender, EventArgs e)
        {
            if (!canAccessSection)
            {
                DisplayAccessError();
                return;
            }

            var restoreButton = sender as LinkButton;

            if (!string.IsNullOrEmpty(restoreButton.CommandArgument) && File.Exists(restoreButton.CommandArgument))
            {
                var returnObj = BaseMapper.GenerateReturn();
                try
                {
                    BackupNow_OnClick(sender, e);

                    returnObj = BackupHelper.RestoreDatabase(restoreButton.CommandArgument);

                    if (returnObj.IsError)
                    {
                        DisplayErrorMessage("Error restoring", returnObj.Error);
                    }
                    else
                    {
                        DisplaySuccessMessage($"Successfully restored file: ( {restoreButton.CommandArgument} )");
                    }
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);
                    returnObj.Error = ErrorHelper.CreateError(ex);

                    DisplayErrorMessage(returnObj.Error.Message, returnObj.Error);
                }
            }
        }
    }
}