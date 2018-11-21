using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class TagType
    {
        public string Indicator { get; }
        public string HtmlTag { get; }
        public IEnumerable<TagType> AvailableInnerTagTypes { get; }
        public bool IsDigitLess { get; }

        protected TagType(string indicator, string htmlTag, bool isDigitLess, IEnumerable<TagType> availableInnerTagTypes)
        {
            Indicator = indicator;
            HtmlTag = htmlTag;
            AvailableInnerTagTypes = availableInnerTagTypes;
            IsDigitLess = isDigitLess;
        }

        public string ToHtml(string text) => $"<{HtmlTag}>{text}</{HtmlTag}>";
    }
}