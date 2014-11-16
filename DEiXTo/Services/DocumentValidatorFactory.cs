using System;

namespace DEiXTo.Services
{
    public class DocumentValidatorFactory
    {
        public IDocumentValidator CreateValidator(string address)
        {
            try
            {
                Uri uri = new Uri(address);

                if (!uri.IsFile)
                {
                    return new WebDocumentValidator(uri);
                }

                if (IsFileAddress(address))
                {
                    return new LocalDocumentValidator(uri);
                }

                return new LocalDocumentValidator(new Uri("file:///" + address));
            }
            catch (UriFormatException)
            {
                Uri uri;

                if (IsWebAddress(address))
                {
                    uri = new Uri("http://" + address);
                    return new WebDocumentValidator(uri);
                }

                uri = new Uri("http://www." + address);
                return new WebDocumentValidator(uri);
            }

            throw new ArgumentException("No Validator found for this URL");
        }

        private bool IsFileAddress(string address)
        {
            return address.StartsWith("file:///");
        }

        private bool IsWebAddress(string address)
        {
            return address.StartsWith("www");
        }
    }
}
