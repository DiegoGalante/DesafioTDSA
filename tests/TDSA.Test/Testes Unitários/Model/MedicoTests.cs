using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TDSA.Business.Models;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Model
{
    public class MedicoTests
    {
        private Faker _faker;
        private Medico _medico;

        public MedicoTests()
        {
            _faker = new Faker("pt_BR");
            var especialidades = new List<Especialidade>();

            for (int i = 0; i < _faker.Random.Int(1, 5); i++)
                especialidades.Add(new Especialidade(_faker.Random.Guid(), _faker.Random.String(5, 'a', 'z')));


            _medico = new Faker<Medico>("pt_BR")
                .CustomInstantiator((f) => new Medico(_faker.Random.Guid(),
                                                      _faker.Person.FirstName,
                                                      _faker.Person.Cpf(true),
                                                      _faker.Random.String(10, 'a', 'z'),
                                                      especialidades))
                .Generate(1)
                .First();
        }

        [Fact(DisplayName = "Medico - AtualizarNome - Nome deve ser atualizado")]
        public void Medico_AtualizarNome_NomeDeveSerAtualizado()
        {
            //Arrange
            var novoNome = _faker.Person.FirstName;

            //Act
            _medico.AtualizarNome(novoNome);

            //Assert
            _medico.Nome.Should().Be(novoNome);
        }

        [Theory(DisplayName = "MedicoService - AtualizarNome - Nome deve ser inválido por ser vazio ou nulo")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Medico_AtualizarNome_NomeDeveSerInValidoPorSerVazioOuNulo(string nome)
        {
            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AtualizarNome(nome)).Message;

            //Assert
            validacao.Should().Contain("Nome é obrigatório!");
        }

        [Fact(DisplayName = "Medico - AtualizarNome - Nome deve ser inválido pelo tamanho")]
        public void Medico_AtualizarNome_NomeDeveSerInValidoPeloTamanho()
        {
            //Arrange
            var nomeInvalido = _faker.Random.String(256, 'a', 'z');

            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AtualizarNome(nomeInvalido)).Message;

            //Assert
            validacao.Should().Contain("Nome não pode ser maior que 255 caracteres!");
        }

        [Theory(DisplayName = "Medico - AtualizarCRM - Crm deve ser válido")]
        [InlineData("1223-SC")]
        [InlineData("teste")]
        [InlineData("aa ")]
        [InlineData(" aa ")]
        [InlineData(" aa")]
        public void Medico_AtualizarCRM_CrmDeveSerValido(string crm)
        {
            //Act
            _medico.AtualizarCRM(crm);

            //Assert
            _medico.CRM.Should().Be(crm);
        }

        [Theory(DisplayName = "Medico - AtualizarCRM - Crm deve ser inválido por ser vazio")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Medico_AtualizarCRM_CrmDeveSerInValidoPorSerVazio(string crm)
        {
            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AtualizarCRM(crm)).Message;

            //Assert
            validacao.Should().Contain("CRM é obrigatório!");
        }

        [Theory(DisplayName = "Medico - AtualizarCPF - Cpf deve ser inválido")]
        [InlineData("238.677.850-9")]
        [InlineData("238.677.850-900")]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("1-a")]
        [InlineData(null)]
        public void Medico_AtualizarCPF_CpfDeveSerInValido(string cpf)
        {
            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AtualizarCPF(cpf)).Message;

            //Assert
            validacao.Should().Contain("CPF inválido!");
        }

        [Theory(DisplayName = "Medico - AtualizarCPF - Cpf deve ser válido")]
        [InlineData("238.677.850-90")]
        [InlineData("23867785090")]
        public void Medico_AtualizarCPF_CrmDeveSerValido(string cpf)
        {
            //Act
            _medico.AtualizarCPF(cpf);

            //Assert
            _medico.CPF.Should().Be(cpf);
        }

        [Fact(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser válido")]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerValido()
        {
            var especialidade = new Especialidade(_faker.Random.Guid(), _faker.Random.String(5, 'a', 'z'));
            //Act
            _medico.AdicionarEspecialidade(especialidade);

            //Assert
            _medico.Especialidades.Should().Contain(especialidade);
        }

        [Fact(DisplayName = "Medico - AtualizarEspecialidades - Especialidades devem ser adicionadas")]
        public void Medico_AtualizarEspecialidades_EspecialidadesDeveSerAdicionadas()
        {
            _medico.LimparEspecialidades();

            var especialidades = new Faker<Especialidade>("pt_BR")
                                        .CustomInstantiator((f) => new Especialidade(_faker.Random.Guid(), _faker.Random.String(5, 'a', 'z')))
                                        .Generate(5);


            //Act
            _medico.AdicionarEspecialidades(especialidades);

            //Assert
            _medico.Especialidades.Should().HaveCount(especialidades.Count());
        }

        [Theory(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser inválido por ser Vazio Ou Nulo")]
        [InlineData("")]
        [InlineData(null)]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerInvalidoValidoPorSerVazioOuNullo(string nome)
        {
            var especialidade = new Especialidade(_faker.Random.Guid(), nome);

            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AdicionarEspecialidade(especialidade)).Message;

            //Assert
            validacao.Should().Contain("Especialidade é obrigatório!");
        }

        [Fact(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser invalido por ser nulo")]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerInvalidoValidoPorSerNulo()
        {
            //Act
            var validacao = Assert.Throws<Exception>(() => _medico.AdicionarEspecialidade(null)).Message;

            //Assert
            validacao.Should().Contain("Especialidade é obrigatório!");
        }

        [Fact(DisplayName = "Medico - LimparEspecialidades - Especialidades deve ser ter a lista esvaziada")]
        public void Medico_LimparEspecialidades_EspecialidadesDeveAListaEsvaziada()
        {
            var especialidades = new Faker<Especialidade>("pt_BR")
                                        .CustomInstantiator((f) => new Especialidade(_faker.Random.Guid(), _faker.Random.String(5, 'a', 'z')))
                                        .Generate(5);


            //Act
            _medico.AdicionarEspecialidades(especialidades);
            _medico.LimparEspecialidades();

            _medico.Especialidades.Should().HaveCount(0);
        }
    }

}
