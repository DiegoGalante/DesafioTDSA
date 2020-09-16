using Bogus;
using FluentAssertions;
using Moq.AutoMock;
using System.Linq;
using TDSA.Business.Notificacoes;
using Xunit;

namespace TDSA.Test.Testes_Unitários.NotificacaoTests
{
    public class NotificadorTests
    {


        [Fact(DisplayName = "NotificarErro deve adicionar a Notificacao")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_NotificarErro_DeveSerValido()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var notificacao = new Notificacao(new Faker().Random.String(10, 'a', 'z'));

            //Act
            notificador.NotificarErro(notificacao);

            //Assert
            notificador.Notificacoes.Should().Contain(notificacao);
        }

        [Fact(DisplayName = "NotificarErro deve adicionar a Notificacao")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_NotificarErro_DeveSerValidoPorCampoEErro()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var campo = new Faker().Random.String(10, 'a', 'z');
            var erro = new Faker().Random.String(10, 'a', 'z');

            //Act
            notificador.NotificarErro(campo, erro);

            //Assert
            Assert.Contains(notificador.Notificacoes, x => x.Campo == campo && x.Erro == erro);
        }


        [Fact(DisplayName = "NotificarErro deve adicionar a Notificacao")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_NotificarErro_DeveSerValidoPorErro()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var erro = new Faker().Random.String(10, 'a', 'z');

            //Act
            notificador.NotificarErro(erro);

            //Assert
            Assert.Contains(notificador.Notificacoes, x => x.Erro == erro);
        }

        [Fact(DisplayName = "NotificarErro deve adicionar a Notificacao")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_NotificarErro_DeveSerValidoPorListaDeErros()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var erros = new Faker<string>("pt_BR")
                            .CustomInstantiator((f) => f.Random.String(10, 'a', 'z'))
                            .Generate(10);

            //Act
            notificador.NotificarErro(erros);

            //Assert
            Assert.True(notificador.Notificacoes.Where(x => erros.Contains(x.Erro)).Count() == erros.Count);
        }

        [Fact(DisplayName = "NotificarErro deve adicionar a Notificacao")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_NotificarErro_DeveSerValidoPorValidationResult()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var erros = new Faker<FluentValidation.Results.ValidationFailure>("pt_BR")
                            .CustomInstantiator((f) => new FluentValidation.Results.ValidationFailure(f.Random.String(10, 'a', 'z'), f.Random.String(15, 'a', 'z')))
                            .Generate(10);

            var validationResult = new FluentValidation.Results.ValidationResult(erros);

            //Act
            notificador.NotificarErros(validationResult);

            //Assert
            Assert.True(notificador.Notificacoes.Count() == validationResult.Errors.Count);
        }

       
    }
}
