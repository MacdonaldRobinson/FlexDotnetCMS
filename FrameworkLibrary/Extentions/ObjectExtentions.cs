using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.UI.WebControls;

namespace FrameworkLibrary
{
    public static class ObjectExtentions
    {
        private static readonly List<string> ommitPropertiesBySegment = new List<string>();
        private static readonly List<string> ommitValuesBySegment = new List<string>();
        private static int maxDepthAllowed = 5;

        static ObjectExtentions()
        {
            ommitValuesBySegment.Add(".EntityReference");
            ommitValuesBySegment.Add(".EntityCollection");
            ommitValuesBySegment.Add("FrameworkLibrary");

            ommitPropertiesBySegment.Add("EntityKey");
            ommitPropertiesBySegment.Add("EntityState");
            ommitPropertiesBySegment.Add("CreatedByUser");
            ommitPropertiesBySegment.Add("LastUpdatedByUser");
            ommitPropertiesBySegment.Add("ValidationErrors");
            ommitPropertiesBySegment.Add("Count");
            ommitPropertiesBySegment.Add("Capacity");
            ommitPropertiesBySegment.Add("LiveMediaDetail");            
            ommitPropertiesBySegment.Add("Language");
            ommitPropertiesBySegment.Add("CacheData");
        }

        public static void ExpandParents(this TreeNode _self)
        {
            if (_self != null && _self.Parent != null)
            {
                _self.Parent.Expand();
                ExpandParents(_self.Parent);
            }
        }

        public static List<TreeNode> GetAllNodes(this TreeView _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        public static List<TreeNode> GetAllNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(_self);
            foreach (TreeNode child in _self.ChildNodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        public static IEnumerable<string> OmmitValuesBySegment
        {
            get
            {
                return ommitValuesBySegment;
            }
        }

        public static IEnumerable<string> OmmitPropertiesBySegment
        {
            get
            {
                return ommitPropertiesBySegment;
            }
        }

        public static T Clone<T>(this T item) where T : class
        {
            if (item == null)
                return default(T);
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);

            var result = (T)formatter.Deserialize(stream);

            stream.Close();

            return result;
        }

        public static void CopyFrom<T>(this T to, object from, IEnumerable<string> omitPoperties = null) where T : class
        {
            if (from == null)
                return;

            if (omitPoperties == null)
                omitPoperties = new List<string>();

            var properties = from.GetType().GetProperties();

            var updatedProperties = new Dictionary<PropertyInfo, object>();

            foreach (var property in properties)
            {
                if (omitPoperties.Contains(property.Name))
                    continue;

                if (ommitValuesBySegment.Any(ommitValueBySegment => property.ToString().Contains(ommitValueBySegment)))
                    continue;

                var toProperty = to.GetType().GetProperty(property.Name);

                if (toProperty == null) continue;
                var value = property.GetValue(from, null);

                updatedProperties.Add(toProperty, value);

                UpdateProperty(toProperty, value, to, from);
            }
        }

        public static bool SearchForTerm<T>(this T obj, string searchTerm, IEnumerable<string> limitToProperties = null) where T : class
        {
            var properties = obj.GetType().GetProperties();
            searchTerm = searchTerm.Trim().ToLower();

            foreach (var property in properties)
            {
                if (property == null) continue;

                if (limitToProperties != null)
                {
                    if (!limitToProperties.Contains(property.Name))
                        continue;
                }

                var ommitSegments = new List<string>();
                ommitSegments.AddRange(ommitPropertiesBySegment);

                var ommit = ommitSegments.Any(ommitPropertySegment =>
                {
                    if (property.Name.Contains(ommitPropertySegment))
                        return true;

                    return false;
                });

                if (ommit)
                    continue;

                var value = property.GetValue(obj, null);

                if ((value != null) && (value is string) && (value.ToString().ToLower().Trim().Contains(searchTerm)))
                    return true;
            }

            return false;
        }

        private static void UpdateProperty<T>(PropertyInfo toProperty, object value, T toObject, T fromObject)
        {
            if (toProperty.GetSetMethod() == null) return;
            if (value != null)
            {
                if (toProperty.Name == "EntityKey")
                    return;

                if (toProperty.Name == "ID")
                    return;

                if (value.GetType().BaseType == typeof(EntityReference))
                    return;

                if (value.GetType().Name.Contains("EntityCollection"))
                    return;

                if (value.GetType().BaseType == typeof(EntityObject))
                {
                    value = BaseMapper.GetObjectFromContext((IMustContainID)value);
                }
            }

            toProperty.SetValue(toObject, value, null);
        }

        public static string ToJSON<T>(this T to, long toDepth = 1) where T : class
        {
            var tmpMaxDepthAllowed = maxDepthAllowed;

            if (toDepth < maxDepthAllowed)
                maxDepthAllowed = int.Parse(toDepth.ToString());

            var json = _ToJSON<T>(to, 1);

            maxDepthAllowed = tmpMaxDepthAllowed;

            return json;
        }

        private static string _ToJSON<T>(this T to, long depthCount = 1) where T : class
        {
            var json = "{";

            var properties = to.GetType().GetProperties();

            foreach (var property in properties)
            {
                var ommit = false;
                var toProperty = to.GetType().GetProperty(property.Name);

                if (toProperty == null) continue;

                ommit = ommitPropertiesBySegment.Any(ommitPropertySegment =>
                {
                    if (property.Name.Contains(ommitPropertySegment))
                        return true;

                    return false;
                });

                if (ommit)
                    continue;

                var isAssemblyObject = false;
                object value = to;

                if (!to.ToString().Contains(".Enumerable") && !to.ToString().Contains(".List") && (to.GetType() != typeof(System.String)))
                {
                    value = property.GetValue(to, null) ?? "";

                    if (value.ToString().Length > 0)
                    {
                        var assemblyClasses = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsClass);

                        if (assemblyClasses.Any(assemblyClass =>
                        {
                            if (assemblyClass.ToString() == value.ToString())
                                return true;

                            if (value.ToString().Contains("Enumerable"))
                            {
                                foreach (var tmp in (IEnumerable)value)
                                {
                                    if (assemblyClass.ToString() == tmp.ToString())
                                        return true;
                                }
                            }

                            return false;
                        }))
                        {
                            isAssemblyObject = true;
                        }
                    }
                }

                var type = value.GetType().ToString();

                if (type.Contains("Elmah") || type.Contains("Exception"))
                {
                    isAssemblyObject = true;
                }

                if ((isAssemblyObject) && (depthCount <= maxDepthAllowed))
                {
                    value = value.ToJSON(depthCount + 1);
                }
                else
                {
                    if ((value.ToString().Contains(".Enumerable") || value.ToString().Contains(".List")) && (to.GetType() != typeof(String)))
                    {
                        var dynValue = (dynamic)value;
                        int max = 0;

                        try
                        {
                            max = Enumerable.Count(dynValue);
                        }
                        catch (Exception ex)
                        {
                        }

                        if (max == 0)
                        {
                            value = "[]";
                        }
                        else
                        {
                            var tmpJson = "[";
                            var counter = 1;

                            if (depthCount < maxDepthAllowed)
                            {
                                foreach (var item in dynValue)
                                {
                                    if (dynValue.GetType().GetInterface("IEnumerator") != null)
                                    {
                                        tmpJson += ObjectExtentions.ToJSON(item, depthCount);
                                    }
                                    else
                                    {
                                        tmpJson += item;
                                    }

                                    if (counter >= max)
                                        continue;
                                    tmpJson += ", ";
                                    counter++;
                                }
                            }

                            tmpJson += "]";

                            value = tmpJson;
                        }
                    }
                    else
                    {
                        if (value is string || value is int || value is long || value is DateTime)
                        {
                            value = "\"" + StringHelper.JavascriptStringEncode(value.ToString().Replace(System.Environment.NewLine, "")) + "\"";
                        }
                        else if (value is object)
                        {
                            if (depthCount < maxDepthAllowed)
                            {
                                depthCount = depthCount + 1;
                                value = "\"" + _ToJSON(value, depthCount) + "\"";
                            }
                            else
                            {
                                value = "\"" + StringHelper.JavascriptStringEncode(value.ToString().Replace(System.Environment.NewLine, "")) + "\"";
                            }
                        }
                    }                        
                }

                if (ommitValuesBySegment.Any(ommitValueBySegment => value.ToString().Contains(ommitValueBySegment)))
                {
                    ommit = true;
                }

                if (ommit)
                    continue;

                json += "\"" + property.Name + "\" : " + value.ToString() + ", ";
            }

            json += "}";

            json = json.Replace(Environment.NewLine, "").Replace(", }", "}");

            return json;
        }

        public static string ToCsv<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}