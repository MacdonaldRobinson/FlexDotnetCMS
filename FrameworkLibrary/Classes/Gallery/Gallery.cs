using System.Collections.Generic;

namespace FrameworkLibrary
{
    public class Gallery
    {
        private List<Slide> slides = new List<Slide>();
        private string pathToRenderControl = null;

        public Gallery()
        {
        }

        public string PathToRenderControl
        {
            get
            {
                return pathToRenderControl;
            }
            set
            {
                pathToRenderControl = value;
            }
        }

        public List<Slide> Slides
        {
            get
            {
                return slides;
            }
        }
    }
}