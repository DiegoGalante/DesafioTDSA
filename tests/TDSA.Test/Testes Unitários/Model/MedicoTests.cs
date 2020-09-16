using Bogus;
using FluentAssertions;
using System;
using System.Linq;
using TDSA.Business.Models;
using TDSA.Test.Testes_Unitários.Fixture;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Model
{
    [Collection(nameof(MedicoBogusCollection))]
    public class MedicoTests
    {
        readonly MedicoTestsFixtureService _medicoServiceTestsFixture;
        private readonly Faker _faker;

        public MedicoTests(MedicoTestsFixtureService medicoServiceTestsFixture)
        {
            _medicoServiceTestsFixture = medicoServiceTestsFixture;

            _faker = new Faker("pt_BR");
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarNome - Nome deve ser atualizado")]
        public void Medico_AtualizarNome_NomeDeveSerAtualizado()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            var novoNome = _faker.Person.FirstName;

            //Act
            medico.AtualizarNome(novoNome);

            //Assert
            medico.Nome.Should().Be(novoNome);
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "MedicoService - AtualizarNome - Nome deve ser inválido por ser vazio ou nulo")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Medico_AtualizarNome_NomeDeveSerInValidoPorSerVazioOuNulo(string nome)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AtualizarNome(nome)).Message;

            //Assert
            validacao.Should().Contain("Nome é obrigatório!");
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarNome - Nome deve ser inválido pelo tamanho")]
        public void Medico_AtualizarNome_NomeDeveSerInValidoPeloTamanho()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            var nomeInvalido = _faker.Random.String(256, 'a', 'z');

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AtualizarNome(nomeInvalido)).Message;

            //Assert
            validacao.Should().Contain("Nome não pode ser maior que 255 caracteres!");
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "Medico - AtualizarCRM - Crm deve ser válido")]
        [InlineData("1223-SC")]
        [InlineData("teste")]
        [InlineData("aa ")]
        [InlineData(" aa ")]
        [InlineData(" aa")]
        public void Medico_AtualizarCRM_CrmDeveSerValido(string crm)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            medico.AtualizarCRM(crm);

            //Assert
            medico.CRM.Should().Be(crm);
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "MedicoService - AtualizarCRM - Crm deve ser inválido por ser vazio")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Medico_AtualizarCRM_CrmDeveSerInValidoPorSerVazio(string crm)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AtualizarCRM(crm)).Message;

            //Assert
            validacao.Should().Contain("CRM é obrigatório!");
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "Medico - AtualizarCPF - Cpf deve ser inválido")]
        [InlineData("238.677.850-9")]
        [InlineData("238.677.850-900")]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("1-a")]
        [InlineData(null)]
        public void Medico_AtualizarCPF_CpfDeveSerInValido(string cpf)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AtualizarCPF(cpf)).Message;

            //Assert
            validacao.Should().Contain("CPF inválido!");
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "Medico - AtualizarCPF - Cpf deve ser válido")]
        [InlineData("238.677.850-90")]
        [InlineData("23867785090")]
        public void Medico_AtualizarCPF_CrmDeveSerValido(string cpf)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            medico.AtualizarCPF(cpf);

            //Assert
            medico.CPF.Should().Be(cpf);
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser válido")]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerValido()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            var especialidade = new Especialidade(_faker.Random.Guid(), _faker.Random.String(5, 'a', 'z'));
            //Act
            medico.AdicionarEspecialidade(especialidade);

            //Assert
            medico.Especialidades.Should().Contain(especialidade);
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarEspecialidade - Especialidade deve ser válida mesmo com a propriedade Especialidades instânciada nula")]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerValia()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValidoComEspeclidadeNula();
            var especialidade = _medicoServiceTestsFixture.GerarEspecialidadeValida();

            //Act
            medico.AdicionarEspecialidade(especialidade);

            //Assert
            medico.Especialidades.Should().Contain(especialidade);
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarEspecialidades - Especialidades devem ser adicionadas")]
        public void Medico_AtualizarEspecialidades_EspecialidadesDeveSerAdicionadas()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            medico.LimparEspecialidades();

            var especialidades = _medicoServiceTestsFixture.GerarEspecialidades(_faker.Random.Int(1, 50)).ToList();

            //Act
            medico.AdicionarEspecialidades(especialidades);

            //Assert
            medico.Especialidades.Should().HaveCount(especialidades.Count());
        }

        [Trait("Model", "Medico Testes")]
        [Theory(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser inválido por ser Vazio Ou Nulo")]
        [InlineData("")]
        [InlineData(null)]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerInvalidoValidoPorSerVazioOuNullo(string nome)
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            var especialidade = new Especialidade(_faker.Random.Guid(), nome);

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AdicionarEspecialidade(especialidade)).Message;

            //Assert
            validacao.Should().Contain("Especialidade é obrigatória!");
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - AtualizarEspecialidade - Especialidades deve ser inválido por ser Vazio Ou Nulo")]
        public void Medico_AtualizarEspecialidade_EspecialidadesDeveSerInvalidoValidoPorSerNullo()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();

            //Act
            var validacao = Assert.Throws<Exception>(() => medico.AdicionarEspecialidade(null)).Message;

            //Assert
            validacao.Should().Contain("Especialidade é obrigatória!");
        }

        [Trait("Model", "Medico Testes")]
        [Fact(DisplayName = "Medico - LimparEspecialidades - Especialidades deve ser ter a lista esvaziada")]
        public void Medico_LimparEspecialidades_EspecialidadesDeveAListaEsvaziada()
        {
            //Arrange
            var medico = _medicoServiceTestsFixture.GerarMedicoValido();
            var especialidades = _medicoServiceTestsFixture.GerarEspecialidades(_faker.Random.Int(1, 15)).ToList();

            //Act
            medico.AdicionarEspecialidades(especialidades);
            medico.LimparEspecialidades();

            //Assert
            medico.Especialidades.Should().HaveCount(0);
        }
    }

}
