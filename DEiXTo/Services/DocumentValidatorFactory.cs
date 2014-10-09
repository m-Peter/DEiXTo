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

                if (uri.IsFile)
                {
                    if (address.StartsWith("file:///"))
                    {
                        return new LocalDocumentValidator(uri);

                    }
                    else
                    {
                        return new LocalDocumentValidator(new Uri("file:///" + address));
                    }
                }
                else
                {
                    return new WebDocumentValidator(uri);
                }
            }
            catch (UriFormatException)
            {
                Uri uri;

                if (address.StartsWith("www"))
                {
                    uri = new Uri("http://" + address);
                }
                else
                {
                    uri = new Uri("http://www." + address);
                }

                return new WebDocumentValidator(uri);
            }

            throw new ArgumentException("No Validator found for this URL");
        }
    }
}
