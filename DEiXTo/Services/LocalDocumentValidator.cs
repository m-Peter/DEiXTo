using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class LocalDocumentValidator : IDocumentValidator
    {
        private string _url;

        public LocalDocumentValidator(string url)
        {
            _url = url;
        }

        public bool IsValid()
        {
            try
            {
                FileWebRequest request = FileWebRequest.Create(_url) as FileWebRequest;
                request.Method = "HEAD";
                FileWebResponse response = request.GetResponse() as FileWebResponse;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
