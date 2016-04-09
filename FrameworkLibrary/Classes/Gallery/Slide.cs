namespace FrameworkLibrary
{
    public class Slide
    {
        private string pathToFile;
        private string pathToAlternativeFile;
        private string link;
        private string title;
        private string bgColor;
        private IMediaDetail mediaDetails = null;

        public Slide(string pathToFile, string pathToAlternativeFile, string title, string link, string bgColor)
        {
            this.pathToFile = pathToFile;
            this.pathToAlternativeFile = pathToAlternativeFile;
            this.link = link;
            this.title = title;
            this.bgColor = bgColor;
        }

        public Slide()
        {
        }

        public string BgColor
        {
            get
            {
                return bgColor;
            }
            set
            {
                bgColor = value;
            }
        }

        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public string PathToFile
        {
            get
            {
                return pathToFile;
            }
            set
            {
                pathToFile = value;
            }
        }

        public IMediaDetail MediaDetail
        {
            get
            {
                return mediaDetails;
            }
            set
            {
                mediaDetails = value;
            }
        }

        public string PathToAlternativeFile
        {
            get
            {
                return pathToAlternativeFile;
            }
            set
            {
                pathToAlternativeFile = value;
            }
        }
    }
}