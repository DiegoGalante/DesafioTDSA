using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public async Task<List<Medico>> ObterMedicosVariados(bool vazio = false)
        {
            var medicos = new List<Medico>();
            if (vazio)
                return medicos;

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

            return medicos.Generate(quantidade);
        }

        public async Task<Medico> GerarMedicoComCpfFixo(string cpf)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var medico = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(f.Random.Guid(),
                                                      f.Name.FirstName(genero),
                                                      cpf,
                                                      f.Random.String(10, 'a', 'z'),
                                                      (List<Especialidade>)GerarEspecialidades(f.Random.Int(1, 10))));

            return medico;
        }

        public async Task<Medico> ObterPorId(Medico medico = null)
        {
            return medico;
        }

        public async Task<List<Medico>> Listar(string especialidade, bool encontrar)
        {
            var medicos = ObterMedicosVariados().Result;

            if (!encontrar)
                return medicos;

            var random = new Faker().Random.Int(0, medicos.Count);

            var index = medicos.FindIndex(random, m => !m.Especialidades.Select(c => c.Nome).Contains(especialidade));
            var medico = medicos.GetRange(index, 1).FirstOrDefault();

            var novaEspecialidade = new Especialidade(Guid.NewGuid(), especialidade);
            medico.AdicionarEspecialidade(novaEspecialidade);

            return medicos.Where(m => m.Especialidades.Contains(novaEspecialidade)).ToList();
        }



        public ICollection<Especialidade> GerarEspecialidades(int quantidade)
        {
            var especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(f.Random.Guid(),
                                                                                                               f.Random.String(5, 'a', 'z')));

            return especialidades.Generate(quantidade);
        }

        public ICollection<Especialidade> GerarEspecialidadesInvalidas(int quantidade)
        {
            var especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(Guid.Empty,
                                                                                                               string.Empty));

            return especialidades.Generate(quantidade);
        }

        public ICollection<Especialidade> GerarEspecialidadesVariadas()
        {
            var especialidades = new List<Especialidade>();

            especialidades.AddRange(GerarEspecialidades(50));
            especialidades.AddRange(GerarEspecialidadesInvalidas(50));

            return especialidades;
        }

        public Medico GerarMedicoInvalido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(Guid.Empty,
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

        public bool OperacaoValida(bool comNotificacao = false)
        {
            return comNotificacao;
        }

        public void Dispose()
        {

        }
    }
}
