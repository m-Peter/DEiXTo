using mshtml;

namespace DEiXTo.Services
{
    public class GenericContentExtractor : TagContentExtractor
    {
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
