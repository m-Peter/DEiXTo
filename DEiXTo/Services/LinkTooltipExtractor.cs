using mshtml;

namespace DEiXTo.Services
{
    public class LinkTooltipExtractor : TagTooltipExtractor
    {
        public LinkTooltipExtractor(IHTMLDOMNode element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            var elem = (IHTMLElement)_element;
            return elem.getAttribute("href");
        }
    }
}
