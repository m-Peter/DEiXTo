using DEiXTo.Models;
using mshtml;

namespace DEiXTo.Services
{
    public class ImageAttributeExtractor : AttributeExtractor
    {
        public ImageAttributeExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override AttributeCollection Attributes()
        {
            var attributes = base.Attributes();
            attributes.Add(Src);
            attributes.Add(Alt);

            return attributes;
        }

        private TagAttribute Src
        {
            get
            {
                var src = _element.getAttribute("src");

                if (string.IsNullOrWhiteSpace(src))
                {
                    return null;
                }

                var tag = new TagAttribute { Name = "src", Value = src };

                return tag;
            }
        }

        private TagAttribute Alt
        {
            get
            {
                var alt = _element.getAttribute("alt");

                if (string.IsNullOrWhiteSpace(alt))
                {
                    return null;
                }

                var tag = new TagAttribute { Name = "alt", Value = alt };

                return tag;
            }
        }
    }
}
