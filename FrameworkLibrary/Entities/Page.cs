using System;

namespace FrameworkLibrary
{
    public partial class Page : IMediaDetail
    {
        public object ToLiquid()
        {
            return this;
        }

        public new Return Validate()
        {
            base.Validate();
            return GenerateValidationReturn();
        }
    }
}