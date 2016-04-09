using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Controls.Widgets
{
    public partial class Instagram : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            var webRequestHelper = new FrameworkLibrary.WebRequestHelper();
            webRequestHelper.EnableCaching = true;
            webRequestHelper.CacheDurationInSeconds = ((60 * 60) * 24);

            var returnJson = "";

            if (string.IsNullOrEmpty(UserID))
            {
                returnJson = webRequestHelper.MakeWebRequest("https://api.instagram.com/v1/tags/" + TagName + "/media/recent?client_id=" + AppSettings.InstagramClientID + "&count=" + Count);
            }
            else
            {
                returnJson = webRequestHelper.MakeWebRequest("https://api.instagram.com/v1/users/" + UserID + "/media/recent/?client_id=" + AppSettings.InstagramClientID + "&count=" + Count);
            }

            var obj = (new JavaScriptSerializer()).Deserialize<Dictionary<string, object>>(returnJson);

            if (obj.ContainsKey("data"))
            {
                var data = ((ArrayList)obj["data"]);
                data = ShuffleList(data);

                var startIndexCollection = new System.Collections.ArrayList();

                if ((StartIndex < EndIndex) && (EndIndex < Count))
                {
                    for (var index = StartIndex; index < EndIndex; index++)
                    {
                        var width = ((dynamic)data[index])["images"]["standard_resolution"]["width"];
                        var height = ((dynamic)data[index])["images"]["standard_resolution"]["height"];

                        if ((width == 640) && (height == 640))
                        {
                            startIndexCollection.Add(data[index]);
                        }
                        else
                        {
                            EndIndex++;
                        }
                    }
                }

                InstagramList.DataSource = startIndexCollection;
                InstagramList.DataBind();
            }
        }

        private ArrayList ShuffleList(ArrayList inputList)
        {
            var randomList = new ArrayList();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        protected void InstagramList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.DataItem == null)
                return;

            var dataItem = (dynamic)e.Item.DataItem;
            var imageUrl = dataItem["images"]["standard_resolution"]["url"];
            var InstagramImage = (Image)e.Item.FindControl("InstagramImage");

            InstagramImage.ImageUrl = imageUrl;
        }

        public string TagName { get; set; }

        public string UserID { get; set; }

        public int Count { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}