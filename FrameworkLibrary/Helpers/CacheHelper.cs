using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkLibrary
{
    public class CacheHelper
    {
        public static Return ClearAllCache()
        {
            try
            {
                ContextHelper.ClearAllMemoryCache();
                FileCacheHelper.ClearAllCache();
                RedisCacheHelper.ClearAllCache();

                var webserviceRequests = FrameworkLibrary.WebserviceRequestsMapper.GetAll();

                foreach (var item in webserviceRequests)
                {
                    var context = BaseMapper.GetObjectFromContext(item);

                    if (context != null)
                        BaseMapper.DeleteObjectFromContext(context);
                }

                var returnObj = BaseMapper.SaveDataModel();

                return BaseMapper.GenerateReturn();
            }
            catch (Exception ex)
            {
                return BaseMapper.GenerateReturn(ex);
            }
        }
    }
}
