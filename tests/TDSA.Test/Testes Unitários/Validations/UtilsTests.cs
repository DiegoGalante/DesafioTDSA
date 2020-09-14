using FluentAssertions;
using TDSA.Business.Validations;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Validations
{
    public class UtilsTests
    {
        [Trait("Utils", "Utils Testes")]
        [Theory(DisplayName = "Utils - ApenasNumeros - Deve Ser Valido")]
        [InlineData("123.456.789-09")]
        [InlineData("123.456.78909")]
        [InlineData("19000-000")]
        [InlineData("19000-000 ")]
        [InlineData(" 19000-000 ")]
        [InlineData("(18) 23432-1234")]
        [InlineData("123.456.789-09abc")]
        public void Utils_ApenasNumeros_DeveSerValido(string str)
        {
            var strTeste = Utils.ApenasNumeros(str);

            long numeros = 0;
            long.TryParse(strTeste, out numeros);

            strTeste.Should().Be(numeros.ToString());

        }

        [Trait("Utils", "Utils Testes")]
        [Theory(DisplayName = "Utils - ApenasNumeros - Deve Ser Invalido")]
        [InlineData("  ")]
        [InlineData("-")]
        [InlineData("a")]
        [InlineData(".")]
        public void Utils_ApenasNumeros_DeveSerInvalido(string str)
        {
            var strTeste = Utils.ApenasNumeros(str);

            strTeste.Should().BeEmpty();

        }

    }
}