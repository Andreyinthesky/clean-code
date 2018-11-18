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


        public static string RemoveEscapes(this string text)
        {
            return text.Replace("\\", "");
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