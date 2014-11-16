using mshtml;

namespace DEiXTo.Services
{
    public class ContentExtractionFactory
    {
        public static TagContentExtractor GetExtractorFor(IHTMLElement element)
        {
            string tagName = element.tagName;

            switch (tagName)
            {
                case "A":
                    return new LinkContentExtractor(element);
                case "IMG":
                    return new ImageContentExtractor(element);
                case "FORM":
                    return new FormContentExtractor(element);
                case "INPUT":
                    return new InputContentExtractor(element);
                default:
                    return new GenericContentExtractor(element);
            }
        }
    }
}
