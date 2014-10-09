using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class LinkContentExtractor : TagContentExtractor
    {
        private IHTMLElement _element;

        public LinkContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractContent()
        {
            return _element.getAttribute("href");
        }
    }
}
