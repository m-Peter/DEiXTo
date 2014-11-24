using mshtml;

namespace DEiXTo.Services
{
    public class TooltipExtractionFactory
    {
        public static TagTooltipExtractor GetTooltipFor(IHTMLElement element)
        {
            string tagName = element.tagName;

            switch (tagName)
            {
                case "A":
                    return new LinkTooltipExtractor(element);
                case "IMG":
                    return new ImageTooltipExtractor(element);
                case "FORM":
                    return new FormTooltipExtractor(element);
                case "INPUT":
                    return new InputTooltipExtractor(element);
            }

            return new NullTooltipExtractor();
        }
    }
}
