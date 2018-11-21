using System.Collections.Generic;

namespace Markdown
{
    public class EmTag : TagType
    {
        public EmTag() : base("_", "em", new List<TagType>())
        {
        }
    }
}