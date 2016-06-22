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

            if (mediaDetail != null)
            {
                foreach (var item in mediaDetail.ChildMediaDetails)
                {
                    autoSuggestList.Add(new AutoSuggest() { name = item.SectionTitle, value = item.ID.ToString() });
                }
            }

            return StringHelper.ObjectToJson(autoSuggestList);
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
            var ids = StringHelper.JsonToObject<List<long>>(values);
            var field = GetField();

            var newIds = ids.ToList().ToJSON(2);
            var oldIds = field.FieldAssociations.Select(i => i.AssociatedMediaDetailID).ToList().ToJSON(2);

            if (newIds == oldIds)
                return;

            field.FieldAssociations.Clear();

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

            if (IsPostBack && !BasePage.IsAjaxRequest)
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