using mshtml;

namespace DEiXTo.Services
{
    public abstract class TagTooltipExtractor
    {
        protected IHTMLElement _element;
        public abstract string ExtractTooltip();
    }
}
