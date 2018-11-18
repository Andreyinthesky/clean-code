using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class TagType
    {
        public string Indicator { get; }
        public string HtmlTag { get; }
        private readonly IEnumerable<TagType> availableInnerTagTypes;

        protected TagType(string indicator, string htmlTag, IEnumerable<TagType> availableInnerTagTypes)
        {
            Indicator = indicator;
            HtmlTag = htmlTag;
            this.availableInnerTagTypes = availableInnerTagTypes;
        }

        public bool IsInAvailableInnerTagTypes(TagType tagType) =>
            availableInnerTagTypes?.Any(t => t.GetType() == tagType.GetType()) ?? false;

        public string ToHtml(string text) => $"<{HtmlTag}>{text}</{HtmlTag}>";
    }
}