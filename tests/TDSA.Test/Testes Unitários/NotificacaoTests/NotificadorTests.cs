using Bogus;
using FluentAssertions;
using Moq.AutoMock;
using System.Linq;
using TDSA.Business.Interfaces;
using TDSA.Business.Notificacoes;
using TDSA.Test.Testes_Unitários.Fixture;
using Xunit;

namespace TDSA.Test.Testes_Unitários.NotificacaoTests
{
    [Collection(nameof(NotificadorTestsFixtureCollection))]
    public class NotificadorTests
    {
        readonly NotificadorTestsFixture _notificacaoTestsFixture;

        public NotificadorTests(NotificadorTestsFixture notificacaoTestsFixture)
        {
            _notificacaoTestsFixture = notificacaoTestsFixture;
        }

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

        [Fact(DisplayName = "Deve Retornar Notificacoes")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_ObterNotificacoes_DeveRetornarNotificacoes()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            var erros = new Faker<string>("pt_BR")
                .CustomInstantiator((f) => f.Random.String(10, 'a', 'z'))
                .Generate(10);

            notificador.NotificarErro(erros);

            //Act
            var notificacoes = notificador.ObterNotificacoes();

            //Assert
            Assert.True(notificacoes.Any());
        }

        [Fact(DisplayName = "Deve Possuir Notificacoes")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_ObterNotificacoes_DevePossuirNotificacoes()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();
            var erros = new Faker<string>("pt_BR")
                .CustomInstantiator((f) => f.Random.String(10, 'a', 'z'))
                .Generate(10);

            notificador.NotificarErro(erros);

            //Act
            var result = notificador.TemNotificacao();

            //Assert
            Assert.True(result);
        }


        [Fact(DisplayName = "Não Deve Possuir Notificacoes")]
        [Trait("Services", "Notificador Testes")]
        public void MedicoService_ObterNotificacoes_NaoDevePossuirNotificacoes()
        {
            //Arrange
            var mocker = new AutoMocker();
            var notificador = mocker.CreateInstance<Notificador>();

            //Act
            var result = notificador.TemNotificacao();

            //Assert
            Assert.False(result);
        }


    }
}
