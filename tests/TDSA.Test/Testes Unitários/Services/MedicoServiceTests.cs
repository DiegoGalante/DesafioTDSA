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
            var especialidadeRepo = new Mock<IEspecialidadeRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, especialidadeRepo.Object, mockNotificador.Object);
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
            var especialidadeRepo = new Mock<IEspecialidadeRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, especialidadeRepo.Object, mockNotificador.Object);
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = medicoService.ValidarMedico(medico);

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

        [Fact(DisplayName = "Medico Deve Retornar Guid Vazio")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Cadastrar_NaoDeveSerCadastrado()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();
            var especialidadeRepo = new Mock<IEspecialidadeRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, especialidadeRepo.Object, mockNotificador.Object);
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
