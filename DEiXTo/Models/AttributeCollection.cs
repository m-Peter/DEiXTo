using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _attributes.Add(tagAttribute);
        }

        public TagAttribute GetByName(string attributeName)
        {
            return _attributes.Find(attribute => attribute.Name == attributeName);
        }
    }
}
