using System.Collections.Generic;

namespace Markdown
{
    public class StrongTag : TagType
    {
        public StrongTag() : base("__", "strong", new List<TagType> {new EmTag()})
        {
        }
    }
}