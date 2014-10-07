using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class DocumentValidatorFactory
    {
        public IDocumentValidator createValidator(string address)
        {
            Uri uri;

            if (address.StartsWith("http"))
            {
                uri = new Uri(address);
            }
            else
            {
                uri = new Uri("http://" + address);
            }

            if (uri.IsFile)
            {
                return new LocalDocumentValidator(uri);
            }
            else
            {
                return new WebDocumentValidator(uri);
            }

            throw new ArgumentException("No Validator found for this URL");
        }
    }
}
