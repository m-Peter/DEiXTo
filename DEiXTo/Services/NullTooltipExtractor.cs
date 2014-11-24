using mshtml;

namespace DEiXTo.Services
{
    public class NullTooltipExtractor : TagTooltipExtractor
    {
        public override string ExtractTooltip()
        {
            return string.Empty;
        }
    }
}
