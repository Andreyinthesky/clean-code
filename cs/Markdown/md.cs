using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public Md()
        {
        }

        public string Render(string paragraph)
        {
            if (paragraph == null)
                throw new NullReferenceException("paragraph was null");
            return ConvertToHtmlString(paragraph).RemoveEscapes();
        }

        public string ConvertToHtmlString(string paragraph)
        {
            var result = new StringBuilder();
            var markdownTokenizer = new MarkdownTokenizer(paragraph);
            foreach (var token in markdownTokenizer.GetTokens())
            {
                string htmlTag;
                if (token.TokenType == TokenType.Tag)
                {
                    var tokenContent = token.TagType.AvailableInnerTagTypes.Any()
                        ? ConvertToHtmlString(token.Content)
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