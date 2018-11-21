using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Markdown
{
    public class MarkdownTokenizer
    {
        private readonly string markdownString;
        private int currentPosition;
        private readonly IEnumerable<TagType> availableTagTypes;

        public MarkdownTokenizer(string markdownString, IEnumerable<TagType> availableTagTypes)
        {
            this.markdownString = markdownString;
            this.availableTagTypes = availableTagTypes;
        }

        public IEnumerable<Token> GetTokens()
        {
            currentPosition = 0;
            var textTokenStart = currentPosition;

            while (currentPosition < markdownString.Length)
            {
                var tagToken = GetTagToken();
                if (tagToken != null)
                {
                    var possibleTextToken = new Token(textTokenStart,
                        markdownString.Substring(textTokenStart, currentPosition - textTokenStart));
                    if (textTokenStart < currentPosition)
                    {
                        yield return possibleTextToken;
                    }

                    yield return tagToken;

                    currentPosition += tagToken.Length;
                    textTokenStart = currentPosition;
                }
            }

            if (textTokenStart < currentPosition)
                yield return new Token(textTokenStart,
                    markdownString.Substring(textTokenStart, currentPosition - textTokenStart));
        }

        private Token GetTagToken()
        {
            var openTagTokenInfo = GetOpenTagInfo();
            if (openTagTokenInfo == null)
            {
                return null;
            }

            var tagType = openTagTokenInfo.Type;
            var closingTagTokenInfo = GetClosingTagTokenInfo(tagType);
            if (closingTagTokenInfo != null)
            {
                var tagContent = markdownString.Substring(currentPosition + tagType.Indicator.Length,
                    closingTagTokenInfo.OpeningIndex - currentPosition - tagType.Indicator.Length);
                var possibleTagToken = new Token(currentPosition, tagContent, tagType);
                if (!possibleTagToken.TagType.IsDigitLess 
                    || possibleTagToken.TagType.IsDigitLess && IsDigitLessToken(possibleTagToken))
                {
                    return possibleTagToken;
                }

                currentPosition += possibleTagToken.Length;
                return null;
            }

            currentPosition += tagType.Indicator.Length;

            return null;
        }

        private TokenInfo GetOpenTagInfo()
        {
            TagType currentTagType = null;
            for (; currentPosition < markdownString.Length; currentPosition++)
            {
                foreach (var tagType in availableTagTypes)
                {
                    currentTagType = ((IPairTag) tagType).IsOpeningTag(markdownString, currentPosition) ? tagType : currentTagType;
                }

                if (currentTagType != null)
                {
                    return new TokenInfo(currentPosition, currentPosition - currentTagType.Indicator.Length - 1,
                        currentTagType);
                }
            }

            return null;
        }

        private TokenInfo GetClosingTagTokenInfo(TagType openTagType)
        {
            var indicator = openTagType.Indicator;
            for (var pos = currentPosition + indicator.Length; pos < markdownString.Length - indicator.Length + 1; pos++)
            {
                if (((IPairTag) openTagType).IsClosingTag(markdownString, pos))
                {
                    return new TokenInfo(pos, pos + indicator.Length - 1, openTagType);
                }
            }

            return null;
        }

        private bool IsDigitLessToken(Token token)
        {
            return !token.Content.Any(char.IsDigit);
        }
    }
}