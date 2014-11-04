using DEiXTo.Models;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class LinkAttributeExtractor : AttributeExtractor
    {
        public LinkAttributeExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override AttributeCollection Attributes()
        {
            var attributes = base.Attributes();
            attributes.Add(Href);

            return attributes;
        }

        private TagAttribute Href
        {
            get
            {
                var href = _element.getAttribute("href");

                if (string.IsNullOrWhiteSpace(href))
                {
                    return null;
                }

                var tag = new TagAttribute { Name = "href", Value = href };

                return tag;
            }
        }
    }
}
