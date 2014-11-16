using mshtml;

namespace DEiXTo.Services
{
    public class FormContentExtractor : TagContentExtractor
    {
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
