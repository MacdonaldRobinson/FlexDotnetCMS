using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class AssociationDropDownList : BaseFieldControl
    {
        public void Page_Init(object sender, EventArgs e)
        {
            BindItems();
        }

        private void BindItems()
        {
            ItemsList.DataSource = MediaDetailsMapper.GetByID(ParentMediaDetailID).ChildMediaDetails;
            ItemsList.DataTextField = "SectionTitle";
            ItemsList.DataValueField = "ID";
            ItemsList.DataBind();
        }

        private void SetSelectedIds(string values)
        {
            var selectedIds = (object[])StringHelper.JsonToObject(values);

            foreach (ListItem item in ItemsList.Items)
            {
                if (selectedIds.Contains(int.Parse(item.Value)))
                {
                    item.Selected = true;
                }
            }
        }

        public long ParentMediaDetailID { get; set; }

        public override void RenderControlInFrontEnd()
        {
            base.RenderControlInFrontEnd();
        }

        public override void RenderControlInAdmin()
        {
            base.RenderControlInAdmin();
        }

        private void SaveToDB(string values)
        {
            var obj = StringHelper.JsonToObject(values);
            var field = GetField();

            field.FieldAssociations.Clear();

            var orderIndex = 0;
            foreach (var item in obj)
            {
                var mediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(item);
                if (mediaDetail != null)
                {
                    field.FieldAssociations.Add(new FieldAssociation() { MediaDetail = mediaDetail, MediaDetailField = field, OrderIndex = orderIndex });
                    orderIndex++;
                }
            }

            var returnObj = BaseMapper.GetDataModel().SaveChanges();
        }

        public override void SetValue(object values)
        {
            if (values == "")
                return;

            var field = GetField();

            SetSelectedIds(values.ToString());
            SaveToDB(values.ToString());

            Values.Value = StringHelper.ObjectToJson(field.FieldAssociations.Select(i => i.AssociatedMediaDetailID));
        }

        public override object GetValue()
        {
            var listIds = new List<long>();

            foreach (ListItem item in ItemsList.Items)
            {
                if (item.Selected)
                    listIds.Add(long.Parse(item.Value));
            }

            var jsonString = StringHelper.ObjectToJson(listIds);

            return jsonString;
        }
    }
}