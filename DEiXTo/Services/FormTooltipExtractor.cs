using mshtml;

namespace DEiXTo.Services
{
    public class FormTooltipExtractor : TagTooltipExtractor
    {
        public FormTooltipExtractor(IHTMLDOMNode element)
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
