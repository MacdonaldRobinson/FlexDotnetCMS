using RazorEngine;
using RazorEngine.Templating;
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

            var matches = Regex.Matches(data, openToken + "[a-zA-Z0-9.\\[\\]\\(\\=><\"\"\\(),\'?&/) ]+" + closeToken);

            foreach (var item in matches)
            {
                var tag = item.ToString();
                var tagValue = "";
                var propertyName = tag.Replace("{", "").Replace("}", "");

                var nestedProperties = StringHelper.SplitByString(propertyName, ".");

                if (nestedProperties.Length > 0)
                {
                    object tempNestedProperty = obj;
                    var propertyloopIndex = 0;

                    foreach (var nestedProperty in nestedProperties)
                    {
                        PropertyInfo tempPropertyInfo = null;
                        MethodInfo tempMethodInfo = null;

                        var methodParamsMatches = Regex.Matches(nestedProperty, "([a-zA-Z0-9]+)");

                        if (nestedProperty.Contains("(") && !nestedProperty.Contains("."))
                        {
                            tempMethodInfo = tempNestedProperty.GetType().GetMethod(methodParamsMatches[0].Value);
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
                                        objParams[i] = Convert.ChangeType(methodParamsMatches[i + 1].Value, parametersInfo[i].ParameterType);
                                }

                                tempNestedProperty = tempMethodInfo.Invoke(tempNestedProperty, objParams.Where(i => i != null).ToArray());
                            }

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
                                else if (tempNestedProperty is object)
                                {
                                    //ParseData("{" + nestedProperties[propertyloopIndex + 1] + "}", tempNestedProperty);
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

                //foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                //{
                //    Template.RegisterSafeType(type, type.GetMembers().Select(i => i.Name).ToArray());
                //}

                //var template = Template.Parse(tagValue);
                //tagValue = template.Render(Hash.FromAnonymousObject(obj));

                if (!string.IsNullOrEmpty(tagValue) && tagValue.Contains("@") && compileRazor)
                {
                    var razorTagValue = "@using FrameworkLibrary\n@using System\n@using System.Linq\n@using System.Web\n" + tagValue;
                    var tagKey = "templateKey:" + tagValue;

                    try
                    {
                        var result = RunOrCompileRazorCode(razorTagValue, tagKey, obj);

                        if (tagValue != result)
                        {
                            data = data.Replace(tag, result);
                        }
                        else
                        {
                            data = tagValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHelper.LogException(ex);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(tagValue))
                        data = tagValue;
                }
            }

            if (!string.IsNullOrEmpty(data) && data.Contains("@") && !data.StartsWith("{") && compileRazor)
            {
                data = "@using FrameworkLibrary\n@using System\n@using System.Linq\n@using System.Web\n" + data;
                var topTagKey = "templateKey:" + data;

                try
                {
                    var topResult = RunOrCompileRazorCode(data, topTagKey, obj);
                    data = data.Replace(data, topResult);
                }
                catch (Exception ex)
                {
                    ErrorHelper.LogException(ex);
                }
            }

            return data;
        }

        public static string RunOrCompileRazorCode(string code, string key, object obj)
        {
            if (Engine.Razor.IsTemplateCached(key, null))
            {
                return Engine.Razor.Run(key, null, obj);
            }

            var result = Engine.Razor.RunCompile(code, key, null, obj);

            return result;
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