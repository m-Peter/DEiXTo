using mshtml;

namespace DEiXTo.Services
{
    public class InputContentExtractor : TagContentExtractor
    {
        public InputContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractContent()
        {
            return _element.getAttribute("name");
        }
    }
}
