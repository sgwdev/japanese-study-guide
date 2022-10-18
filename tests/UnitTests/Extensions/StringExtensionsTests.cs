using Infrastructure.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("SnakeCaseTest", "snake_case_test")]
        [InlineData("Test", "test")]
        [InlineData("", "")]
        public void String_changes_from_pascal_case_to_snake_case(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.ToSnakeCase());
        }

        [Fact]
        public void null_to_snake_case_returns_null()
        {
            string s = null;
            Assert.Null(s.ToSnakeCase());
        }
    }
}
