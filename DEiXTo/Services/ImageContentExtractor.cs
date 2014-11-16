using mshtml;

namespace DEiXTo.Services
{
    public class ImageContentExtractor : TagContentExtractor
    {
        public ImageContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractContent()
        {
            return _element.getAttribute("src");
        }
    }
}
