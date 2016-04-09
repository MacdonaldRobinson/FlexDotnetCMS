using System.IO;

namespace FrameworkLibrary
{
    public partial class MasterPage : IMustContainID
    {
        public Return Validate()
        {
            var returnOnj = BaseMapper.GenerateReturn();

            return returnOnj;
        }

        public string GetMobileTemplate()
        {
            if (string.IsNullOrEmpty(this.MobileTemplate))
                return this.PathToFile;

            if (File.Exists(URIHelper.ConvertToAbsPath(this.MobileTemplate)))
                return this.MobileTemplate;

            return this.PathToFile;
        }
    }
}