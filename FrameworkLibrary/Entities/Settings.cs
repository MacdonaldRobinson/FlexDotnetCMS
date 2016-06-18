using System;

namespace FrameworkLibrary
{
    public partial class Settings : IMustContainID
    {
        public bool IsSiteOnline()
        {
            if (SiteOnlineAtDateTime == null)
                return true;

            if (DateTime.Now >= SiteOnlineAtDateTime && SiteOfflineAtDateTime == null)
                return true;

            if (DateTime.Now >= SiteOfflineAtDateTime)
                return false;

            if (SiteOfflineAtDateTime != null)
                return true;

            return false;
        }
    }
}