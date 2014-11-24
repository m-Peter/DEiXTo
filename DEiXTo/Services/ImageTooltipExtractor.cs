using mshtml;

namespace DEiXTo.Services
{
    public class ImageTooltipExtractor : TagTooltipExtractor
    {
        public ImageTooltipExtractor(IHTMLElement element)
        {
            _element = element;
        }

        public override string ExtractTooltip()
        {
            return _element.getAttribute("src");
        }
    }
}
