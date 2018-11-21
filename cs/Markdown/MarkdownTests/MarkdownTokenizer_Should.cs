using System;
using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTokenizer_Should
    {
        private MarkdownTokenizer tokenizer;
        private static readonly List<TagType> availableTagTypes = new List<TagType>
        {
            new EmTag(),
            new StrongTag(),
            new DelTag()
        };

        public void InitTokenizer(string markdownString)
        {
            tokenizer = new MarkdownTokenizer(markdownString, availableTagTypes);
        }

        [Test]
        public void GetTokens_WhenStringIsEmpty_ShouldBeEmpty()
        {
            InitTokenizer(string.Empty);
            tokenizer.GetTokens().Should().BeEmpty();
        }

        [TestCase("word1", TestName = "text is one word")]
        [TestCase("word1 word2", TestName = "text is two words")]
        public void GetTokens_WhenStringIsWords_ShouldHaveOneTextToken(string markdownString)
        {
            InitTokenizer(markdownString);
            var tokens = tokenizer.GetTokens().ToList();
            tokens.Count.Should().Be(1);
            tokens[0].Content.Should().Be(markdownString);
        }

        [TestCaseSource(nameof(StringHasSingleTags_TestCases))]
        public void GetTokens_WhenStringHasSingleTags_ShouldHaveSameTagTokens(string markdownString,
            List<Token> expectedTokens)
        {
            InitTokenizer(markdownString);
            var tokens = tokenizer.GetTokens().ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [TestCase("_word __another__ and another_", ExpectedResult = false,
            TestName = "text has strong tag inside em tag")]
        [TestCase("__word _another_ and another__", ExpectedResult = true,
            TestName = "text has em tag inside strong tag")]
        [TestCase("~~word _another_ and another~~", ExpectedResult = true,
            TestName = "text has em tag inside del tag")]
        [TestCase("~~word __another__ and another~~", ExpectedResult = true,
            TestName = "text has strong tag inside del tag")]
        public bool GetTokens_WhenStringHasInnerTags_ShouldBeCorrect(string markdownString)
        {
            InitTokenizer(markdownString);
            var tokens = tokenizer.GetTokens().ToList();
            return tokens.Count == 1
                   && tokens[0].TagType.AvailableInnerTagTypes.Any();
        }

        private static IEnumerable StringHasSingleTags_TestCases
        {
            get
            {
                var random = new Random();
                for (var i = 0; i < 10; i++)
                {
                    var expectedTokens = new List<Token>();
                    var markdownText = string.Empty;
                    var currentPosition = 0;
                    for (var j = 0; j < random.Next(1, 25); j++)
                    {
                        var tagType = availableTagTypes[random.Next(availableTagTypes.Count)];
                        var tagContent = "word" + char.ConvertFromUtf32('A' + j);
                        var markdownString = $"{tagType.Indicator}{tagContent}{tagType.Indicator}";
                        expectedTokens.Add(new Token(currentPosition, tagContent, tagType));
                        markdownText += markdownString;
                        currentPosition += markdownString.Length;
                        expectedTokens.Add(new Token(currentPosition, " "));
                        markdownText += " ";
                        currentPosition++;
                    }

                    yield return new TestCaseData(markdownText, expectedTokens);
                }
            }
        }
    }
}