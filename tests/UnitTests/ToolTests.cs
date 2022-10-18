using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Core;

namespace UnitTests
{
    public class ToolTests
    {
        [Theory]
        [InlineData('あ', 'ア')]
        [InlineData('き', 'キ')]
        public void Hiragana_char_changes_into_katakana(char input, char expectedResult)
        {
            char output = Tools.ToKatakana(input);
            Assert.Equal(expectedResult, output);
        }
        
        [Theory]
        [InlineData('A', '?')]
        public void Non_hiragana_char_doesnt_change_into_katakana(char input, char expectedResult)
        {
            char output = Tools.ToKatakana(input);
            Assert.Equal(expectedResult, output);
        }

        [Theory]
        [InlineData("あたま", "アタマ")]
        [InlineData("きょ", "キョ")]
        public void Hiragana_string_changes_into_katakana_string(string input, string expectedResult)
        {
            string output = Tools.ToKatakana(input);
            Assert.Equal(expectedResult, output);
        }

        [Theory]
        [InlineData("AAA", "???")]
        public void Non_hiragana_string_doesnt_change_into_Katakana_string(string input, string expectedResult)
        {
            string output = Tools.ToKatakana(input);
            Assert.Equal(expectedResult, output);
        }

        [Theory]
        [InlineData("あ", 0x3041, 0x3096)]
        [InlineData("あたま", 0x3041, 0x3096)]
        [InlineData("アタマ", 0x30A1, 0x30FE)]
        public void Char_are_inside_specified_range(string input, int startingRange, int endingRange)
        {
            bool result = Tools.GetCharsInRange(input, startingRange, endingRange);
            Assert.True(result);
        }

        [Theory]
        [InlineData("A", 0x3041, 0x3096)]
        public void Char_are_outside_specified_range(string input, int startingRange, int endingRange)
        {
            bool result = Tools.GetCharsInRange(input, startingRange, endingRange);
            Assert.False(result);
        }

        [Theory]
        [InlineData("あたま")]
        public void String_contains_only_hiragana_char(string input)
        {
            bool result = Tools.IsInHiragana(input);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("アタマ")]
        [InlineData("あタマ")]
        public void String_does_not_only_contain_hiragana_char(string input)
        {
            bool result = Tools.IsInHiragana(input);
            Assert.False(result);
        }
    }
}
