using mshtml;

namespace DEiXTo.Services
{
    public class InputTooltipExtractor : TagTooltipExtractor
    {
        public InputTooltipExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            return _element.getAttribute("name");
        }
    }
}
