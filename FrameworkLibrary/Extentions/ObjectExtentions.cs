using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        private static int maxDepthAllowed = 100;

        static ObjectExtentions()
        {
            //ommitValuesBySegment.Add(".EntityReference");
            //ommitValuesBySegment.Add(".EntityCollection");
            //ommitValuesBySegment.Add("FrameworkLibrary");

            ommitPropertiesBySegment.Add("EntityKey");
            ommitPropertiesBySegment.Add("EntityState");
            ommitPropertiesBySegment.Add("CreatedByUser");
            ommitPropertiesBySegment.Add("LastUpdatedByUser");
            ommitPropertiesBySegment.Add("ValidationErrors");
            ommitPropertiesBySegment.Add("Count");
            ommitPropertiesBySegment.Add("Capacity");
            ommitPropertiesBySegment.Add("LiveMediaDetail");
			ommitPropertiesBySegment.Add("ChildMedias");
			ommitPropertiesBySegment.Add("MediaType");
			ommitPropertiesBySegment.Add("MediaDetail");
			ommitPropertiesBySegment.Add("Parent");
			//ommitPropertiesBySegment.Add("Use");
			//ommitPropertiesBySegment.Add("Media");
			ommitPropertiesBySegment.Add("Language");
            ommitPropertiesBySegment.Add("CacheData");
        }

		public static string DataTableToCSV(this DataTable datatable, char seperator)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < datatable.Columns.Count; i++)
			{
				sb.Append(datatable.Columns[i]);
				if (i < datatable.Columns.Count - 1)
					sb.Append(seperator);
			}
			sb.AppendLine();
			foreach (DataRow dr in datatable.Rows)
			{
				for (int i = 0; i < datatable.Columns.Count; i++)
				{
					if (dr[i] is string[])
					{
						var list = dr[i] as string[];
						var val = string.Join(";", list);

						if (val.Contains(","))
						{
							val = "\"" + val + "\"";
						}

						sb.Append(val);
					}
					else
					{
						var value = dr[i].ToString();

						if (value.Contains("\""))
						{
							value = value.Replace("\"", "'");
						}

						if (value.Contains(","))
						{
							value = "\"" + value + "\"";
						}

						sb.Append(value);
					}

					if (i < datatable.Columns.Count - 1)
						sb.Append(seperator);
				}
				sb.AppendLine();
			}
			return sb.ToString();
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

                try
                {
                    var value = property.GetValue(from, null);

                    updatedProperties.Add(toProperty, value);

                    UpdateProperty(toProperty, value, to, from);
                }
                catch(Exception ex)
                {
                    ErrorHelper.LogException(ex);
                }
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
				var propertyName = toProperty.Name.ToString();

				if (propertyName == "EntityKey")
                    return;

                if (propertyName == "ID")
                    return;

                if (value.GetType().BaseType == typeof(EntityReference))
                    return;

                if (value.GetType().Name.Contains("EntityCollection"))
                    return;

				if (value.GetType().Name.Contains("HashSet"))
					return;

				if (value.GetType().Name.Contains("EntityReference"))
					return;

				if (value.GetType().BaseType.ToString().Contains("FrameworkLibrary"))
					return;

				if (value.GetType().BaseType == typeof(EntityObject))
                {
                    value = BaseMapper.GetObjectFromContext((IMustContainID)value);
                }
            }

            toProperty.SetValue(toObject, value, null);
        }

        public static string ToJson(this object to, long toDepth = 1)
        {
            var tmpMaxDepthAllowed = maxDepthAllowed;

            if (toDepth < maxDepthAllowed)
                maxDepthAllowed = int.Parse(toDepth.ToString());

            var json = _ToJson(to, 1);

            maxDepthAllowed = tmpMaxDepthAllowed;

            return json;
        }

        private static string _ToJson<T>(this T to, long depthCount = 1)
        {
            var json = "{";

            var properties = to.GetType().GetProperties();

            foreach (var property in properties)
            {
				var strPropertyName = property.ToString();

				var ommit = false;
                var toProperty = to.GetType().GetProperty(property.Name);

                if (toProperty == null) continue;

                ommit = ommitPropertiesBySegment.Any(ommitPropertySegment =>
                {
                    if (property.Name.StartsWith(ommitPropertySegment) && !property.Name.Contains("ID"))
                        return true;

                    return false;
                });

                if (ommit)
                    continue;

                var isAssemblyObject = false;
                object value = to;

                if (to.GetType().GetInterface("IEnumerable") == null && (to.GetType() != typeof(System.String)))
                {
                    value = property.GetValue(to, null) ?? "";

                    if (value.ToString().Length > 0)
                    {
                        var assemblyClasses = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsClass);

                        if (assemblyClasses.Any(assemblyClass =>
                        {
                            if (assemblyClass.ToString() == value.ToString())
                                return true;

                            if (value.GetType().GetInterface("IEnumerable") != null)
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
                    value = value.ToJson(depthCount + 1);
                }
                else
                {
                    if (!value.GetType().ToString().Contains("String") && value.GetType().GetInterface("IEnumerable") != null)
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
                            var counter = 1;
							var tmpJson = "";

							if (depthCount < maxDepthAllowed)
                            {
								if (!dynValue.GetType().ToString().Contains("String"))
								{
									tmpJson = "[";

									foreach (var item in dynValue)
									{
										//tmpJson += ObjectExtentions.ToJson(item, depthCount);
										/*if (item.GetType().GetInterface("IEnumerator") != null)
										{*/
											tmpJson +=  ObjectExtentions.ToJson(item, depthCount);

											if (counter >= max)
												continue;

											tmpJson += ", ";											
										/*}
										else
										{
											tmpJson += "\"" + dynValue + "\"";

											if (counter >= max)
												continue;

											tmpJson += ", ";
											counter++;

											continue;
										}*/
									}									

									tmpJson += "]";

									if (tmpJson.Contains("}, ]"))
									{
										tmpJson = tmpJson.Replace("}, ]", "}]");
									}
									counter++;
								}
								else
								{
									tmpJson += "\"" + dynValue + "\"";

									if (counter >= max)
										continue;

									tmpJson += ", ";
								}
                            }														

                            value = tmpJson;
                        }
                    }
                    else
                    {
                        if (!value.GetType().BaseType.GetInterfaces().Any())
                        {
                            value = "\"" + StringHelper.JavascriptStringEncode(value.ToString().Replace(System.Environment.NewLine, "")) + "\"";
                        }
                        else if (value is object)
                        {
                            if (depthCount < maxDepthAllowed)
                            {
                                //depthCount = depthCount + 1;
                                value = _ToJson(value, depthCount);
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


				if (string.IsNullOrEmpty(value.ToString()))
				{
					value = "\""+value.ToString()+"\"";
				}

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