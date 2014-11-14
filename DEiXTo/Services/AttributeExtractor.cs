using DEiXTo.Models;
using mshtml;

namespace DEiXTo.Services
{
    public class AttributeExtractor
    {
        protected IHTMLElement _element;

        public AttributeExtractor()
        {

        }

        public AttributeExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public TagAttribute Id
        {
            get
            {
                var id = _element.getAttribute("id");

                if (string.IsNullOrWhiteSpace(id))
                {
                    return null;
                }

                var tag = new TagAttribute { Name = "id", Value = id };

                return tag;
            }
        }

        public TagAttribute Klass
        {
            get
            {
                var klass = _element.getAttribute("className");

                if (string.IsNullOrWhiteSpace(klass))
                {
                    return null;
                }
                
                var tag = new TagAttribute { Name = "class", Value = klass };

                return tag;
            }
        }

        public virtual TagAttributeCollection Attributes()
        {
            var attributes = new TagAttributeCollection();
            attributes.Add(Id);
            attributes.Add(Klass);

            return attributes;
        }
    }
}
