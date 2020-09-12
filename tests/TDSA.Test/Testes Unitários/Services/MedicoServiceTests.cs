using Bogus;
using Bogus.Extensions.Brazil;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Linq;
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


            especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(_faker.Random.Guid(),
                                                                                                            _faker.Random.String(5, 'a', 'z')))
                                                               .Generate(2);

            _medico = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(_faker.Random.Guid(),
                                                      _faker.Person.FirstName,
                                                      _faker.Person.Cpf(true),
                                                      _faker.Random.String(10, 'a', 'z'),
                                                      especialidades))
                .Generate(1)
                .First();
        }


        [Fact(DisplayName = "MedicoService - ValidarMedico - Deve Ser Valido")]
        public void MedicoService_ValidarMedico_DeveSerValido()
        {
            MedicoService.ValidarMedico(_medico);

            var mock = Mocker.GetMock<INotificador>();
            mock.Verify(x => x.NotificarErro(It.IsAny<Notificacao>()), Times.Never());
        }

    }
}
