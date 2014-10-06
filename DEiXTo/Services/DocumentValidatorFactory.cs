using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class DocumentValidatorFactory
    {
        public IDocumentValidator createValidator(string url)
        {
            if (url.Contains("www"))
            {
                return new WebDocumentValidator(url);
            }
            else if (url.Contains("file"))
            {
                return new LocalDocumentValidator(url);
            }

            throw new ArgumentException("No Validator found for this URL");
        }
    }
}
