using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FrameworkLibrary
{
    public class WebFormHelper
    {
        private Dictionary<string, Dictionary<string, HtmlControl>> list = new Dictionary<string, Dictionary<string, HtmlControl>>();
        private string loadFileServiceUrl = "";
        private bool combineCssAndJsIncludes = true;

        public string LoadFileServiceUrl
        {
            get
            {
                return loadFileServiceUrl;
            }
            set
            {
                loadFileServiceUrl = value;
            }
        }

        public bool CombineCssAndJsIncludes
        {
            get
            {
                return combineCssAndJsIncludes;
            }
            set
            {
                combineCssAndJsIncludes = value;
            }
        }

        public void AddCSSFile(string relToRootPath, Control addToControl, string templateBaseUrl, Dictionary<string, string> attributes = null)
        {
            AddCustomTag(GenerateCssFileTag(relToRootPath, templateBaseUrl, attributes), addToControl);
        }

        public HtmlLink GenerateCssFileTag(string relToRootPath, string templateBaseUrl, Dictionary<string, string> attributes = null, bool useUrlAsIs = false)
        {
            HtmlLink tag = new HtmlLink();

            var url = relToRootPath;

            if (!useUrlAsIs)
                url = URIHelper.ConvertToAbsUrl(LoadFileServiceUrl) + URIHelper.ConvertToAbsUrl(relToRootPath);
            //url = URIHelper.ConvertToAbsUrl(LoadFileServiceUrl) + URIHelper.ConvertToAbsUrl(relToRootPath) + "&templateBaseUrl=" + templateBaseUrl;

            if (!url.Contains("?"))
                url = url.Replace("&", "?");

            tag.Href = url;

            tag.Attributes.Add("type", "text/css");
            tag.Attributes.Add("rel", "stylesheet");

            tag = AddAttributesToTag(tag, attributes);

            return tag;
        }

        public T AddAttributesToTag<T>(T tag, Dictionary<string, string> attributes) where T : HtmlControl
        {
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                    tag.Attributes.Add(attribute.Key, attribute.Value);
            }

            return tag;
        }

        public void ClearIncludesList()
        {
            list.Clear();
        }

        public Dictionary<string, HtmlControl> GetJSIncludes(Control placeHolderControl)
        {
            return GetJSIncludes(placeHolderControl.UniqueID);
        }

        public Dictionary<string, HtmlControl> GetCSSIncludes(Control placeHolderControl)
        {
            return GetCSSIncludes(placeHolderControl.UniqueID);
        }

        public Dictionary<string, HtmlControl> GetJSIncludes(string includePlaceHolderId)
        {
            return GetIncludesByTagName("script", includePlaceHolderId);
        }

        public Dictionary<string, HtmlControl> GetCSSIncludes(string includePlaceHolderId)
        {
            return GetIncludesByTagName("link", includePlaceHolderId);
        }

        public Dictionary<string, HtmlControl> GetIncludesByTagName(string tagName, string includePlaceHolderId)
        {
            Dictionary<string, HtmlControl> controls = new Dictionary<string, HtmlControl>();

            IncludedList.Where(i => i.Key.Contains(includePlaceHolderId)).ToList().ForEach(j =>
            {
                j.Value.Keys.Where(k => k.StartsWith(tagName + "_")).ToList().ForEach(l =>
                {
                    controls.Add(l, j.Value[l]);
                });
            });

            return controls;
        }

        public Dictionary<string, Dictionary<string, HtmlControl>> IncludedList
        {
            get
            {
                return list;
            }
        }

        public bool ControlWithIDExists(ControlCollection collection, string id)
        {
            bool exists = false;
            foreach (Control c in collection)
            {
                if (c.ID == id)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public void AddJSFile(string relToRootPath, Control addToControl, string templateBaseUrl, Dictionary<string, string> attributes = null)
        {
            AddCustomTag(GenerateJsFileTag(relToRootPath, templateBaseUrl, attributes), addToControl);
        }

        public HtmlGenericControl GenerateJsFileTag(string relToRootPath, string templateBaseUrl, Dictionary<string, string> attributes = null, bool useUrlAsIs = false)
        {
            HtmlGenericControl tag = new HtmlGenericControl();

            tag.TagName = "script";

            var url = relToRootPath;

            if (!useUrlAsIs)
                url = URIHelper.ConvertToAbsUrl(LoadFileServiceUrl) + URIHelper.ConvertToAbsUrl(relToRootPath);
            //url = URIHelper.ConvertToAbsUrl(LoadFileServiceUrl) + URIHelper.ConvertToAbsUrl(relToRootPath) + "&templateBaseUrl=" + templateBaseUrl;

            if (!url.Contains("?"))
                url = url.Replace("&", "?");

            tag.Attributes.Add("src", url);
            tag.Attributes.Add("type", "text/javascript");

            tag = AddAttributesToTag(tag, attributes);

            return tag;
        }

        public void AddMetaTag(AttributeCollection attrs, Control addToControl)
        {
            HtmlMeta tag = new HtmlMeta();

            foreach (string attr in attrs.Keys)
                tag.Attributes.Add(attr, attrs[attr]);

            AddCustomTag(tag, addToControl);
        }

        public void AddCustomTag(HtmlControl tag, Control addToControl)
        {
            string controlUniqueId = addToControl.UniqueID.ToString();
            string id = tag.TagName + "_";

            foreach (string attr in tag.Attributes.Keys)
            {
                id += attr + "=" + tag.Attributes[attr] + ";";
            }

            id = id.ToLower();

            KeyValuePair<string, HtmlControl> entry = new KeyValuePair<string, HtmlControl>(id, tag);

            if (list.Keys.Contains(controlUniqueId))
            {
                if (!list[controlUniqueId].Keys.Contains(entry.Key))
                {
                    list[controlUniqueId].Add(entry.Key, entry.Value);

                    if (!combineCssAndJsIncludes)
                        addToControl.Controls.Add(tag);
                }
            }
            else
            {
                if (list != null && !list.ContainsKey(controlUniqueId))
                    list.Add(controlUniqueId, new Dictionary<string, HtmlControl>());

                if (!list[controlUniqueId].Keys.Contains(entry.Key))
                {
                    list[controlUniqueId].Add(entry.Key, entry.Value);

                    if (!combineCssAndJsIncludes)
                        addToControl.Controls.Add(tag);
                }
            }
        }

        public static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id) return root;
            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);
                if (t != null) return t;
            }
            return null;
        }

		public static Control FindParentControlWithID(Control control, string id)
		{
			if (control == null)
				return null;

			if (control.Parent == null)
				return null;

			if (control.Parent.ID == id)
				return control.Parent;

			return FindParentControlWithID(control.Parent, id);
		}

		public static Control FindParentControlWithType(Control control, Type type)
		{
			if (control == null)
				return null;

			if (control.Parent == null)
				return null;

			if (control.Parent.GetType() == type)
				return control.Parent;

			return FindParentControlWithType(control.Parent, type);
		}

		public static IEnumerable<Control> FindControlsByCssClass(Control root, string cssClass)
        {
            var controls = new List<Control>();

            dynamic control = root;
            dynamic cssClasses = "";

            if ((control is WebControl) || (control is HtmlControl))
            {
                if (control is WebControl)
                    cssClasses = control.CssClass;
                else
                    cssClasses = control.Attributes["class"];

                if ((cssClasses != null) &&
                    ((cssClasses.Contains(" " + cssClass)) ||
                     ((cssClasses == cssClass)) ||
                     (cssClasses.Contains(cssClass + " "))))
                {
                    controls.Add(root);
                    return controls;
                }
            }

            foreach (Control c in root.Controls)
            {
                var t = FindControlsByCssClass(c, cssClass);

                if (t.Any())
                    controls.AddRange(t);
            }
            return controls.Distinct();
        }

        public static IEnumerable<Control> FindControlsRecursive(Control root, string id)
        {
            var controls = new List<Control>();
            if (root.ID == id)
            {
                controls.Add(root);
                return controls;
            }

            foreach (Control c in root.Controls)
            {
                var t = FindControlsRecursive(c, id);

                if (t.Any())
                    controls.AddRange(t);
            }
            return controls.Distinct();
        }
    }
}