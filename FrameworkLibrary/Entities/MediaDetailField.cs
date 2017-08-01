using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace FrameworkLibrary
{
    public partial class MediaDetailField : IMustContainID, IField
    {
        public DirectoryInfo UploadFolder
        {
            get
            {
                var baseUploadFolder = "~/media/uploads/";

                return new DirectoryInfo(FrameworkLibrary.URIHelper.ConvertToAbsPath(baseUploadFolder + "fields/" + this.ID + "/"));
            }
        }
    }
}