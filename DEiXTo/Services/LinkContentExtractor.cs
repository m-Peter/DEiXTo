using mshtml;

namespace DEiXTo.Services
{
    public class LinkContentExtractor : TagContentExtractor
    {
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
