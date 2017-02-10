using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Installer
{
    public partial class Default : System.Web.UI.Page
    {
        private PageStatePersister pageStatePersister;

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                // Unlike as exemplified in the MSDN docs, we cannot simply return a new PageStatePersister
                // every call to this property, as it causes problems
                return pageStatePersister ?? (pageStatePersister = new Handlers.CustomPageStatePersister(this));
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (AppSettings.EnableInstaller)
            {
                InstallerPanel.Visible = true;
                InstallerDisabledPanel.Visible = false;

                InitInstaller();
            }
            else
            {
                InstallerPanel.Visible = false;
                InstallerDisabledPanel.Visible = true;
            }
        }

        private void InitInstaller()
        {
            var files = Directory.GetFiles(GetAbsolutePathToInstallerSqlFiles());

            var listItems = new List<FileInfo>();

            foreach (var file in files)
            {
                listItems.Add(new FileInfo(file));
            }

            SqlFiles.DataSource = listItems;
            SqlFiles.DataTextField = "Name";
            SqlFiles.DataValueField = "Name";
            SqlFiles.DataBind();

            IsRunningOnDev.SelectedValue = AppSettings.IsRunningOnDev.ToString();
            ReadConnectionStringSettings(AppSettings.IsRunningOnDev);
        }

        private void ReadConnectionStringSettings(bool isRunningOnDev)
        {
            try
            {
                /*System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                var connectionStringSettings = new SqlConnectionStringBuilder(config.ConnectionStrings.ConnectionStrings[isRunningOnDev.ToString()].ConnectionString);*/

                var connectionStringSettings = new SqlConnectionStringBuilder(GetConnectionString());

                DataSource.Text = connectionStringSettings.DataSource;
                DatabaseName.Text = connectionStringSettings.InitialCatalog;
                UserID.Text = connectionStringSettings.UserID;
                Password.Text = connectionStringSettings.Password;
                IntegratedSecurity.Checked = connectionStringSettings.IntegratedSecurity;
                AttachDBFilename.Text = connectionStringSettings.AttachDBFilename;
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }
        }

        public async Task<bool> AttemptConnection()
        {
            var conn = GetConnection();

            Messages.Text += "Attempting to connect using the connection string: " + conn.ConnectionString + "<br /><br />";

            try
            {
                conn.Open();
                Messages.Text += "Successfully connected to Database: " + conn.Database + "<br /><br />";
                ExecutePanel.Visible = true;
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br /><br />";
                ExecutePanel.Visible = false;

                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return true;
        }

        private string GetAbsolutePathToInstallerSqlFiles()
        {
            return GetAbsolutePathToInstallerDirectory().FullName + "SqlFiles/";
        }

        private DirectoryInfo GetAbsolutePathToInstallerDirectory()
        {
            return new DirectoryInfo(URIHelper.ConvertToAbsPath("~/Installer/"));
        }

        private DirectoryInfo GetAbsolutePathToMoveDirectory()
        {
            return new DirectoryInfo(URIHelper.ConvertToAbsPath("~/App_Data/Installer/"));
        }

        private FileInfo GetWebConfig()
        {
            return new FileInfo(Server.MapPath("~/Web.config"));
        }

        private string GetConnectionString()
        {
            var connectionString = AppSettings.GetConnectionSettingsByIsRunningOnDev(bool.Parse(IsRunningOnDev.SelectedValue)).ConnectionString;
            return connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        private void ExecuteSQL(string sql)
        {
            var conn = GetConnection();

            //split the script on "GO" commands
            string[] splitter = new string[] { "GO\r\n" };
            string[] commandTexts = sql.Split(splitter,
              StringSplitOptions.RemoveEmptyEntries);
            try
            {
                conn.Open();

                foreach (string commandText in commandTexts)
                {
                    if (commandText.ToLower().StartsWith("use"))
                        continue;

                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    SqlTransaction sqlTransaction = conn.BeginTransaction();

                    cmd.Connection = conn;
                    cmd.Transaction = sqlTransaction;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
                        sqlTransaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }

            Messages.Text += "<br />Execution finished!<br /><br />";

            conn.Close();
            conn.Dispose();

            ExecutePanel.Visible = true;
        }

        protected void ExecuteSQL_Click(object sender, EventArgs e)
        {
            var fileContent = File.ReadAllText(GetAbsolutePathToInstallerSqlFiles() + SqlFiles.SelectedValue);
            ExecuteSQL(fileContent);
        }

        protected void MoveInstallerDirectory_Click(object sender, EventArgs e)
        {
            if (GetAbsolutePathToInstallerDirectory().Exists)
            {
                try
                {
                    Directory.Move(GetAbsolutePathToInstallerDirectory().FullName, GetAbsolutePathToMoveDirectory().FullName);
                    Response.Redirect("~/admin/");
                }
                catch (Exception ex)
                {
                    Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
                }
            }
        }

        protected void CreateDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "CREATE DATABASE " + new SqlConnectionStringBuilder(connection.ConnectionString).InitialCatalog;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }
        }

        protected async void Attempt_Click(object sender, EventArgs e)
        {
            var attempt = await AttemptConnection();
        }

        protected void SaveLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SetWebconfigReadWrite();

                var connectionString = GetConnectionString();

                var webConfig = GetWebConfig();
                var webConfigContent = File.ReadAllText(webConfig.FullName);

                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.DataSource = DataSource.Text;

                if (!string.IsNullOrEmpty(DatabaseName.Text))
                    connectionStringBuilder.InitialCatalog = DatabaseName.Text;

                if (!string.IsNullOrEmpty(UserID.Text))
                    connectionStringBuilder.UserID = UserID.Text;

                if (!string.IsNullOrEmpty(Password.Text))
                    connectionStringBuilder.Password = Password.Text;

                if (!string.IsNullOrEmpty(AttachDBFilename.Text))
                    connectionStringBuilder.AttachDBFilename = AttachDBFilename.Text;

                connectionStringBuilder.PersistSecurityInfo = true;
                connectionStringBuilder.IntegratedSecurity = IntegratedSecurity.Checked;

                var newConnectionString = connectionStringBuilder.ConnectionString;

                webConfigContent = webConfigContent.Replace(connectionString, newConnectionString);

                File.WriteAllText(webConfig.FullName, webConfigContent);

                SetWebconfigReadonly();

                Messages.Text += "Successfully saved connection string settings<br />";
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }
        }

        protected void DisableInstaller_Click(object sender, EventArgs e)
        {
            try
            {
                SetWebconfigReadWrite();

                var webConfig = GetWebConfig();
                var webConfigContent = File.ReadAllText(webConfig.FullName);

                var installerKey = "key=\"EnableInstaller\" value=\"True\"";
                var disableInstallerKey = "key=\"EnableInstaller\" value=\"False\"";

                webConfigContent = webConfigContent.Replace(installerKey, disableInstallerKey);

                File.WriteAllText(webConfig.FullName, webConfigContent);

                SetWebconfigReadonly();

                Response.Redirect("~/", false);
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }
        }

        private void SetWebconfigReadonly()
        {
            File.SetAttributes(GetWebConfig().FullName, File.GetAttributes(GetWebConfig().FullName) | FileAttributes.ReadOnly);
        }

        private void SetWebconfigReadWrite()
        {
            File.SetAttributes(GetWebConfig().FullName, File.GetAttributes(GetWebConfig().FullName) & ~FileAttributes.ReadOnly);
        }

        protected void IsRunningOnDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetWebconfigReadWrite();

                var isRunningOnDev = bool.Parse(IsRunningOnDev.SelectedValue);

                var webConfig = GetWebConfig();
                var webConfigContent = File.ReadAllText(webConfig.FullName);

                var findKey = "<add key=\"IsRunningOnDev\" value=\"" + AppSettings.IsRunningOnDev.ToString() + "\" />";
                var replaceWithKey = "<add key=\"IsRunningOnDev\" value=\"" + isRunningOnDev.ToString() + "\" />";

                webConfigContent = webConfigContent.Replace(findKey, replaceWithKey);

                File.WriteAllText(webConfig.FullName, webConfigContent);

                ReadConnectionStringSettings(isRunningOnDev);

                SetWebconfigReadonly();
            }
            catch (Exception ex)
            {
                Messages.Text += ex.Message + "" + ex.InnerException + "<br />";
            }
        }

        protected void UpdateCMSAdminLogin_Click(object sender, EventArgs e)
        {
            var returnObj = new Return();

            if (CMSEmailAddress.Text == "" || CMSPassword.Text == "")
            {
                Messages.Text = "Please make sure to enter a valid 'Email Address' and 'Password'";
            }
            else
            {
                var admin = UsersMapper.GetByUserName("admin");
                if (admin == null)
                {
                    returnObj.Error = new Elmah.Error() { Message = "Cannot find user with username: 'admin'" };
                }
                else
                {
                    admin.EmailAddress = CMSEmailAddress.Text;
                    admin.Password = StringHelper.Encrypt(Password.Text.Trim());

                    returnObj = UsersMapper.Update(admin);
                }

                if (returnObj.IsError)
                {
                    Messages.Text = returnObj.Error.Message + "" + returnObj.Error.Exception.InnerException + "<br />";
                }
                else
                {
                    DisableInstallerPanel.Visible = true;
                    Messages.Text = "Successfully updated CMS Admin Login Credentials";
                }
            }

            ExecutePanel.Visible = true;

        }
    }
}