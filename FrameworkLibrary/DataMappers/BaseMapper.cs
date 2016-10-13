using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FrameworkLibrary
{
    public interface IMustContainID
    {
        long ID { get; set; }
    }

    public class BaseMapper
    {
        private const string dataModelKey = "FrameworkDataModelKey";

        protected static ContextType mapperStorageContext = ContextType.RequestContext;
        protected static ContextType dataContextStorageContext = ContextType.RequestContext;

        protected static string DataModelKey
        {
            get
            {
                return dataModelKey;
            }
        }

        public static ContextType MapperStorageContext
        {
            get
            {
                return mapperStorageContext;
            }
        }

        public static Return GenerateReturn(string message = "", string innerExceptionMessage = "")
        {
            if (message == "")
                return new Return();

            var ex = new Exception(message, new Exception(innerExceptionMessage));

            var returnObj = new Return { Error = ErrorHelper.CreateError(ex) };

            return returnObj;
        }

        /// <summary>
        /// Attempts to update the DB with all the changes made to the current DataContext
        /// </summary>
        /// <param name="logError"></param>
        /// <returns>An instance of the Return class</returns>
        public static Return SaveDataModel(bool logError = true)
        {
            var returnObj = new Return();
            try
            {
                var returnVal = GetDataModel().SaveChanges();
                returnObj.SetRawData(returnVal);

                if (returnVal == 0)
                    throw new Exception("No changes made", new Exception("The transaction was successfull but no changes were made"));
            }
            catch (Exception ex)
            {
                returnObj.Error = ErrorHelper.CreateError(ex);

                if (logError)
                    ErrorHelper.LogException(ex);
            }

            return returnObj;
        }

        /// <summary>
        /// Attempts to get the EntityObject from the current DataContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="notInCurrentContext"></param>
        /// <param name="forceNew"></param>
        /// <returns>Generic T ( EntityObject ) object</returns>
        public static T GetObjectFromContext<T>(T notInCurrentContext, bool forceNew = false) where T : class, IMustContainID
        {
            if (notInCurrentContext == null)
                return default(T);

            var foundInContext = GetDataModel(forceNew).Set<T>().Find(notInCurrentContext.ID);

            if (foundInContext != null)
                return (T)foundInContext;

            return default(T);
        }

        public static IEnumerable<T> GetAll<T>(string mapperKey, Func<IEnumerable<T>> function)
        {
            return function();
        }

        public static Return AddObjectToContext<T>(T obj) where T : class, IMustContainID
        {
            var returnObj = new Return();
            try
            {
                if (GetObjectFromContext(obj) == null)
                    GetDataModel().Set<T>().Add(obj);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
                returnObj.Error = ErrorHelper.CreateError(ex);
            }

            return returnObj;
        }

        /// <summary>
        /// Attempts to delete an object from the current DataContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>An instance of the Return class</returns>
        public static Return DeleteObjectFromContext<T>(T obj) where T : class, IMustContainID
        {
            var returnObj = new Return();
            try
            {
                obj = GetObjectFromContext(obj);

                if (obj != null)
                    GetDataModel().Set<T>().Remove(obj);
            }
            catch (Exception ex)
            {
                returnObj.Error = ErrorHelper.CreateError(ex);

                ErrorHelper.LogException(ex);
            }

            return returnObj;
        }

        /// <summary>
        /// Attempts to get a List of EntityObjects from the current DataContext from a List of EntityObjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="notInCurrentContexts"></param>
        /// <returns>A List of Generic T ( EntityObject ) objects</returns>
        protected static IEnumerable<T> GetObjects<T>(IEnumerable<T> notInCurrentContexts) where T : class, IMustContainID
        {
            return notInCurrentContexts.Select(obj => GetObjectFromContext(obj));
        }

        public static bool? CanConnectToDB { get; private set; }

        public static bool CanConnectToDBUsingEntities(Entities context)
        {
            if (CanConnectToDB != null)
                return (bool)CanConnectToDB;

            try
            {
                context.Database.Connection.Open();
                CanConnectToDB = true;
                return true;
            }
            catch (Exception ex)
            {
                CanConnectToDB = false;
                ErrorHelper.LogException(ex);
                return false;
            }
        }

        public static Entities GetDataModel(bool forceNew = false, bool generateProxies = true)
        {
            Entities context;

            if ((ContextHelper.Get(DataModelKey, dataContextStorageContext) == null) || (forceNew))
            {
                context = new Entities(FrameworkBaseMedia.ConnectionSettings.ConnectionString);

                if (!CanConnectToDBUsingEntities(context))
                    return null;

                if (generateProxies)
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = true;
                }
                else
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    context.Configuration.ProxyCreationEnabled = false;
                }

                ContextHelper.Set(DataModelKey, context, dataContextStorageContext);

                return context;
            }
            context = (Entities)ContextHelper.Get(DataModelKey, dataContextStorageContext);
            if (context.Database.Connection.State == ConnectionState.Broken)
            {
                context.Database.Connection.Close();
                context.Database.Connection.Open();
            }

            return context;
        }

        private static bool CurrentUserHasPermissions(PermissionsEnum permissionsEnum)
        {
            return FrameworkSettings.CurrentUser == null || FrameworkSettings.CurrentUser.HasPermission(permissionsEnum);
        }

        public static Return Insert<T>(string mapperKey, T obj, bool logError = true) where T : class, IMustContainID
        {
            if (!CurrentUserHasPermissions(PermissionsEnum.SaveItems))
                return GetPermissionDeniedError();

            var returnObj = AddObjectToContext(obj);

            if (!returnObj.IsError)
            {
                returnObj = SaveDataModel(logError);
            }

            return returnObj;
        }

        /// <summary>
        /// Attempts to update the object context by calling the SaveDataModel method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapperKey"></param>
        /// <param name="obj"></param>
        /// <param name="logError"></param>
        /// <returns>An instance of the Return class</returns>
        public static Return Update<T>(string mapperKey, T obj, bool logError = true) where T : class, IMustContainID
        {
            if (!CurrentUserHasPermissions(PermissionsEnum.SaveItems))
                return GetPermissionDeniedError();

            var returnObj = SaveDataModel(logError);

            return returnObj;
        }

        private static Return GetPermissionDeniedError()
        {
            Return returnObj = GenerateReturn("You do not have the appropriate permissions to perform this operation");

            return returnObj;
        }

        /// <summary>
        /// Attempts to delete the object from the context and then calls the SaveDataModel method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapperKey"></param>
        /// <param name="obj"></param>
        /// <param name="logError"></param>
        /// <returns>An instance of the Return class</returns>
        public static Return Delete<T>(string mapperKey, T obj, bool logError = true) where T : class, IMustContainID
        {
            if (!CurrentUserHasPermissions(PermissionsEnum.DeleteItemsPermanently))
                return GetPermissionDeniedError();

            var returnObj = DeleteObjectFromContext(obj);

            if (!returnObj.IsError)
            {
                returnObj = SaveDataModel(logError);
            }

            return returnObj;
        }
    }
}