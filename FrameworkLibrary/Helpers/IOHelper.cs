using System.IO;

namespace FrameworkLibrary
{
    public partial class IOHelper
    {
        public static FileInfo[] GetFiles(DirectoryInfo dirInfo, string pattern = "*.*")
        {
            return dirInfo.GetFiles(pattern);
        }
    }
}