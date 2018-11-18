using System;

namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var md = new Md();

            Console.WriteLine(md.Render("_one_"));
            Console.WriteLine(md.Render("__one__"));
            Console.WriteLine(md.Render("__italic _inside_ bold__"));
            Console.WriteLine(md.Render("_bold __inside__ italic_"));
        }
    }
}