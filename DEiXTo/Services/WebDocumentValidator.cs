﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class WebDocumentValidator : IDocumentValidator
    {
        private string _url;

        public WebDocumentValidator(string url)
        {
            _url = url;
        }

        public bool IsValid()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(_url) as HttpWebRequest;
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
