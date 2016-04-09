namespace FrameworkLibrary
{
    public class Return
    {
        private object rawData = null;
        private Elmah.Error error = null;

        public Return(object rawData, Elmah.Error error)
        {
            this.rawData = rawData;
            this.error = error;
        }

        public Return()
        {
        }

        public T GetRawData<T>()
        {
            return (T)rawData;
        }

        public void SetRawData(object rawData)
        {
            this.rawData = rawData;
        }

        public bool IsError
        {
            get
            {
                if (Error == null || error.Exception == null)
                    return false;

                return Error.Exception != null;
            }
        }

        public Elmah.Error Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }
    }
}