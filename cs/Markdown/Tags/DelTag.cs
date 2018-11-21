using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class DelTag : TagType, IPairTag
    {
        public DelTag() : base("~~", "del", false, new List<TagType>
            {
                new EmTag(),
                new StrongTag()
            }
        )
        {
        }

        public bool IsOpeningTag(string text, int startPosition)
        {
            return
                !(text.TryGetCharAt(startPosition - 1, out var previousChar)
                  && !(previousChar != this.Indicator.LastOrDefault() && previousChar != '\\'
                       || text.IsEscapedCharAt(startPosition - 1)))
                && text.TryGetCharAt(startPosition + this.Indicator.Length, out var nextChar)
                && nextChar != this.Indicator.FirstOrDefault()
                && text.IsSubstringStartsWith(this.Indicator, startPosition);
        }

        public bool IsClosingTag(string text, int startPosition)
        {
            return
                text.TryGetCharAt(startPosition - 1, out var previousChar)
                && (previousChar != this.Indicator.LastOrDefault() && previousChar != '\\'
                    || text.IsEscapedCharAt(startPosition - 1))
                && (!text.TryGetCharAt(startPosition + this.Indicator.Length, out var nextChar)
                    || nextChar != this.Indicator.FirstOrDefault())
                && text.IsSubstringStartsWith(this.Indicator, startPosition);
        }
    }
}