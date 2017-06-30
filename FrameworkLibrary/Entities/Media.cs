using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public partial class Media : IMustContainID
    {
        public IMediaDetail GetLiveMediaDetail(Language language = null)
        {
            if(language == null)
            {
                var currentLanguage = FrameworkSettings.GetCurrentLanguage();
                language = currentLanguage;
            }     
            
            return this.MediaDetails.SingleOrDefault(i => i.HistoryForMediaDetail == null && i.LanguageID == language.ID && ((i.MediaType != null)? i.MediaType.ShowInSiteTree : false));
        }

        public string GetTagsAsString()
        {
            return MediaTags.Select(i => i.Tag).Aggregate("", (current, item) => current + (item.Name + ","));
        }

        public Return MoveUp()
        {
            return MoveToIndex(GetIndex() - 1);
        }

        public Return MoveDown()
        {
            return MoveToIndex(GetIndex() + 1);
        }

        public Return MoveToIndex(int insertAtIndex)
        {
            var returnObj = BaseMapper.GenerateReturn();

            if (insertAtIndex < 0)
            {
                var ex = new System.Exception("Cant move to index: " + insertAtIndex);
                returnObj.Error = ErrorHelper.CreateError(ex);

                return returnObj;
            }

            var siblings = GetSiblings();
            var currentIndex = GetIndex(siblings);

            if (currentIndex >= 0 && currentIndex <= siblings.Count)
                siblings.RemoveAt(currentIndex);

            if (siblings.Count < insertAtIndex)
            {
                var ex = new System.Exception("Cant move to index: " + insertAtIndex);
                returnObj.Error = ErrorHelper.CreateError(ex);

                return returnObj;
            }

            siblings.Insert(insertAtIndex, this);

            var index = 0;

            foreach (var media in siblings)
            {
                if (media.OrderIndex == index)
                {
                    index++;
                    continue;
                }

                var inContext = BaseMapper.GetObjectFromContext(media);
                media.OrderIndex = index;
                returnObj = MediasMapper.Update(media);

                index++;
            }

            MediasMapper.ClearCache();
            MediaDetailsMapper.ClearCache();

            return returnObj;
        }

        public List<Media> GetSiblings()
        {
            var siblings = this.ParentMedia?.ChildMedias?.Where(i=>i.GetLiveMediaDetail() != null).OrderBy(i => i.OrderIndex).ToList();

            if (siblings == null)
                return new List<Media>();

            return siblings;
        }

        public int GetIndex()
        {
            var siblings = GetSiblings();
            return siblings.FindIndex(i => i.ID == this.ID);
        }

        public int GetIndex(List<Media> inList)
        {
            return inList.FindIndex(i => i.ID == this.ID);
        }

        public void ReorderChildren()
        {
            var index = 0;
            var childMedias = ChildMedias.Where(i=> i.MediaDetails.Any() && i.MediaDetails.ElementAt(0).MediaType.ShowInSiteTree).OrderBy(i => i.OrderIndex).ToList();
            foreach (var mediaItem in childMedias)
            {
                var context = BaseMapper.GetObjectFromContext(mediaItem);

                if (context == null)
                    continue;

                if (context.OrderIndex != index)
                {
                    context.OrderIndex = index;
                    MediasMapper.Update(context);
                }

                index++;
            }
        }

        public string PermaLink
        {
            get
            {
                return "/?MediaID=" + this.ID;
            }
        }

        public string PermaShortCodeLink
        {
            get
            {
                return "{Link:"+this.ID+"}";
            }
        }
    }
}