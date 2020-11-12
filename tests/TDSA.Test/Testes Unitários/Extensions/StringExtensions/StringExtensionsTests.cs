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



        [Trait("Extensions", "Extensions Testes")]
        [Theory(DisplayName = "StringExtension - EhVazio - Deve ser válido")]
        [InlineData("238.677.850-9")]
        [InlineData("238.677.850-900")]
        [InlineData("assfe-cx]  ")]
        [InlineData(" abc")]
        [InlineData("1-a")]
        public void Medico_EhVazio_CpfDeveSerValido(string str)
        {
            var resultado = str.EhVazio();

            resultado.Should().BeFalse();
        }



        [Trait("Extensions", "Extensions Testes")]
        [Theory(DisplayName = "StringExtension - EhVazio - Deve ser inválido")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Medico_EhVazio_CpfDeveSerInvalido(string str)
        {
            var resultado = str.EhVazio();

            resultado.Should().BeTrue();
        }
    }
}
