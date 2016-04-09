namespace FrameworkLibrary
{
    public partial class Language : IMustContainID
    {
        public Return Validate()
        {
            var returnOnj = BaseMapper.GenerateReturn();

            return returnOnj;
        }
    }
}