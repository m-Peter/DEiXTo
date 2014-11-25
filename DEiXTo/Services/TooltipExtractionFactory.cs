using mshtml;

namespace DEiXTo.Services
{
    public class TooltipExtractionFactory
    {
        public static TagTooltipExtractor GetTooltipFor(IHTMLDOMNode element)
        {
            string tagName = element.nodeName;

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
                case "#text":
                    return new TextTooltipExtractor(element);
            }

            return new NullTooltipExtractor();
        }
    }
}
