using FluentAssertions;
using TDSA.Business.Validations;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Validations
{
    public class CpfValidationTests
    {
        [Trait("Validação", "Validação Cpf Testes")]
        [Theory(DisplayName = "CpfValidation - Validar - Deve Ser Valido")]
        [InlineData("652.472.120-96")]
        [InlineData("65247212096")]
        public void CpfValidation_Validar_DeveSerValido(string str)
        {
            var ehValido = CpfValidacao.Validar(str);

            ehValido.Should().BeTrue();
        }

        [Trait("Validação", "Validação Cpf Testes")]
        [Theory(DisplayName = "CpfValidation - Validar - Deve Ser Invalido")]
        [InlineData("")]
        [InlineData("abc_")]
        [InlineData("11111111111")]
        [InlineData("222-111-111-00")]
        public void CpfValidation_Validar_DeveSerInvalido(string str)
        {
            var ehValido = CpfValidacao.Validar(str);

            ehValido.Should().BeFalse();
        }

    }
}
