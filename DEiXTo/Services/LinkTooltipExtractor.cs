using mshtml;

namespace DEiXTo.Services
{
    public class LinkTooltipExtractor : TagTooltipExtractor
    {
        public LinkTooltipExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            return _element.getAttribute("href");
        }
    }
}
