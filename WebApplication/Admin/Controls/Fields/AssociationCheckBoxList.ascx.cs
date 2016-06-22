using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public partial class AssociationCheckBoxList : BaseFieldControl
    {
        public void Page_Init(object sender, EventArgs e)
        {
            BindItems();
        }

        private void BindItems()
        {
            var parentMediaDetailId = MediaDetailsMapper.GetByID(ParentMediaDetailID);

            if (parentMediaDetailId != null)
            {
                ItemsList.DataSource = MediaDetailsMapper.GetByID(ParentMediaDetailID).ChildMediaDetails.Where(i => i.CanRender && i.ShowInSiteTree);
                ItemsList.DataTextField = "SectionTitle";
                ItemsList.DataValueField = "ID";
                ItemsList.DataBind();
            }
        }

        private void SetSelectedIds(string values)
        {
            var selectedIds = StringHelper.JsonToObject<object[]>(values);

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
            var obj = StringHelper.JsonToObject<List<long>>(values);
            var field = GetField();

            var newIds = obj.ToList().ToJSON(2);
            var oldIds = field.FieldAssociations.Select(i => i.AssociatedMediaDetailID).ToList().ToJSON(2);

            if (newIds == oldIds)
                return;

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

            if (IsPostBack && !BasePage.IsAjaxRequest)
            {
                SaveToDB(values.ToString());
            }

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