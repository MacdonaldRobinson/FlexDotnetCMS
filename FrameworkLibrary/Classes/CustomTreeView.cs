using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrameworkLibrary.Classes
{
    public class CustomTreeNode : TreeNode
    {
        private Dictionary<string, string> _LIAttributes = new Dictionary<string, string>();
        private List<string> _LIClasses = new List<string>();
        private Dictionary<string, string> _LinkAttributes = new Dictionary<string, string>();
        private List<string> _LinkClasses = new List<string>();

        public CustomTreeNode()
        {
        }

        public CustomTreeNode(string text, string value, string EditUrl = "")
            : base(text, value)
        {
            if (!string.IsNullOrEmpty(EditUrl))
                this.NavigateUrl = EditUrl;
        }

        public Dictionary<string, string> LIAttributes
        {
            get
            {
                return _LIAttributes;
            }
        }

        public List<string> LIClasses
        {
            get
            {
                return _LIClasses;
            }
        }

        public Dictionary<string, string> LinkAttributes
        {
            get
            {
                return _LinkAttributes;
            }
        }

        public List<string> LinkClasses
        {
            get
            {
                return _LinkClasses;
            }
        }

        public string GetLinkAttributesAsString()
        {
            var attrs = "";
            foreach (var item in LinkAttributes)
            {
                attrs += " " + item.Key + "='" + item.Value + "'";
            }

            if (LinkClasses.Count > 0)
            {
                attrs += " class='";
                foreach (var item in LinkClasses)
                {
                    attrs += " " + item;
                }
                attrs += "'";
            }

            return attrs;
        }

        public string GetLIAttributesAsString()
        {
            var attrs = "";
            foreach (var item in LIAttributes)
            {
                if (string.IsNullOrEmpty(attrs))
                    attrs += item.Key + "='" + item.Value + "'";
                else
                    attrs += " " + item.Key + "='" + item.Value + "'";
            }

            if (LIClasses.Count > 0)
            {
                attrs += " class='";
                foreach (var item in LIClasses)
                {
                    if (string.IsNullOrEmpty(attrs))
                        attrs += item;
                    else
                        attrs += " " + item;
                }
                attrs += "'";
            }

            return attrs;
        }
    }

    public class CustomTreeView : TreeView
    {
        protected override TreeNode CreateNode()
        {
            return new CustomTreeNode();
        }
    }
}