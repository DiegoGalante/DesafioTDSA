using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using TDSA.Business.Models;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Fixture
{
    [CollectionDefinition(nameof(MedicoBogusCollection))]
    public class MedicoBogusCollection : ICollectionFixture<MedicoTestsFixtureService>
    { }

    public class MedicoTestsFixtureService : IDisposable
    {
        public Medico GerarMedicoValido()
        {
            return GerarMedicos(1).FirstOrDefault();
        }


        public IEnumerable<Medico> ObterMedicosVariados()
        {
            var medicos = new List<Medico>();

            medicos.AddRange(GerarMedicos(new Faker().Random.Int(1, 100)).ToList());

            return medicos;
        }

        public IEnumerable<Medico> GerarMedicos(int quantidade)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var medicos = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(f.Random.Guid(),
                                                      f.Name.FirstName(genero),
                                                      f.Person.Cpf(true),
                                                      f.Random.String(10, 'a', 'z'),
                                                      (List<Especialidade>)GerarEspecialidades(f.Random.Int(1, 10))));
            //.RuleFor(c => c.Email, (f, c) =>
            //      f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return medicos.Generate(quantidade);
        }

        public IEnumerable<Especialidade> GerarEspecialidades(int quantidade)
        {
            var especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(f.Random.Guid(),
                                                                                                           f.Random.String(5, 'a', 'z')));

            return especialidades.Generate(quantidade);
        }

        public Medico GerarMedicoInvalido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(f.Random.Guid(),
                                                      string.Empty,
                                                      f.Person.Cpf(true),
                                                      f.Random.String(10, 'a', 'z'),
                                                      new List<Especialidade>()));

            return cliente;
        }

        public Especialidade GerarEspecialidadeValida()
        {
            return GerarEspecialidades(1).FirstOrDefault();
        }

        public Especialidade GerarEspecialidadeInvalida()
        {
            var especialidade = new Faker<Especialidade>("pt_BR")
                .CustomInstantiator((f) => new Especialidade(Guid.Empty,
                                                             string.Empty));

            return especialidade;
        }

        public void Dispose()
        {

        }
    }
}
