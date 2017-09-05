using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FrameworkLibrary;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication.Services;
using WebApplication.Admin.Controls.Fields;

namespace WebApplication.WebServices
{
    /// <summary>
    /// Summary description for Handlers
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Handlers : BaseService
    {
        [WebMethod]
        public void FieldSettingsSubmissionHandler()
        {
            var returnObj = BaseMapper.GenerateReturn("No action performed");

            if (HttpContext.Current.Request.Form["fieldId"] == null)
            {
                returnObj = BaseMapper.GenerateReturn("'fieldId' is missing");
                WriteJSON(returnObj.ToJson());
            }

            if (!long.TryParse(HttpContext.Current.Request.Form["fieldId"], out long fieldId))
            {
                returnObj = BaseMapper.GenerateReturn("Invalid 'fieldId'");
                WriteJSON(returnObj.ToJson());
            }

            var field = FieldsMapper.GetByID(fieldId);

            if (field == null)
            {
                returnObj = BaseMapper.GenerateReturn($"Cannot find field with id '{fieldId}'");
                WriteJSON(returnObj.ToJson());
            }

            var FormDictionary = new Dictionary<string, string>();

            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                FormDictionary.Add(key, HttpContext.Current.Request.Form[key]);
            }

            FormDictionary.Add("DateLastModified", StringHelper.FormatDateTime(DateTime.Now));

            var jsonEntry = new JavaScriptSerializer().Serialize(FormDictionary);
            field.FieldSettings = jsonEntry;

            returnObj = FieldsMapper.Update(field);

            WriteJSON(returnObj.ToJson());

        }
        
        public Return FieldFrontEndFormSubmissionHandler(long fieldId)
        {
            var returnObj = BaseMapper.GenerateReturn("No action performed");            

            if (HttpContext.Current.Request.Form["fieldId"] == null)
            {
                returnObj = BaseMapper.GenerateReturn("'fieldId' is missing");
                return returnObj;
            }

            var field = (MediaDetailField)FieldsMapper.GetByID(fieldId);

            if (field == null)
            {
                returnObj = BaseMapper.GenerateReturn($"Cannot find field with id '{fieldId}'");
                return returnObj;
            }

            var FormDictionary = new Dictionary<string, string>();

            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                FormDictionary.Add(key, HttpContext.Current.Request.Form[key]);
            }

            FormDictionary.Add("DateSubmitted", StringHelper.FormatDateTime(DateTime.Now));

            var currentEntries = StringHelper.JsonToObject<Newtonsoft.Json.Linq.JArray>(field.FrontEndSubmissions);

            var files = new Dictionary<string,List<string>>();
            var fileIndex = 0;
            foreach (string key in HttpContext.Current.Request.Files)
            {
                var postedFile = HttpContext.Current.Request.Files[fileIndex];

                if(!field.UploadFolder.Exists)
                {
                    field.UploadFolder.Create();
                }

                var uploadFilePath = field.UploadFolder.FullName + key + "_" + postedFile.FileName;
                postedFile.SaveAs(uploadFilePath);

                var relativePath = URIHelper.ConvertAbsPathToAbsUrl(uploadFilePath);

                if (files.ContainsKey(key))
                {
                    files[key].Add(relativePath);
                }
                else
                {
                    files.Add(key, new List<string>() { relativePath });
                }

                fileIndex++;
            }

            var jObjectUploadFiles = JObject.Parse(StringHelper.ObjectToJson(files));

            var jsonEntry = new JavaScriptSerializer().Serialize(FormDictionary);

            var jObject = JObject.Parse(jsonEntry);

            jObject.Merge(jObjectUploadFiles);

            if (currentEntries == null)
            {
                currentEntries = new JArray();
                currentEntries.Add(jObject);
            }
            else
            {
                currentEntries.Add(jObject);
            }
            
            field.FrontEndSubmissions = currentEntries.ToString(Formatting.None);

            returnObj = FieldsMapper.Update(field);

            var formFieldSettings = StringHelper.JsonToObject<FormFieldSettings>(field.FieldSettings);

            if(formFieldSettings != null && !string.IsNullOrEmpty(formFieldSettings.EmailTemplateMediaID) && long.TryParse(formFieldSettings.EmailTemplateMediaID, out long i))
            {
                var media = MediasMapper.GetByID(long.Parse(formFieldSettings.EmailTemplateMediaID));

                if(media != null)
                {
                    var layout = MediaDetailsMapper.ParseSpecialTags(media.GetLiveMediaDetail());
                    var parsedLayout = ParserHelper.ParseData(layout, FormDictionary);

                    EmailHelper.SendDirectMessage(AppSettings.SystemEmailAddress, formFieldSettings.EmailAddress, "Form Submission", parsedLayout);
                }                
            }            

            return returnObj;
        }
    }
}
