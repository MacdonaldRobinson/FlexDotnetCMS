using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace FrameworkLibrary
{
    public class BackupHelper
    {
        public static string DbBackupPath = $@"{URIHelper.BasePath}App_Data\DBBackups\";

        public static Return RestoreDatabase(string backUpFilePath)
        {
            Return returnObj = BaseMapper.GenerateReturn();

            if(!string.IsNullOrEmpty(backUpFilePath) && File.Exists(backUpFilePath))
            {
                var databaseName = BaseMapper.GetDataModel().Database.Connection.Database;

                var sqlCommand = $@"ALTER DATABASE {databaseName} SET Single_User WITH Rollback Immediate; USE master; RESTORE DATABASE {databaseName} FROM DISK = '{backUpFilePath}'; ALTER DATABASE {databaseName} SET Multi_User";

                try
                {
                    var result = BaseMapper.GetDataModel(true).Database.ExecuteSqlCommand(transactionalBehavior: System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, sql: sqlCommand);
                    returnObj.SetRawData(result);

                    return returnObj;
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);
                    returnObj.Error = ErrorHelper.CreateError(ex);

                    return returnObj;
                }
            }

            return returnObj;
        }

        public static Return BackupDatabase()
        {
            Return returnObj = BaseMapper.GenerateReturn();

            var databaseName = BaseMapper.GetDataModel().Database.Connection.Database;

            var sqlCommand = $@"BACKUP DATABASE {databaseName} TO DISK = '{DbBackupPath}{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt")}-{databaseName}.bak'";

            try
            {
                var result = BaseMapper.GetDataModel(true).Database.ExecuteSqlCommand(transactionalBehavior: System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, sql: sqlCommand);
                returnObj.SetRawData(result);                

                return returnObj;
            }
            catch(Exception ex)
            {
                ErrorHelper.LogException(ex);
                returnObj.Error = ErrorHelper.CreateError(ex);

                return returnObj;
            }


            /*SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
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
            }*/                
        }
    }
}