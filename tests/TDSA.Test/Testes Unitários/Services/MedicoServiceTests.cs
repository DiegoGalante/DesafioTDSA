using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Notificacoes;
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
        public void MedicoService_ValidarEspecialidade_DeveSerValida()
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


        [Fact(DisplayName = "MedicoService - ValidarEspecialidades - Devem Ser Válidas")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarEspecialidades_DevemSerValidas()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var especialidades = _medicoServiceTestsFixture.GerarEspecialidades(10);

            //Act
            var result = medicoService.ValidarEspecialidades(especialidades);

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarEspecialidades - Devem Ser Inválidas")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarEspecialidades_DevemSerInvalidas()
        {
            //Arrange
            var mockNotificador = new Mock<INotificador>();
            var medicoRepo = new Mock<IMedicoRepository>();

            var medicoService = new MedicoService(medicoRepo.Object, mockNotificador.Object);
            var especialidades = _medicoServiceTestsFixture.GerarEspecialidadesVariadas();

            //Act
            var result = medicoService.ValidarEspecialidades(especialidades);

            //Assert
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
            mockNotificador.Verify(r => r.NotificarErro(It.IsAny<Notificacao>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "MedicoService - ValidarCPFJaCadastrado - Deve Ser Falso")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_ValidarCPFJaCadastrado_DeveSerFalse()
        {
            //Arrange
            var mocker = new AutoMocker();

            var medicoService = mocker.CreateInstance<MedicoService>();

            var cpf = new Faker().Person.Cpf(true);
            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorCPF(cpf))
                      .Returns(_medicoServiceTestsFixture.GerarMedicoComCpfFixo(cpf));

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            medico.AtualizarCPF(cpf);

            //Act
            var result = medicoService.ValidarCPFJaCadastrado(medico);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(r => r.ObterPorCPF(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotificador>().Verify(r => r.NotificarErro(It.IsAny<Notificacao>()), Times.Once);
            Assert.False(result);
        }

        [Fact(DisplayName = "Medico Deve Ser Atualizado")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Atualizar_DeveSerAtualizado()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId(medico));

            //Act
            var result = await medicoService.Atualizar(medico);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(r => r.Atualizar(medico), Times.Once);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.SaveChanges(), Times.Once);
            Assert.Equal(medico, result);
        }

        [Fact(DisplayName = "Medico Não Deve Passar na Validação por ser inválido")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Atualizar_NaoDevePassarNaValidacao()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();
            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            //Act
            var result = await medicoService.Atualizar(medico);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(r => r.Atualizar(medico), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.SaveChanges(), Times.Never);
            Assert.Null(result);
        }

        [Fact(DisplayName = "Medico Não Deve Atualizar Por Não Encontrar No Banco")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Atualizar_NaoDeveAtualizarPorNaoEncontrarNoBanco()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId());

            //Act
            var result = await medicoService.Atualizar(medico);

            //Assert
            mocker.GetMock<INotificador>().Verify(r => r.NotificarErro(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.Atualizar(medico), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.SaveChanges(), Times.Never);
            Assert.Null(result);
        }

        [Fact(DisplayName = "Medico Não Deve Atualizar Por Possuir Notificação")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Atualizar_NaoDeveAtualizarPorPossuirNoticacao()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId(medico));

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(true));

            //Act
            var result = await medicoService.Atualizar(medico);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(r => r.Atualizar(medico), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(r => r.SaveChanges(), Times.Never);
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve Possuir Notificação")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_OperacaoValida_DevePossuirNoticacao()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(true));

            //Act
            var result = medicoService.OperacaoValida();

            //Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Não Deve Possuir Notificação")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_OperacaoValida_NaoDevePossuirNoticacao()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(false));

            //Act
            var result = medicoService.OperacaoValida();

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve Obter Pelo Id")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_ObterPorId_DeveObterPeloId()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId(medico));

            //Act
            var result = await medicoService.ObterPorId(medico.Id);

            //Assert
            Assert.Equal(medico, result);
        }

        [Fact(DisplayName = "Deve RetornarNulo")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_ObterPorId_DeveRetornarNulo()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId());

            //Act
            var result = await medicoService.ObterPorId(medico.Id);

            //Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve Obter a Lista de Médicos")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Listar_DeveObterListaDeMedicos()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.Listar())
                      .Returns(_medicoServiceTestsFixture.ObterMedicosVariados());

            //Act
            var result = await medicoService.Listar();

            //Assert
            Assert.True(result.Any());
        }

        [Fact(DisplayName = "Deve Retornar Lista Vazia")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Listar_DeveRetornarListaVazia()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.Listar())
                      .Returns(_medicoServiceTestsFixture.ObterMedicosVariados(true));

            //Act
            var result = await medicoService.Listar();

            //Assert
            Assert.True(!result.Any());
        }

        [Fact(DisplayName = "Deve Retornar Lista Com Especialidade Pesquisada")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Listar_DeveRetornarListaComEspecialidadePesquisada()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var especialidade = new Faker("pt_BR").Random.String(200, 'a', 'z');

            mocker.GetMock<IMedicoRepository>().Setup(m => m.Listar(especialidade))
                      .Returns(_medicoServiceTestsFixture.Listar(especialidade, true));

            //Act
            var result = await medicoService.Listar(especialidade);

            //Assert
            Assert.True(result.Any());
            Assert.True(result.Where(c => c.Especialidades.Select(n => n.Nome).Contains(especialidade)).ToList().Count == result.Count);
        }

        [Fact(DisplayName = "Deve Retornar Lista Vazia")]
        [Trait("Services", "MedicoService Testes")]
        public async Task MedicoService_Listar_DeveRetornarListaVaziaComBaseNaEspecialidadePesquisada()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var especialidade = new Faker("pt_BR").Random.String(200, 'a', 'z');

            mocker.GetMock<IMedicoRepository>().Setup(m => m.Listar(especialidade))
                      .Returns(_medicoServiceTestsFixture.Listar(especialidade, false));

            //Act
            var result = await medicoService.Listar(especialidade);

            //Assert
            Assert.DoesNotContain(result, c => c.Especialidades.Select(n => n.Nome).Contains(especialidade));
        }

        [Fact(DisplayName = "Médico Deve Ser Removido")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_Remover_DeveSerRemovido()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId(medico));

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(false));
            //Act
            medicoService.Remover(medico.Id);

            //Assert
            mocker.GetMock<IMedicoRepository>().Verify(m => m.Remover(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IMedicoRepository>().Verify(m => m.SaveChanges(), Times.Once);
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>()), Times.Never);
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Médico Não Deve Ser Removido Pelo Guid ser Vazio")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_Remover_NaoDeveSerRemovidoPeloGuidSerVazio()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoInvalido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId(medico));

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(true));

            //Act
            medicoService.Remover(medico.Id);

            //Assert
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(m => m.Remover(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact(DisplayName = "Médico Não Deve Ser Removido Por não Encontrar No Banco")]
        [Trait("Services", "MedicoService Testes")]
        public void MedicoService_Remover_NaoDeveSerRemovidoPorNaoEncontrarNoBanco()
        {
            //Arrange
            var mocker = new AutoMocker();
            var medicoService = mocker.CreateInstance<MedicoService>();

            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            mocker.GetMock<IMedicoRepository>().Setup(m => m.ObterPorId(medico.Id))
                      .Returns(_medicoServiceTestsFixture.ObterPorId());

            mocker.GetMock<INotificador>().Setup(m => m.TemNotificacao())
                      .Returns(_medicoServiceTestsFixture.OperacaoValida(true));

            //Act
            medicoService.Remover(medico.Id);

            //Assert
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>()), Times.Never);
            mocker.GetMock<INotificador>().Verify(m => m.NotificarErro(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mocker.GetMock<IMedicoRepository>().Verify(m => m.Remover(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IMedicoRepository>().Verify(m => m.SaveChanges(), Times.Never);
        }

    }
}
