using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class TagType
    {
        public string Indicator { get; }
        public string HtmlTag { get; }
        public IEnumerable<TagType> AvailableInnerTagTypes { get; }

        protected TagType(string indicator, string htmlTag, IEnumerable<TagType> availableInnerTagTypes)
        {
            Indicator = indicator;
            HtmlTag = htmlTag;
            AvailableInnerTagTypes = availableInnerTagTypes;
        }

        public string ToHtml(string text) => $"<{HtmlTag}>{text}</{HtmlTag}>";
    }
}