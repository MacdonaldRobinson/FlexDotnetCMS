using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;

namespace FrameworkLibrary.Classes.ControlAdapters
{
    public class TreeViewAdapter : ControlAdapter
    {
        protected CustomTreeView Target
        {
            get
            {
                return (this.Control as CustomTreeView);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("ul");
            writer.WriteLine();
            foreach (CustomTreeNode node in Target.Nodes)
            {
                RenderNode(node, writer);
            }
            writer.WriteLine();
            writer.WriteEndTag("ul");
        }

        private void RenderNode(CustomTreeNode node, HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("li " + node.GetLIAttributesAsString());
            writer.WriteFullBeginTag("a href=\"" + node.NavigateUrl + "\"" + node.GetLinkAttributesAsString());
            writer.Write(node.Text);
            writer.WriteEndTag("a");
            if (node.ChildNodes.Count > 0)
            {
                writer.WriteFullBeginTag("ul");
                foreach (CustomTreeNode childNode in node.ChildNodes)
                {
                    RenderNode(childNode, writer);
                }
                writer.WriteEndTag("ul");
            }
            writer.WriteEndTag("li");
        }
    }
}