using System;

namespace FrameworkLibrary
{
    public partial class Website
    {
        public bool IsSiteOnline()
        {
            var settings = FrameworkLibrary.SettingsMapper.GetSettings();

            if ((DateTime.Now >= settings.SiteOnlineAtDateTime) && ((DateTime.Now < settings.SiteOfflineAtDateTime) || (settings.SiteOfflineAtDateTime == null)))
                return true;

            return false;
        }
    }
}