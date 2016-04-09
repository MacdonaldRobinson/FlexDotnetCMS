using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FrameworkLibrary
{
    public static class XmlHelper
    {
        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(string objectData, string rootElementName)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T), rootElementName);
        }

        public static object XmlDeserializeFromString(string objectData, Type type, string rootElementName)
        {
            try
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = rootElementName;
                // xRoot.Namespace = "http://www.cpandl.com";
                xRoot.IsNullable = true;

                var serializer = new XmlSerializer(type, xRoot);
                object result;

                using (TextReader reader = new StringReader(objectData))
                {
                    result = serializer.Deserialize(reader);
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}