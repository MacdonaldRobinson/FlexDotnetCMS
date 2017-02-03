using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FrameworkLibrary
{
    public class BackupHelper
    {
        public static Return BackupDatabase(string connectionString)
        {
            Return returnObj = BaseMapper.GenerateReturn();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string dbName = builder.InitialCatalog;
            string backUpPath = URIHelper.BasePath + "App_Data/DBBackups/" + DateTime.Now.ToString("yyyy'-'MM'-'dd-HH'-'mm'-'ss'Z'") + "-" + dbName + ".bak";

            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                dbName = cnn.Database.ToString();

                ServerConnection sc = new ServerConnection(cnn);
                Server sv = new Server(sc);

                // Create backup device item for the backup
                BackupDeviceItem bdi = new BackupDeviceItem(backUpPath, DeviceType.File);

                // Create the backup informaton 
                Microsoft.SqlServer.Management.Smo.Backup bk = new Backup();

                //bk.PercentComplete += new PercentCompleteEventHandler(percentComplete);
                bk.Devices.Add(bdi);
                bk.Action = BackupActionType.Database;
                bk.PercentCompleteNotification = 1;
                bk.BackupSetDescription = dbName;
                bk.BackupSetName = dbName;
                bk.Database = dbName;

                //bk.ExpirationDate = DateTime.Now.AddSeconds(10);
                bk.LogTruncation = BackupTruncateLogType.Truncate;
                bk.FormatMedia = false;
                bk.Initialize = true;
                bk.Checksum = true;
                bk.ContinueAfterError = true;
                bk.Incremental = false;

                try
                {
                    // Run the backup
                    bk.SqlBackup(sv);//Exception
                    return returnObj;
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);

                    returnObj.Error = ErrorHelper.CreateError(ex);

                    return returnObj;
                }
            }
        }
    }
}