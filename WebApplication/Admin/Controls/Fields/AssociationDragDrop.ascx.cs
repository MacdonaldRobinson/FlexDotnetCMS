using FrameworkLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Admin.Controls.Fields
{
    public class JsonObj
    {
        public string name { get; set; }
        public long id { get; set; }
    }

    public partial class AssociationDragDrop : BaseFieldControl
    {
        public override void RenderControlInFrontEnd()
        {
            base.RenderControlInFrontEnd();
        }

        public override void RenderControlInAdmin()
        {
            base.RenderControlInAdmin();
        }

        private void SaveToDB(string value)
        {
            var obj = StringHelper.JsonToObject<List<JsonObj>>(value);
            var field = GetField();

            field.FieldAssociations.Clear();

            var orderIndex = 0;
            foreach (var item in obj)
            {
                var mediaDetail = (MediaDetail)MediaDetailsMapper.GetByID(item.id);

                if (mediaDetail != null)
                {
                    field.FieldAssociations.Add(new FieldAssociation() { MediaDetail = mediaDetail, MediaDetailField = field, OrderIndex = orderIndex });
                    orderIndex++;
                }
            }

            var returnObj = BaseMapper.GetDataModel().SaveChanges();
        }

        public override void SetValue(object value)
        {
            if (value == "")
                return;

            if (IsPostBack)
            {
                SaveToDB(value.ToString());
            }            

            Values.Value = value.ToString();
        }

        public override object GetValue()
        {
            if (Values.Value == "")
            {
                var field = GetField();

                var jsonObjList = new List<JsonObj>();

                foreach (var item in field.FieldAssociations)
                {
                    var jsonObj = new JsonObj();
                    jsonObj.name = item.MediaDetail.Title;
                    jsonObj.id = item.MediaDetail.ID;

                    jsonObjList.Add(jsonObj);
                }

                var jsonString = StringHelper.ObjectToJson(jsonObjList);

                return jsonString;
            }
            else
            {
                return Values.Value;
            }
        }
    }
}