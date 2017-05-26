using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public class AutoSuggest
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public partial class AssociationAutoSuggest : BaseFieldControl
    {
        public string GetDataJson()
        {
            var mediaDetail = MediaDetailsMapper.GetByID(ParentMediaDetailID);
            var autoSuggestList = new List<AutoSuggest>();
            IEnumerable<IMediaDetail> mediaDetailItems = new List<IMediaDetail>();

            if (mediaDetail != null)
            {                
                if (MediaTypeID > 0)
                {
                    mediaDetailItems = mediaDetail.ChildMediaDetails.Where(i => i.MediaTypeID == MediaTypeID && i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && !i.IsDeleted);
                }
                else
                {
                    mediaDetailItems = mediaDetail.ChildMediaDetails.Where(i=>i.HistoryVersionNumber == 0 && i.MediaType.ShowInSiteTree && !i.IsDeleted);
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

            autoSuggestList = GetAutoSuggestList(mediaDetailItems);

            return StringHelper.ObjectToJson(autoSuggestList);
        }

        private List<AutoSuggest> GetAutoSuggestList(IEnumerable<IMediaDetail> mediaDetailItems)
        {
            var autoSuggestList = new List<AutoSuggest>();

            foreach (var item in mediaDetailItems)
            {
                autoSuggestList.Add(new AutoSuggest() { name = item.SectionTitle, value = item.ID.ToString() });
            }

            return autoSuggestList;
        }

        public string GetCurrentAutoSuggestItems()
        {
            var field = GetField();
            var list = new List<AutoSuggest>();
            foreach (var assoiciatedMediaDetail in field.FieldAssociations)
            {
                list.Add(new AutoSuggest() { name = assoiciatedMediaDetail.MediaDetail.SectionTitle, value = assoiciatedMediaDetail.AssociatedMediaDetailID.ToString() });
            }

            var json = StringHelper.ObjectToJson(list);

            return json;
        }

        public ShowStatus ShowInMenu { get; set; } = ShowStatus.True;
        public long ParentMediaDetailID { get; set; }
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
            var ids = StringHelper.JsonToObject<List<long>>(values);

            if (ids == null)
                return;

            var field = GetField();

            var newIds = ids.ToList().ToJson(2);
            var oldIds = field.FieldAssociations.Select(i => i.AssociatedMediaDetailID).ToList().ToJson(2);

            if (newIds == oldIds)
                return;

            var fieldAssociations = field.FieldAssociations.ToList();

            foreach (var item in fieldAssociations)
            {
                BaseMapper.DeleteObjectFromContext(item);
            }            

            var orderIndex = 0;
            foreach (var id in ids)
            {
                var mediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(id);

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
            if ((values == "") || (!values.ToString().StartsWith("[")))
                return;

            if (IsPostBack)
            {
                SaveToDB(values.ToString());
            }

            var field = GetField();
            Values.Value = StringHelper.ObjectToJson(field.FieldAssociations.Select(i => i.AssociatedMediaDetailID));
        }

        public override object GetValue()
        {
            return Values.Value;
        }
    }
}