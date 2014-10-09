using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class GenericContentExtractor : TagContentExtractor
    {
        private IHTMLElement _element;

        public GenericContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractContent()
        {
            return _element.innerText;
        }
    }
}
