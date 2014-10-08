using System;
using System.Net;

namespace DEiXTo.Services
{
    public class WebDocumentValidator : IDocumentValidator
    {
        private Uri _uri;

        public WebDocumentValidator(Uri uri)
        {
            _uri = uri;
        }

        public bool IsValid()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
    }
}
