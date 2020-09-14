using FluentAssertions;
using TDSA.Business.Extensions;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Extensions.StringExtensions
{
    public class StringExtensionsTests
    {
        [Trait("Extensions", "Extensions Testes")]
        [Theory(DisplayName = "StringExtension - FormataStringComLetrasMinusculas - Deve Ser Minuscula")]
        [InlineData("ABC")]
        [InlineData(" aBc")]
        [InlineData("abC ")]
        [InlineData("aBC 1")]
        [InlineData("1234 .")]
        public void StringExtension_FormataStringComLetrasMinusculas_DeveSerMinuscula(string str)
        {
            var strFormatado = str.FormataStringComLetrasMinusculas();

            var strTeste = str.Trim().ToLower();
            strFormatado.Should().BeEquivalentTo(strTeste);
        }

  


    }
}
