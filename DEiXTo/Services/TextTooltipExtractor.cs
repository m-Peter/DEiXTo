using mshtml;

namespace DEiXTo.Services
{
    public class TextTooltipExtractor : TagTooltipExtractor
    {
        public TextTooltipExtractor(IHTMLDOMNode element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            string value = _element.nodeValue as string;
            return value.Trim();
        }
    }
}
