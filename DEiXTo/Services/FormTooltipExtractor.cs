using mshtml;

namespace DEiXTo.Services
{
    public class FormTooltipExtractor : TagTooltipExtractor
    {
        public FormTooltipExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            return _element.getAttribute("name");
        }
    }
}
