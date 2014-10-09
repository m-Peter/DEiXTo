﻿using System;
using System.IO;
using System.Net;

namespace DEiXTo.Services
{
    public class LocalDocumentValidator : IDocumentValidator
    {
        private Uri _uri;

        public LocalDocumentValidator(Uri uri)
        {
            _uri = uri;
        }

        public bool IsValid()
        {
            try
            {
                FileWebRequest request = FileWebRequest.Create(_uri.ToString()) as FileWebRequest;
                request.Method = "HEAD";
                FileWebResponse response = request.GetResponse() as FileWebResponse;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Url()
        {
            return _uri.OriginalString;
        }
    }
}
