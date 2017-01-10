using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using FrameworkLibrary;
/// <summary>
/// Summary description for SessionHelper
/// </summary>

namespace FrameworkLibrary
{
    public class ContextHelper
    {
        public static double? CacheDurationInSeconds
        {
            get
            {
                return  SettingsMapper.GetSettings().OutputCacheDurationInSeconds;
            }
        }

        public static object Get(string key, ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Application:
                    return GetFromApplication(key);

                case ContextType.RequestContext:
                    return GetFromRequestContext(key);

                case ContextType.Session:
                    return GetFromSession(key);

                case ContextType.Cache:
                    return GetFromCache(key);
            }
            return null;
        }

        public static bool Set(string key, object value, ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Application:
                    return SetToApplication(key, value);
                    break;

                case ContextType.RequestContext:
                    return SetToRequestContext(key, value);
                    break;

                case ContextType.Session:
                    return SetToSession(key, value);
                    break;

                case ContextType.Cache:
                    return SetToCache(key, value);
                    break;
            }

            return false;
        }

        public static void Remove(string key, ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Application:
                    RemoveFromApplication(key);
                    break;

                case ContextType.RequestContext:
                    RemoveFromRequestContext(key);
                    break;

                case ContextType.Session:
                    RemoveFromSession(key);
                    break;

                case ContextType.Cache:
                    RemoveFromCache(key);
                    break;
            }
        }

        public static void ClearAllMemoryCache()
        {
            Clear(ContextType.Application);
            //Clear(ContextType.Session);
            //Clear(ContextType.RequestContext);
            Clear(ContextType.Cache);
        }

        public static void ClearAllCached()
        {
            var webrequests = BaseMapper.GetDataModel().WebserviceRequests;

            foreach (var item in webrequests)
            {
                BaseMapper.GetDataModel().WebserviceRequests.Remove(item);
            }

            BaseMapper.GetDataModel().SaveChanges();

            ClearAllMemoryCache();
        }

        public static void Clear(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Application:
                    HttpContext.Current.Application.Clear();
                    break;

                case ContextType.RequestContext:
                    HttpContext.Current.Items.Clear();
                    break;

                case ContextType.Session:
                    if (HttpContext.Current.Session != null)
                        HttpContext.Current.Session.Clear();
                    break;

                case ContextType.Cache:
                    var keys = new List<string>();
                    IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
                    while (enumerator.MoveNext())
                        keys.Add(enumerator.Key.ToString());

                    for (int i = 0; i < keys.Count; i++)
                        RemoveFromCache(keys[i]);
                    break;
            }
        }

        public static object GetFromCache(string key)
        {
            key = key.ToLower();
            return HttpContext.Current.Cache[key];
        }

        public static object GetFromApplication(string key)
        {
            return HttpContext.Current.Application.Get(key);
        }

        public static object GetFromSession(string key)
        {
            if (HttpContext.Current.Session == null)
                return null;

            return HttpContext.Current.Session[key];
        }

        public static object GetFromRequestContext(string key)
        {
            return HttpContext.Current.Items[key];
        }

        public static void RemoveFromCache(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public static void RemoveFromApplication(string key)
        {
            HttpContext.Current.Application.Remove(key);
        }

        public static void RemoveFromSession(string key)
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Remove(key);
        }

        public static void RemoveFromRequestContext(string key)
        {
            HttpContext.Current.Items.Remove(key);
        }

        public static bool SetToCache(string key, object value, DateTime? expiryDateTime = null)
        {
            key = key.ToLower();

            if ((expiryDateTime == null) && (CacheDurationInSeconds != null))
                expiryDateTime = DateTime.Now.AddSeconds((double)CacheDurationInSeconds);

            if (HttpContext.Current.Cache == null)
                return false;

            if (value == null)
                HttpContext.Current.Cache.Remove(key);
            else
            {
                if (expiryDateTime != null)
                    HttpContext.Current.Cache.Insert(key, value, null, (DateTime)expiryDateTime, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                else
                    HttpContext.Current.Cache.Insert(key, value);
            }

            return true;
        }

        public static bool SetToApplication(string key, object value)
        {
            if (HttpContext.Current.Application == null)
                return false;

            HttpContext.Current.Application.Set(key, value);
            return true;
        }

        public static bool SetToSession(string key, object value)
        {
            if (HttpContext.Current.Session == null)
                return false;

            HttpContext.Current.Session[key] = value;

            return true;
        }

        public static bool SetToRequestContext(string key, object value)
        {
            if (HttpContext.Current == null)
                return false;

            HttpContext.Current.Items[key] = value;
            return true;
        }

        public static string GetFromQueryString(string key)
        {
            string returnValue = HttpContext.Current.Request[key];

            if (returnValue == null)
                return "";

            return returnValue;
        }
    }
}