using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace FrameworkLibrary
{
    public partial class MediaDetailField : IMustContainID, IField
    {
        public static DirectoryInfo GetUploadFolder(MediaDetailField field, string baseUploadFolder = "~/media/uploads/")
        {
            return new DirectoryInfo(FrameworkLibrary.URIHelper.ConvertToAbsPath(baseUploadFolder + "medias/"+ field.MediaDetail.MediaID + "/media-details/" + field.MediaDetail.ID + "/fields/" + field.ID + "/"));
        }

        public string GetUsageExample()
        {
            if (MediaDetail == null)
                return "Please save first";

            var usageExample = (!string.IsNullOrEmpty(UsageExample)) ? UsageExample : "{Field:" + FieldCode + "} OR {{Load:" + MediaDetail.MediaID + "}.Field:" + FieldCode + "}";

            return usageExample;
        }
    }
}