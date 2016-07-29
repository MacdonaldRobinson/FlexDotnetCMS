using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class LanguagesMapper : BaseMapper
    {
        private const string MapperKey = "LanguagesMapperKey";

        /*public static IEnumerable<Language> GetAll()
        {
            return GetAll(MapperKey, () => GetDataModel().Languages);
        }*/

        public static Language GetByID(long id)
        {
            return GetDataModel().Languages.FirstOrDefault(item => item.ID == id);
        }

        public static IEnumerable<Language> GetAllActive()
        {
            return GetDataModel().Languages.Where(item => item.IsActive);
        }

        private static int? _callAllActive = null;
        public static int CountAllActive()
        {
            if (_callAllActive != null)
                return (int)_callAllActive;

            _callAllActive = GetDataModel().Languages.Count(item => item.IsActive);

            return (int)_callAllActive;
        }

        public static Language GetByName(string language)
        {
            return GetDataModel().Languages.FirstOrDefault(item => item.Name == language);
        }

        public static Language GetByEnum(LanguageEnum languageEnum)
        {
            return GetDataModel().Languages.FirstOrDefault(item => item.Name == languageEnum.ToString());
        }

        public static Language GetDefaultLanguage()
        {
            var language = ContextHelper.GetFromRequestContext("GetDefaultLanguage");

            if (language != null)
                return (Language)language;

            language = SettingsMapper.GetSettings().DefaultLanguage;

            ContextHelper.SetToRequestContext("GetDefaultLanguage", language);

            return (Language)language;
        }

        public static Language CreateObject()
        {
            return GetDataModel().Languages.Create();
        }

        public static Language GetFromContext(Language obj)
        {
            return GetObjectFromContext(obj);
        }

        public static Return Insert(Language obj)
        {
            obj.DateCreated = DateTime.Now;
            obj.DateLastModified = DateTime.Now;

            return Insert(MapperKey, obj);
        }

        /*public static void UpdateAllVirtualPaths()
        {
            var activeLanguages = GetAllActive();
            var details = MediaDetailsMapper.GetAll();

            if (activeLanguages.Count() > 1)
            {
                foreach (var detail in details)
                {
                    IMediaDetail inContext = GetObjectFromContext((MediaDetail)detail);

                    if (!URIHelper.StartsWithLanguage(detail.VirtualPath))
                        inContext.VirtualPath = detail.VirtualPath.Replace("~/", URIHelper.ConvertAbsUrlToTilda(URIHelper.GetBaseUrlWithLanguage(FrameworkSettings.GetCurrentLanguage())));

                    if (detail.VirtualPath == "~/")
                        inContext.VirtualPath = detail.VirtualPath + GetDefaultLanguage().CultureCode;

                    if (inContext.VirtualPath != detail.VirtualPath)
                        MediaDetailsMapper.Update(inContext);
                }
            }
            else
            {
                foreach (IMediaDetail detail in details)
                {
                    string defaultLanguagePrefix = URIHelper.ConvertAbsUrlToTilda(URIHelper.GetBaseUrlWithLanguage(GetDefaultLanguage()));

                    if (detail.VirtualPath.StartsWith(defaultLanguagePrefix))
                    {
                        IMediaDetail inContext = GetObjectFromContext((MediaDetail)detail);
                        inContext.VirtualPath = detail.VirtualPath.Replace(defaultLanguagePrefix, "~/");

                        MediaDetailsMapper.Update(inContext);
                    }
                }
            }
        }*/

        public static Return Update(Language obj)
        {
            obj.DateLastModified = DateTime.Now;

            var returnObj = Update(MapperKey, obj);

            /*if (!returnObj.IsError)
                UpdateAllVirtualPaths();*/

            return returnObj;
        }

        public static Return DeletePermanently(Language obj)
        {
            return Delete(MapperKey, obj);
        }
    }
}