using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly List<TagType> availableTagTypes = new List<TagType>
        {
            new EmTag(),
            new StrongTag()
        };

        public Md()
        {
        }

        public string Render(string paragraph)
        {
            if (paragraph == null)
                throw new NullReferenceException("paragraph was null");
            return ConvertToHtmlString(paragraph, availableTagTypes).RemoveEscapes();
        }

        public string ConvertToHtmlString(string paragraph, List<TagType> tagTypes)
        {
            var result = new StringBuilder();
            var markdownTokenizer = new MarkdownTokenizer(paragraph, tagTypes);
            foreach (var token in markdownTokenizer.GetTokens())
            {
                string htmlTag;
                if (token.TokenType == TokenType.Tag)
                {
                    var innerTagTypes = availableTagTypes.Where(e => token.TagType.IsInAvailableInnerTagTypes(e)).ToList();
                    var tokenContent = innerTagTypes.Any()
                        ? ConvertToHtmlString(token.Content, innerTagTypes)
                        : token.Content;
                    htmlTag = token.TagType.ToHtml(tokenContent);
                }
                else
                    htmlTag = token.Content;

                result.Append(htmlTag);
            }

            return result.ToString();
        }
    }
}