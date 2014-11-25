using mshtml;

namespace DEiXTo.Services
{
    public class InputTooltipExtractor : TagTooltipExtractor
    {
        public InputTooltipExtractor(IHTMLDOMNode element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            var elem = (IHTMLElement)_element;
            return elem.getAttribute("name");
        }
    }
}
