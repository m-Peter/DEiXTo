using mshtml;

namespace DEiXTo.Services
{
    public abstract class TagContentExtractor
    {
        protected IHTMLElement _element;
        public abstract string ExtractContent();
    }
}
