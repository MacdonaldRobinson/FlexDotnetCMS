using System;
using System.Web;

namespace FrameworkLibrary
{
    public static class RequestToolGZipSimple
    {
        public static HttpWorkerRequest GetWorker(HttpContext context)
        {
            IServiceProvider provider = (IServiceProvider)context;
            HttpWorkerRequest worker = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            return worker;
        }

        public static bool HasSupport(HttpContext context)
        {
            try
            {
                HttpWorkerRequest worker = GetWorker(context);
                string value = worker.GetKnownRequestHeader(HttpWorkerRequest.HeaderAcceptEncoding);
                if (value == null)
                {
                    return false;
                }
                if (value.Length >= 4)
                {
                    if (value[0] == 'g' && value[1] == 'z' && value[2] == 'i' && value[3] == 'p')
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < value.Length - 3; i++)
                        {
                            if ((value[i] == 'g' || value[i] == 'G') &&
                                (value[i + 1] == 'z' || value[i + 1] == 'Z') &&
                                (value[i + 2] == 'i' || value[i + 2] == 'I') &&
                                (value[i + 3] == 'p' || value[i + 3] == 'P'))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}