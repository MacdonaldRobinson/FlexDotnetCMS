using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace FrameworkLibrary
{
    public class ParserHelper
    {
        private static string openToken = "{";
        private static string closeToken = "}";

        public static string ParseData(string data, Dictionary<string, string> keyValuePair, bool isReverseParse = false)
        {
            if (data == null)
                return "";

            foreach (KeyValuePair<string, string> item in keyValuePair)
            {
                if (isReverseParse)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                        data = data.Replace(item.Value, openToken + item.Key + closeToken);
                }
                else
                    data = data.Replace(openToken + item.Key + closeToken, item.Value);
            }

            return data;
        }

        public static string ParseData(string data, object obj, bool compileRazor = true)
        {
            if (obj == null)
                return "";

            data = data.Trim();

            if (data.StartsWith("@") && data.EndsWith("}"))
            {
                data = RunOrCompileRazorCode(data, data, obj, compileRazor);
            }

            var matches = Regex.Matches(data, openToken + "[a-zA-Z0-9-.&=<>/\\;(\n|\r|\r\n)\"#?']+" + closeToken);

            foreach (var item in matches)
            {
                var tag = item.ToString();
                var tagValue = "";
                var propertyName = tag.Replace("{", "").Replace("}", "");
                var queryStringParams = "";

                if (propertyName.Contains("?"))
                {
                    var segments = propertyName.Split('?').ToList();
                    propertyName = segments[0];
                    segments.RemoveAt(0);
                    queryStringParams = string.Join("", segments);
                }

                var nestedProperties = StringHelper.SplitByString(propertyName, ".");

                if (nestedProperties.Length > 0)
                {
                    if (obj == null)
                        continue;

                    object tempNestedProperty = obj;
                    var propertyloopIndex = 0;

                    foreach (var nestedProperty in nestedProperties)
                    {
                        PropertyInfo tempPropertyInfo = null;
                        MethodInfo tempMethodInfo = null;

                        var matchParams = Regex.Matches(nestedProperty, "([a-zA-Z0-9]+)");

                        var methodParamsMatches = new List<string>();
                        
                        for (int i = 0; i < matchParams.Count; i++)
                        {
                            var val = matchParams[i].ToString();
                            if (val != "quot")
                            {
                                methodParamsMatches.Add(val);
                            }
                        }

                        if (nestedProperty.Contains("(") && !nestedProperty.Contains("."))
                        {
                            try
                            {
                                tempMethodInfo = tempNestedProperty.GetType().GetMethod(methodParamsMatches[0]);
                            }
                            catch(Exception ex)
                            {
                                ErrorHelper.LogException(ex);
                            }
                        }
                        else
                        {
                            var prop = nestedProperty;

                            var queryParamsSplit = nestedProperty.Split('?');
                            if (queryParamsSplit.Count() > 1)
                                prop = queryParamsSplit.ElementAt(0);


                            tempPropertyInfo = tempNestedProperty.GetType().GetProperty(prop);
                        }

                        if (tempPropertyInfo != null || tempMethodInfo != null)
                        {
                            if (tempPropertyInfo != null)
                            {
                                tempNestedProperty = tempPropertyInfo.GetValue(tempNestedProperty, null);
                            }
                            else if (tempMethodInfo != null)
                            {
                                var objParams = new object[methodParamsMatches.Count - 1];
                                var parametersInfo = tempMethodInfo.GetParameters();

                                for (var i = 0; i < methodParamsMatches.Count - 1; i++)
                                {
                                    if (parametersInfo.Count() > i)
                                        objParams[i] = Convert.ChangeType(methodParamsMatches[i + 1], parametersInfo[i].ParameterType);
                                }

                                tempNestedProperty = tempMethodInfo.Invoke(tempNestedProperty, objParams.Where(i => i != null)?.ToArray());
                            }

                            if (tempNestedProperty != null)
                            {
                                var hasEnumerable = tempNestedProperty.GetType().ToString().Contains("Enumerable") || tempNestedProperty.GetType().ToString().Contains("Collection");
                                long tmpIndex = 0;

                                if (nestedProperties.Count() > propertyloopIndex + 1)
                                {
                                    if (hasEnumerable)
                                    {
                                        if (long.TryParse(nestedProperties[propertyloopIndex + 1], out tmpIndex))
                                        {
                                            var count = 0;
                                            foreach (var nestedItem in tempNestedProperty as IEnumerable<object>)
                                            {
                                                if (count == tmpIndex)
                                                {
                                                    tempNestedProperty = nestedItem;
                                                    break;
                                                }

                                                count++;
                                            }

                                            continue;
                                        }
                                        else
                                        {
                                            var count = 0;
                                            var returnValue = "";
                                            var tempPropertiesString = "";

                                            var tmp = nestedProperties.ToList();
                                            tmp.RemoveAt(propertyloopIndex);

                                            var newPropertyString = string.Join(".", tmp);

                                            foreach (var nestedItem in (dynamic)tempNestedProperty)
                                            {
                                                var tmpReturn = ParseData("{" + newPropertyString + "}", nestedItem);
                                                returnValue += ParseData(tmpReturn, nestedItem);

                                                /*var tmpReturn = ParseData("{" + nestedProperties[propertyloopIndex + 1] + "}", nestedItem);
                                                returnValue += ParseData(tmpReturn, nestedItem);
                                                count++;*/
                                            }

                                            tagValue = returnValue;
                                        }
                                    }
                                }
                                else
                                {
                                    if (nestedProperties.Length < propertyloopIndex + 1)
                                    {
                                        return ParseData("{" + nestedProperties[propertyloopIndex + 1] + "}", tempNestedProperty);
                                    }
                                    else if (tempNestedProperty is string)
                                    {
                                        if (tempMethodInfo != null)
                                        {
                                            tagValue = tempNestedProperty.ToString();
                                        }
                                        //return ParseData("{" + nestedProperties[propertyloopIndex + 1] + "}", tempNestedProperty);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var splitEq = nestedProperty.ToString().Split('=');
                            if (splitEq.Count() > 1)
                            {
                                tempPropertyInfo = tempNestedProperty.GetType().GetProperty(splitEq[0]);

                                if (tempPropertyInfo != null)
                                {
                                    var returnVal = tempPropertyInfo.GetValue(tempNestedProperty, null);

                                    if (splitEq[1].Replace("\"", "") == returnVal.ToString())
                                    {
                                        var tmp = nestedProperties.ToList();
                                        tmp.RemoveAt(propertyloopIndex);

                                        var newPropertyString = string.Join(".", tmp);

                                        tempNestedProperty = ParseData("{" + newPropertyString + "}", tempNestedProperty);
                                    }
                                    else
                                        tempNestedProperty = "";
                                }
                            }
                        }

                        propertyloopIndex++;
                    }

                    if (tempNestedProperty is DateTime)
                        tagValue = data.Replace(item.ToString(), StringHelper.FormatOnlyDate((DateTime)tempNestedProperty));

                    if (tempNestedProperty is string || tempNestedProperty is bool || tempNestedProperty is long)
                    {
                        var val = tempNestedProperty.ToString();

                        var queryStringSplit = item.ToString().Replace(OpenToken, "").Replace(CloseToken, "").Split('?');

                        if (queryStringSplit.Count() > 1)
                        {
                            var nv = HttpUtility.ParseQueryString(queryStringSplit.ElementAt(1));

                            foreach (string key in nv)
                            {
                                var value = nv[key];
                                val = val.Replace(OpenToken + key + CloseToken, value);
                            }

                        }

                        tagValue = data.Replace(item.ToString(), val);
                    }
                }

                if (tagValue.StartsWith("~/"))
                    tagValue = URIHelper.ConvertToAbsUrl(tagValue);

                if (!string.IsNullOrEmpty(tagValue))
                {
                    data = RunOrCompileRazorCode(tag, tagValue, obj, compileRazor);
                }
            }

            data = RunOrCompileRazorCode(data, data, obj, compileRazor);

            return data;
        }

        public static string ReplaceHrefAndSrcsToAbsoluteUrls(string data)
        {
            if (data.Contains("src=\"/") || data.Contains("src='/") || data.Contains("href=\"/") || data.Contains("href='/"))
            {
                data = StringHelper.Replace(data, "src=\"/", $"src=\"{URIHelper.BaseUrl}");
                data = StringHelper.Replace(data, "src='/", $"src='{URIHelper.BaseUrl}");
                data = StringHelper.Replace(data, "href=\"/", $"href=\"{URIHelper.BaseUrl}");
                data = StringHelper.Replace(data, "href='/", $"href='{URIHelper.BaseUrl}");
                data = StringHelper.Replace(data, URIHelper.BaseUrl + "/", "//");
            }

            return data;
        }

        public static string RunOrCompileRazorCode(string tag, string code, object obj, bool compileRazor)
        {
            /*var config = new TemplateServiceConfiguration();
            config.Debug = true;

            config.EncodedStringFactory = new RawStringFactory();
            var service = RazorEngineService.Create(config);

            Engine.Razor = service;*/

            if (!string.IsNullOrEmpty(code) && !code.Contains("{&quot;") && !code.Contains("{\r\n  &quot;") && (code.Contains("@{") || code.Contains("@using") || code.Contains("@for") || code.Contains("@Model")) && compileRazor)
            {
                code = "@using FrameworkLibrary\n@using System\n@using System.Linq\n@using System.Web\n" + code;
                var key = "templateKey:" + code;

                try
                {
                    if (Engine.Razor.IsTemplateCached(key, null))
                    {
                        return Engine.Razor.Run(key, null, obj);
                    }

                    var result = Engine.Razor.RunCompile(code, key, null, obj);
                    return result;
                }
                catch (RazorEngine.Templating.TemplateCompilationException ex)
                {
                    if(tag.StartsWith("{"))
                    {
                        code = tag;
                    }
                    else
                    {
                        code = "";
                    }

                    var error = ErrorHelper.CreateError(string.Join("\n", ex.CompilerErrors.Select(i=>i.ErrorText)), code);
                    ErrorHelper.LogException(error.Exception);               

                    throw new Exception(tag + "\r\n\r\n"+ error.Exception.Message +"\r\n\r\n"+error.Exception.InnerException);
                }                
            }       
            else
            {
                return code;
            }
        }

        public static void SetValue(object obj, string propertyName, object value)
        {
            if (propertyName.Contains("{"))
                propertyName.Replace("{", "");

            if (propertyName.Contains("}"))
                propertyName.Replace("}", "");

            var nestedProperties = StringHelper.SplitByString(propertyName, ".");

            if (nestedProperties.Length > 0)
            {
                object tempNestedProperty = obj;
                foreach (var nestedProperty in nestedProperties)
                {
                    var tempPropertyInfo = tempNestedProperty.GetType().GetProperty(nestedProperty);

                    if (tempPropertyInfo != null)
                    {
                        var val = tempPropertyInfo.GetValue(tempNestedProperty, null);

                        if (val is string || val is bool || val is long || val == null)
                        {
                            if (System.ComponentModel.TypeDescriptor.GetConverter(tempPropertyInfo.PropertyType).CanConvertFrom(value.GetType()))
                            {
                                if (value != "")
                                {
                                    try
                                    {
                                        tempPropertyInfo.SetValue(tempNestedProperty, Convert.ChangeType(value, tempPropertyInfo.PropertyType), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorHelper.LogException(new Exception($"Error setting value for property '{tempPropertyInfo.Name}' with value '{value}' for control with ID '{((System.Web.UI.Control)obj).ClientID}'", ex));
                                    }
                                }
                            }
                        }
                        else
                            tempNestedProperty = val;
                    }
                }
            }
        }

        public static string OpenToken
        {
            get
            {
                return openToken;
            }
            set
            {
                openToken = value;
            }
        }

        public static string CloseToken
        {
            get
            {
                return closeToken;
            }
            set
            {
                closeToken = value;
            }
        }
    }
}