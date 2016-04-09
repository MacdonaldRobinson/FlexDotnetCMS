using System;

namespace FrameworkLibrary
{
    public partial class Settings : IMustContainID
    {
        public bool IsSiteOnline()
        {
            if ((DateTime.Now >= SiteOnlineAtDateTime) && ((DateTime.Now < SiteOfflineAtDateTime) || (SiteOfflineAtDateTime == null)))
                return true;

            return false;
        }
    }
}