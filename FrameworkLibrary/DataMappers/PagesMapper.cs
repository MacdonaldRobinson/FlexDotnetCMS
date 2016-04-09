using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class PagesMapper : MediaDetailsMapper
    {
        private static string enumName = MediaTypeEnum.Page.ToString();

        public new static IEnumerable<IMediaDetail> GetAll()
        {
            return MediaDetailsMapper.GetDataModel().MediaDetails.Where(i => i.MediaType.Name == enumName); //FilterByMediaType(MediaDetailsMapper.GetAll(), MediaTypeEnum.Page);
        }
    }
}