namespace Markdown
{
    public interface IPairTag
    {
        bool IsOpeningTag(string text, int startPosition);
        bool IsClosingTag(string text, int startPosition);
    }
}