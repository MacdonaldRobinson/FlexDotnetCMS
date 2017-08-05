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

            return returnObj;
        }
    }
}
