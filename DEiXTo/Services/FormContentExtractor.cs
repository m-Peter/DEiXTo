using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class FormContentExtractor : TagContentExtractor
    {
        private IHTMLElement _element;

        public FormContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractContent()
        {
            return _element.getAttribute("name");
        }
    }
}
