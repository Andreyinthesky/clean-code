namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public TokenType TokenType => TagType == null ? TokenType.Text : TokenType.Tag; 

        public TagType TagType { get; }

        public string Content { get; }

        public int Length => Content.Length + (TagType?.Indicator.Length * 2 ?? 0);

        public Token(int position, string content, TagType tagType = null)
        {
            Position = position;
            Content = content;
            TagType = tagType;
        }
    }
}