using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("", ExpectedResult = "", TestName = "text is empty string")]
        [TestCase("one", ExpectedResult = "one", TestName = "text has one word without underlines")]
        [TestCase("one two", ExpectedResult = "one two", TestName = "text has two words without underlines")]
        [TestCase("one_two", ExpectedResult = "one_two", TestName = "text has two words separated single underline")]
        [TestCase("one__two", ExpectedResult = "one__two", TestName = "text has two words separated double underline")]
        [TestCase("one~two", ExpectedResult = "one~two", TestName = "text has two words separated single tilde")]
        [TestCase("one~~two", ExpectedResult = "one~~two", TestName = "text has two words separated double tilde")]

        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "one word edged single underlines")]
        [TestCase("__bold__", ExpectedResult = "<strong>bold</strong>", TestName = "one word edged double underlines")]
        [TestCase("~~a~~", ExpectedResult = "<del>a</del>", TestName = "one word edged double tilde")]
        [TestCase("~~_a_~~", ExpectedResult = "<del><em>a</em></del>",
            TestName = "one word edged double tilde and inner edged single underline")]
        [TestCase("~~__a__~~", ExpectedResult = "<del><strong>a</strong></del>",
            TestName = "one word edged double tilde and inner edged double underline")]
        [TestCase("___boldAndItalic___", ExpectedResult = "___boldAndItalic___", TestName = "word edged single and double underline")]
        [TestCase("~~~tripleTildes~~~", ExpectedResult = "~~~tripleTildes~~~", TestName = "word edged single and double tilde")]
        [TestCase("____tooMuch____", ExpectedResult = "____tooMuch____", TestName = "word edged too much underline")]
        [TestCase("~~~~tooMuch~~~~", ExpectedResult = "~~~~tooMuch~~~~", TestName = "word edged too much tildes")]

        [TestCase("_one", ExpectedResult = "_one", TestName = "text starts with single underline")]
        [TestCase("one_", ExpectedResult = "one_", TestName = "text end with single underline")]
        [TestCase("__one", ExpectedResult = "__one", TestName = "text starts with double underline")]
        [TestCase("one__", ExpectedResult = "one__", TestName = "text end with double underline")]
        [TestCase("___one", ExpectedResult = "___one", TestName = "text starts with single and double underline")]
        [TestCase("one___", ExpectedResult = "one___", TestName = "text end with single and double underline")]
        [TestCase("~~one", ExpectedResult = "~~one", TestName = "text starts with double tilde")]
        [TestCase("one~~", ExpectedResult = "one~~", TestName = "text end with double tilde")]

        [TestCase("_unpair__", ExpectedResult = "_unpair__",
            TestName = "text starts with single underline, end with double underline")]
        [TestCase("__unpair_", ExpectedResult = "__unpair_",
            TestName = "text starts with double underline, end with single underline")]

        [TestCase(@"\", ExpectedResult = "", TestName = "text is back slash")]
        [TestCase(@"\\", ExpectedResult = @"\", TestName = "text is double back slash")]
        [TestCase(@"\_one\_", ExpectedResult = "_one_",
            TestName = "text edged escaped single underlines")]
        [TestCase(@"\_one_", ExpectedResult = "_one_",
            TestName = "text edged single underlines and opening underline escaped")]
        [TestCase(@"_one\_", ExpectedResult = "_one_",
            TestName = "text edged single underlines and closing underline escaped")]
        [TestCase(@"\\_a_", ExpectedResult = @"\<em>a</em>",
            TestName = "word edged single underlines and escape char before opening underline")]
        [TestCase(@"_a\\_", ExpectedResult = @"<em>a\</em>",
            TestName = "word edged single underlines and escape char before closing underline")]
        [TestCase(@"\\_a\\_", ExpectedResult = @"\<em>a\</em>",
            TestName = "word edged single underlines and escape char before opening and closing underline")]
        [TestCase(@"\\\\_a_", ExpectedResult = @"\\<em>a</em>", 
            TestName = "word edged single underlines and double escape char before opening underline")]
        [TestCase(@"_a\\\\_", ExpectedResult = @"<em>a\\</em>",
            TestName = "word edged single underlines and double escape char before closing underline")]
        [TestCase(@"\\\\_a\\\\_", ExpectedResult = @"\\<em>a\\</em>", 
            TestName = "word edged single underlines and double escape char before opening and closing underline")]
        [TestCase(@"\\__a__", ExpectedResult = @"\<strong>a</strong>", 
            TestName = "word edged double underlines and double escape char before opening underline")]
        [TestCase(@"__a\\__", ExpectedResult = @"<strong>a\</strong>", 
            TestName = "word edged double underlines and double escape char before closing underline")]
        [TestCase(@"\\__a\\__", ExpectedResult = @"\<strong>a\</strong>", 
            TestName = "word edged double underlines and double escape char before opening and closing underline")]

        [TestCase(@"\~~one\~~", ExpectedResult = "~~one~~",
            TestName = "text edged escaped double tilde")]
        [TestCase(@"\~~one~~", ExpectedResult = "~~one~~",
            TestName = "text edged double tilde and opening tilde escaped")]
        [TestCase(@"~~one\~~", ExpectedResult = "~~one~~",
            TestName = "text edged double tilde and closing tilde escaped")]
        [TestCase(@"\\~~a~~", ExpectedResult = @"\<del>a</del>",
            TestName = "word edged double tilde and escape char before opening tilde")]
        [TestCase(@"~~a\\~~", ExpectedResult = @"<del>a\</del>",
            TestName = "word edged double tilde and escape char before closing tilde")]
        [TestCase(@"\\~~a\\~~", ExpectedResult = @"\<del>a\</del>",
            TestName = "word edged double tilde and escape char before opening and closing tilde")]

        [TestCase(@"_\ a_", ExpectedResult = @"<em> a</em>",
            TestName = "word edged single underline but try escape whitespace before word")]
        [TestCase(@"_a\ _", ExpectedResult = @"_a _" , 
            TestName = "word edged single underline but try escape whitespace after word")]
        [TestCase(@"_\ a\ _", ExpectedResult = @"_ a _", 
            TestName = "word edged single underline but try escape whitespace before and after word")]
        [TestCase(@"__\ a__", ExpectedResult = @"<strong> a</strong>",
            TestName = "word edged double underline but try escape whitespace before word")]
        [TestCase(@"__a\ __", ExpectedResult = @"__a __", 
            TestName = "word edged double underline but try escape whitespace after word")]
        [TestCase(@"__\ a\ __", ExpectedResult = @"__ a __",
            TestName = "word edged double underline but try escape whitespace before and after word")]

        [TestCase("italic _inside_ text", ExpectedResult = "italic <em>inside</em> text",
            TestName = "text with cursive inside")]
        [TestCase("bold __inside__ text", ExpectedResult = "bold <strong>inside</strong> text",
            TestName = "text with bold inside")]
        [TestCase("del ~~inside~~ text", ExpectedResult = "del <del>inside</del> text",
            TestName = "text with del inside")]
        [TestCase("__italic _inside_ bold__", ExpectedResult = "<strong>italic <em>inside</em> bold</strong>",
            TestName = "text with cursive inside bold")]
        [TestCase("~~italic _inside_ del~~", ExpectedResult = "<del>italic <em>inside</em> del</del>",
            TestName = "text with cursive inside del")]
        [TestCase("_italic _inside_ italic_", ExpectedResult = "<em>italic _inside</em> italic_",
            TestName = "text with cursive inside cursive")]
        [TestCase("_bold __inside__ italic_", ExpectedResult = "<em>bold __inside__ italic</em>",
            TestName = "text with bold inside italic")]
        [TestCase("~~bold __inside__ del~~", ExpectedResult = "<del>bold <strong>inside</strong> del</del>",
            TestName = "text with bold inside del")]
        [TestCase("__bold __inside__ bold__", ExpectedResult = "<strong>bold __inside</strong> bold__",
            TestName = "text with bold inside bold")]
        [TestCase("_del ~~inside~~ italic_", ExpectedResult = "<em>del ~~inside~~ italic</em>",
            TestName = "text with del inside italic")]
        [TestCase("__del ~~inside~~ bold__", ExpectedResult = "<strong>del ~~inside~~ bold</strong>",
            TestName = "text with del inside bold")]
        [TestCase("~~del ~~inside~~ del~~", ExpectedResult = "<del>del </del>inside<del> del</del>",
            TestName = "text with del inside del")]

        [TestCase("_123_", ExpectedResult = "_123_", TestName = "digits edged single underlines")]
        [TestCase("__123__", ExpectedResult = "__123__", TestName = "digits edged double underlines")]
        [TestCase("something _123_ else", ExpectedResult = "something _123_ else", TestName = "text with digits edged single underlines")]
        [TestCase("something __123__ else", ExpectedResult = "something __123__ else", TestName = "text with digits edged double underlines")]
        [TestCase("~~123~~", ExpectedResult = "<del>123</del>", TestName = "digits edged double tilde")]
        [TestCase("something ~~123~~ else", ExpectedResult = "something <del>123</del> else", TestName = "text with digits edged double tilde")]

        [TestCase("_ spaceStart_", ExpectedResult = "_ spaceStart_", TestName = "word edged single underline but whitespace before word")]
        [TestCase("_spaceEnd _", ExpectedResult = "_spaceEnd _", TestName = "word edged single underline but whitespace after word")]
        [TestCase("__ spaceStart__", ExpectedResult = "__ spaceStart__", TestName = "word edged double underline but whitespace before word")]
        [TestCase("__spaceEnd __", ExpectedResult = "__spaceEnd __", TestName = "word edged double underline but whitespace after word")]
        [TestCase("~~ spaceStart~~", ExpectedResult = "<del> spaceStart</del>", TestName = "word edged double tilde but whitespace before word")]
        [TestCase("~~spaceEnd ~~", ExpectedResult = "<del>spaceEnd </del>", TestName = "word edged double tilde but whitespace after word")]

        public string Render_WhenParagraphIsCorrect(string markdownText)
        {
            return md.Render(markdownText);
        }


        [Test]
        [TestCaseSource(nameof(Render_PerformanceTestCases))]
        [Timeout(1000)]
        public void Render_WhenParagraphIsTooMuch(string markdownText)
        {
            md.Render(markdownText);      
        }

        private static IEnumerable Render_PerformanceTestCases
        {
            get
            {
                var testCases = new List<TestCaseData>();
                try
                {
                    var projectPath = Directory.GetParent(TestContext.CurrentContext.TestDirectory).Parent.FullName;
                    var testCasesPath = Path.Combine(projectPath, "MarkdownTests\\PerformanceCases");
                    foreach (var fileName in Directory.GetFiles(testCasesPath))
                    {
                        var testCase = new TestCaseData(File.ReadAllText(fileName))
                            .SetName(Path.GetFileNameWithoutExtension(fileName));
                        testCases.Add(testCase);
                    }
                }
                catch (Exception e)
                {
                    TestContext.WriteLine(e);
                }
                return testCases;
            }
        }

        [Test]
        public void Render_WhenParagraphIsNull_ThrowsNullReferenceException()
        {
            Action act = () => md.Render(null);
            act.Should()
                .Throw<NullReferenceException>()
                .WithMessage("paragraph was null");
        }
    }
}