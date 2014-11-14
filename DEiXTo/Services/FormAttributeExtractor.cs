using DEiXTo.Models;
using mshtml;

namespace DEiXTo.Services
{
    public class FormAttributeExtractor : AttributeExtractor
    {
        public FormAttributeExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override AttributeCollection Attributes()
        {
            var attributes = base.Attributes();
            attributes.Add(Name);

            return attributes;
        }

        private TagAttribute Name
        {
            get
            {
                var name = _element.getAttribute("name");

                if (string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }

                var tag = new TagAttribute { Name = "name", Value = name };

                return tag;
            }
        }
    }
}
