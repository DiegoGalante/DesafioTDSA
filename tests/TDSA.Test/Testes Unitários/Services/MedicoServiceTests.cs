using Bogus;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Services;
using TDSA.Test.Testes_Unitários.Fixture;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Services
{
    [Collection(nameof(MedicoBogusCollection))]
    public class MedicoServiceTests
    {
        readonly MedicoTestsFixtureService _medicoServiceTestsFixture;

        public MedicoServiceTests(MedicoTestsFixtureService medicoServiceTestsFixture)
        {
            _medicoServiceTestsFixture = medicoServiceTestsFixture;
        }


        [Fact(DisplayName = "MedicoService - ValidarMedico - Deve Ser Valido")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarMedico_DeveSerValido()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var result = medicoService.ValidarMedico(medico);

            //Assert
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarMedico - Deve Ser Invalido")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarMedico_DeveSerInvalido()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = medicoService.ValidarMedico(medico);

            //Assert
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.AtLeastOnce);
            Assert.False(result);
        }


        [Fact(DisplayName = "MedicoService - ValidarEspecialidade - Deve Ser Válida")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarEspecialidade_DeveSerValido()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var especialidade = _medicoServiceTestsFixture.GerarEspecialidadeValida();

            //Act
            var result = medicoService.ValidarEspecialidade(especialidade);

            //Assert
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarEspecialidade - Deve Ser Inválida")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarEspecialidade_DeveSerInvalida()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var especialidade = _medicoServiceTestsFixture.GerarEspecialidadeInvalida();

            //Act
            var result = medicoService.ValidarEspecialidade(especialidade);

            //Assert
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.AtLeastOnce);
            Assert.False(result);
        }

        [Fact(DisplayName = "Medico Deve Ser Cadastrado")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Cadastrar_DeveSerCadastrado()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var result = await medicoService.Cadastrar(medico);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(r => r.Adicionar(medico), Times.Once);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.SaveChanges(), Times.Once);
            Assert.Equal(medico.Id, result);
        }

        [Fact(DisplayName = "MedicoService - ValidarCPFJaCadastrado - Deve Ser Verdadeiro")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarCPFJaCadastrado_DeveSerVerdadeiro()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var result = medicoService.ValidarCPFJaCadastrado(medico);

            //Assert
            medicoRepo.Verify(r => r.ObterPorCPF(It.IsAny<string>()), Times.Once);
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarCPFJaCadastrado - Deve Ser Falso")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarCPFJaCadastrado_DeveSerFalse()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            /*Preciso ver como faz essa validação*/
            var result = medicoService.ValidarCPFJaCadastrado(medico);

            //Assert
            medicoRepo.Verify(r => r.ObterPorCPF(It.IsAny<string>()), Times.Once);
            mockNotificador.Verify(r => r.NotificarErros(It.IsAny<FluentValidation.Results.ValidationResult>()), Times.Once);
            //Assert.False(result);

            /*FORÇANDO O ERRO PRA PERCEBER QUE PRECISO ARRUMAR*/
            Assert.True(result);
        }


        [Fact(DisplayName = "MedicoService - ValidarCadastro - Deve Ser Válido")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarCadastro_DeveSerValido()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var result = medicoService.ValidarCadastro(medico);

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarCadastro - Deve Ser Invalido")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarCadastro_DeveSerInvalido()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = medicoService.ValidarCadastro(medico);

            //Assert
            Assert.False(result);
        }


        [Fact(DisplayName = "MedicoService - ValidarAtualizacao - Deve Ser Verdadeiro")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarAtualizacao_DeveSerVerdadeiro()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var result = medicoService.ValidarAtualizacao(medico);

            //Assert
            Assert.True(result);
        }


        [Fact(DisplayName = "MedicoService - ValidarAtualizacao - Deve Ser Falso")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarAtualizacao_DeveSerFalso()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = medicoService.ValidarAtualizacao(medico);

            //Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Medico Deve Retornar Guid Vazio")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Cadastrar_NaoDeveSerCadastrado()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = await medicoService.Cadastrar(medico);

            //Assert
            medicoRepo.Verify(r => r.Adicionar(medico), Times.Never);
            medicoRepo.Verify(r => r.SaveChanges(), Times.Never);
            //result.Should().BeEmpty();
            Assert.Equal(Guid.Empty, result);
        }


    }
}
