using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Business.Notificacoes;
using TDSA.Business.Services;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Services
{
    public class MedicoServiceTests
    {
        private AutoMocker Mocker;
        private readonly MedicoService MedicoService;
        private Faker _faker;
        private Medico _medico;

        public MedicoServiceTests()
        {
            Mocker = new AutoMocker();
            MedicoService = Mocker.CreateInstance<MedicoService>(true);

            _faker = new Faker("pt_BR");
            var especialidades = new List<Especialidade>();

            for (int i = 0; i < _faker.Random.Int(1, 5); i++)
                especialidades.Add(new Especialidade(_faker.Random.String(5, 'a', 'z')));

            _medico = new Medico(_faker.Person.FirstName,
                                 _faker.Person.Cpf(true),
                                 _faker.Random.String(10, 'a', 'z'),
                                 especialidades);
        }


        [Fact(DisplayName = "MedicoService - ValidarMedico - Deve Ser Valido")]
        public void MedicoService_ValidarMedico_DeveSerValido()
        {
            var ehValido = MedicoService.ValidarMedico(_medico);

            ehValido.Should().BeTrue();
        }

        [Fact(DisplayName = "MedicoService - ValidarMedico - Nome Deve Ser Invalido Pelo Tamanho")]
        public void MedicoService_ValidarMedico_NomeDeveSerInvalidoPeloTamanho()
        {

            var medico = new Medico(_faker.Random.String(256, 'a', 'z'),
                                    _medico.CPF,
                                    _medico.CRM,
                                    _medico.Especialidades);

            var ehValido = MedicoService.ValidarMedico(medico);


            var mock = Mocker.GetMock<INotificador>();


            mock.Verify(x => x.NotificarErro(It.IsAny<Notificacao>()), Times.Once(), "Nome não pode ser maior que 255 caracteres!");
        }

        [Fact(DisplayName = "MedicoService - ValidarMedico - Nome Deve Ser Invalido Por Ser Vazio Ou Nulo")]
        public void MedicoService_ValidarMedico_NomeDeveSerInvalidoPorSerVazioOuNulo()
        {
            var medico = new Medico(string.Empty,
                                    _medico.CPF,
                                    _medico.CRM,
                                    _medico.Especialidades);

            var ehValido = MedicoService.ValidarMedico(medico);


            var mock = Mocker.GetMock<INotificador>();
            mock.Verify(x => x.NotificarErro(It.IsAny<Notificacao>()), Times.Once(), "Nome não pode ser vazio ou nulo!");
        }

        [Fact(DisplayName = "MedicoService - ValidarMedico - CRM Deve Ser Invalido Por Ser Vazio Ou Nulo")]
        public void MedicoService_ValidarMedico_CRMDeveSerInvalidoPorSerVazioOuNulo()
        {
            var medico = new Medico(_medico.Nome,
                                    _medico.CPF,
                                    string.Empty,
                                    _medico.Especialidades);

            var ehValido = MedicoService.ValidarMedico(medico);

            var mock = Mocker.GetMock<INotificador>();
            mock.Verify(x => x.NotificarErro(It.IsAny<Notificacao>()), Times.Once(), "CRM não pode ser vazio ou nulo!");
        }

        [Fact(DisplayName = "MedicoService - ValidarMedico - Especialidades Deve Ser Invalido Pelo Ser Lista Vazia")]
        public void MedicoService_ValidarMedico_EspecialidadesDeveSerInvalidoPorSerListaVazia()
        {
            var medico = new Medico(_medico.Nome,
                                    _medico.CPF,
                                    _medico.CRM,
                                    new List<Especialidade>());

            var ehValido = MedicoService.ValidarMedico(medico);

            var mock = Mocker.GetMock<INotificador>();
            mock.Verify(x => x.NotificarErro(It.IsAny<Notificacao>()), Times.Once(), "Especialidades deve ter no mínimo uma especialidade!");
        }


    }
}
