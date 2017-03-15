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
            var parentMedia = MediasMapper.GetByID(ParentMediaID);
            IEnumerable<IMediaDetail> mediaDetailItems = new List<IMediaDetail>();

            if (parentMedia != null)
            {
                var liveMediaDetail = parentMedia.GetLiveMediaDetail();

                if (MediaTypeID > 0)
                {
                    mediaDetailItems = liveMediaDetail.ChildMediaDetails.Where(i => i.MediaTypeID == MediaTypeID && i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && !i.IsDeleted);
                }
                else
                {
                    mediaDetailItems = liveMediaDetail.ChildMediaDetails.Where(i => i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && !i.IsDeleted);
                }
            }

            else
            {
                if (MediaTypeID > 0)
                {
                    mediaDetailItems = BaseMapper.GetDataModel().MediaDetails.Where(i => i.MediaTypeID == MediaTypeID && i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && !i.IsDeleted);
                }
            }

            if (ShowInMenu != ShowStatus.Any)
            {
                mediaDetailItems = mediaDetailItems.Where(i => i.ShowInMenu == bool.Parse(ShowInMenu.ToString()));
            }

            ItemsList.DataSource = mediaDetailItems.ToList();
            ItemsList.DataTextField = "SectionTitle";
            ItemsList.DataValueField = "ID";
            ItemsList.DataBind();
        }

        private void SetSelectedIds(string values)
        {
            var selectedIds = StringHelper.JsonToObject<List<int>>(values);

            if (selectedIds != null)
            {
                foreach (ListItem item in ItemsList.Items)
                {
                    if (!string.IsNullOrEmpty(item.Value) && selectedIds.Contains(int.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        public ShowStatus ShowInMenu { get; set; } = ShowStatus.True;
        public long ParentMediaID { get; set; }
        public long MediaTypeID { get; set; }

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

            var newIds = obj.ToList().ToJson(2);
            var oldIds = field.FieldAssociations.Select(i => i.AssociatedMediaDetailID).ToList().ToJson(2);

            if (newIds == oldIds)
                return;

            var fieldAssociations = field.FieldAssociations.ToList();

            foreach (var item in fieldAssociations)
            {
                BaseMapper.DeleteObjectFromContext(item);
            }

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

            if (IsPostBack)
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
                if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    listIds.Add(long.Parse(item.Value));
            }

            var jsonString = StringHelper.ObjectToJson(listIds);

            return jsonString;
        }
    }
}