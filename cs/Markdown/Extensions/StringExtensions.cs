using System.Text;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool TryGetCharAt(this string text, int index, out char character)
        {
            character = '\0';
            if (index >= text.Length || index < 0)
                return false;
            character = text[index];
            return true;
        }

        public static bool IsEscapedCharAt(this string text, int startPosition)
        {
            if (!text.TryGetCharAt(startPosition, out var character))
                return false;
            var escapeCharRepeatCount = 0;
            while (text.TryGetCharAt(--startPosition, out character) && character == '\\')
            {
                escapeCharRepeatCount++;
            }

            return escapeCharRepeatCount % 2 != 0;
        }

        public static string RemoveEscapes(this string text)
        {
            var result = new StringBuilder();

            for (var index = 0; index < text.Length; index++)
            {
                if (text[index] == '\\')
                {
                    if (TryGetCharAt(text, index + 1, out var nextChar))
                    {
                        result.Append(nextChar);
                        index++;
                        continue;
                    }
                    break;
                }
                result.Append(text[index]);
            }

            return result.ToString();
        }

        public static bool IsSubstringStartsWith(this string text, string substring, int startPosition)
        {
            if (startPosition < 0 || startPosition + substring.Length > text.Length)
                return false;
            for (var i = 0; i < substring.Length; i++)
            {
                if (substring[i] != text[startPosition + i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}