﻿using mshtml;

namespace DEiXTo.Services
{
    public abstract class TagTooltipExtractor
    {
        protected IHTMLDOMNode _element;
        public abstract string ExtractTooltip();
    }
}
