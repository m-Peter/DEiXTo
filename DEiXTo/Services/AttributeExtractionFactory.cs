using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class AttributeExtractionFactory
    {
        public static AttributeExtractor GetExtractorFor(IHTMLElement element)
        {
            string tagName = element.tagName;

            switch (tagName)
            {
                case "A":
                    return new LinkAttributeExtractor(element);
                case "IMG":
                    return new ImageAttributeExtractor(element);
                case "FORM":
                    return new FormAttributeExtractor(element);
                case "INPUT":
                    return new InputAttributeExtractor(element);
                default:
                    return new AttributeExtractor(element);
            }
        }
    }
}
