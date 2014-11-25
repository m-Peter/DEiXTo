using mshtml;

namespace DEiXTo.Services
{
    public class ImageTooltipExtractor : TagTooltipExtractor
    {
        public ImageTooltipExtractor(IHTMLDOMNode element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            var elem = (IHTMLElement)_element;
            return elem.getAttribute("src");
        }
    }
}
