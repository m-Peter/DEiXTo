using mshtml;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TagContentExtractor
    {
        protected IHTMLElement _element;
        public abstract string ExtractContent();
    }
}
