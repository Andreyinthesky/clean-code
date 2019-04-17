using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class String_Should
    {
        [TestCase("", 0, TestName = "string is empty")]
        [TestCase("word", -1, TestName = "index out of range left")]
        [TestCase("word", 4, TestName = "index out of range right")]
        public void TryGetCharAt_WhenIndexOutOfRange_ReturnFalse(string str, int index)
        {
            str.TryGetCharAt(index, out var character).Should().BeFalse();
        }

        [Test]
        public void TryGetCharAt_WhenIndexInRange_ReturnTrue()
        {
            var str = "word123";
            for (var i = 0; i < str.Length; i++)
            {
                str.TryGetCharAt(i, out var character).Should().BeTrue();
                character.Should().Be(str[i]);
            }
        }

        [TestCase("", ExpectedResult = "", TestName = "string is empty")]
        [TestCase("\\", ExpectedResult = "", TestName = "string is one escape")]
        [TestCase("\\\\", ExpectedResult = "\\", TestName = "string is two escapes")]
        public string RemoveEscapes_ShouldBeCorrect(string text)
        {
            return text.RemoveEscapes();
        }

        [TestCase("word", "word1")]
        [TestCase("word", "greater_word")]
        public void IsSubstringStartsWith_WhenSubstringLengthGreaterThanTextLength_ReturnFalse(string text, string substring)
        {
            text.IsSubstringStartsWith(substring, 0).Should().BeFalse();
        }

        [TestCase("word")]
        [TestCase("")]
        public void IsSubstringStartsWith_WhenIndexLessThanZero_ReturnFalse(string text)
        {
            text.IsSubstringStartsWith(string.Empty, -1).Should().BeFalse();
        }

        [Test]
        public void IsSubstringStartsWith_WhenIndexAndSubstringLengthSumOutOfRange_ReturnFalse()
        {
            var text = "word1";
            var substring = "word";
            for (var index = text.Length - substring.Length; index < text.Length; index++)
            {
                text.IsSubstringStartsWith(substring, index).Should().BeFalse();
            }
        }

        [Test]
        public void IsSubstringStartsWith_WhenSubstringDoNotStartWithIndex_ReturnFalse()
        {
            var text = "greater_word";
            var substring = "word";
            for (var index = 0; index < text.IndexOf(substring, StringComparison.OrdinalIgnoreCase); index++)
            {
                text.IsSubstringStartsWith(substring, index).Should().BeFalse();
            }
        }

        [Test]
        public void IsSubstringStartsWith_WhenSubstringStartWithIndex_ReturnTrue()
        {
            var text = "word_word word another word";
            var substring = "word";
            var index = 0;
            while (true)
            {
                var indexMatch = text.IndexOf(substring, index, StringComparison.OrdinalIgnoreCase);
                if (indexMatch == - 1)
                    break;
                text.IsSubstringStartsWith(substring, indexMatch).Should().BeTrue();
                index++;
            }
        }

        [TestCase("", 0)]
        [TestCase("word", -1)]
        [TestCase("word", 4)]
        public void IsEscapedCharAt_WhenIndexOutOfRange_ReturnFalse(string text, int index)
        {
            text.IsEscapedCharAt(index).Should().BeFalse();
        }

        [TestCase("word")]
        [TestCase(@"word\")]
        public void IsEscapedCharAt_WhenCharAtIndexIsNotEscaped_ReturnFalse(string text)
        {
            for (var index = 0; index < text.Length; index++)
            {
                text.IsEscapedCharAt(index).Should().BeFalse();
            }
        }

        [TestCase(@"\w", 1)]
        [TestCase(@"w\ord", 2)]
        [TestCase(@"wo\rd", 3)]
        [TestCase(@"wor\d", 4)]
        public void IsEscapedCharAt_WhenCharAtIndexIsEscaped_ReturnTrue(string text, int index)
        {
            text.IsEscapedCharAt(index).Should().BeTrue();
        }
    }
}