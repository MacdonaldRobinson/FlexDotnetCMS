using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public static class MediaExtentions
    {
        public static IEnumerable<IMediaDetail> GetLiveMediaDetails(this IEnumerable<Media> items)
        {
            return items.Select(i => i.MediaDetails.SingleOrDefault(j => j.HistoryForMediaDetail == null));
        }
    }
}
