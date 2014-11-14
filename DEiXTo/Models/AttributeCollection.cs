using System.Collections.Generic;

namespace DEiXTo.Models
{
    public class AttributeCollection
    {
        private List<TagAttribute> _attributes;

        public AttributeCollection()
        {
            _attributes = new List<TagAttribute>();
        }

        public List<TagAttribute> All
        {
            get { return _attributes; }
        }

        public void Add(TagAttribute tagAttribute)
        {
            if (tagAttribute == null)
            {
                return;
            }

            _attributes.Add(tagAttribute);
        }

        public TagAttribute GetByName(string attributeName)
        {
            return _attributes.Find(attribute => attribute.Name == attributeName);
        }
    }
}
