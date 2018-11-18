namespace Markdown
{
    public class TokenInfo
    {
        public int OpeningIndex { get; }
        public int ClosingIndex { get; }
        public int Length => ClosingIndex - OpeningIndex + 1;
        public TagType Type { get; }

        public TokenInfo(int openingIndex, int closingIndex, TagType tagType = null)
        {
            OpeningIndex = openingIndex;
            ClosingIndex = closingIndex;
            Type = tagType;
        }
    }
}