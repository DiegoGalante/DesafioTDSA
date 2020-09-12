using Bogus;
using Bogus.Extensions.Brazil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Business.Notificacoes;
using TDSA.Business.Validations.EspecialidadeValidation;
using TDSA.Business.Validations.MedicoValidation;

namespace TDSA.Business.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly INotificador _notificacador;
        private readonly IMedicoRepository _medicoRepository;
        public MedicoService(IMedicoRepository medicoRepository,
                             INotificador notificacador)
        {
            _medicoRepository = medicoRepository;
            _notificacador = notificacador;
        }

        public async Task<Guid> Cadastrar(Medico medico)
        {
            if (!ValidarCadastro(medico))
                return Guid.Empty;

            await _medicoRepository.Adicionar(medico);
            await _medicoRepository.SaveChanges();

            return medico.Id;
        }

        public async Task<Medico> Atualizar(Medico medico)
        {
            if (!ValidarAtualizacao(medico))
                return null;

            AtualizarDadosDoMedico(medico);

            if (!OperacaoValida())
                return null;

            _medicoRepository.Atualizar(medico);
            await _medicoRepository.SaveChanges();

            return medico;
        }

        private Medico AtualizarDadosDoMedico(Medico medico)
        {
            var medicoBanco = _medicoRepository.ObterPorId(medico.Id).Result;
            if (medicoBanco == null)
                _notificacador.NotificarErro("Atualizar Médico", "Id do médico inválido!");

            try
            {
                medicoBanco.AtualizarNome(medico.Nome);
                medicoBanco.AtualizarCRM(medico.CRM);
                medicoBanco.AtualizarCPF(medico.CPF);

                medicoBanco.Especialidades.Clear();
                medicoBanco.AdicionarEspecialidades(medico.Especialidades);
            }
            catch (Exception ex)
            {
                _notificacador.NotificarErro("Atualizar Médico", ex.Message);
            }

            return medicoBanco;
        }

        public async Task<IList<Medico>> Listar()
        {
            return await _medicoRepository.Listar();
        }

        public async Task<IList<Medico>> Listar(string especialidade)
        {
            return await _medicoRepository.Listar(especialidade);
        }

        public async Task<Medico> ObterPorId(Guid id)
        {
            return await _medicoRepository.ObterPorId(id);
        }

        public async Task Remover(Guid id)
        {
            if (id == Guid.Empty)
                _notificacador.NotificarErro("Id Inválido para remover!");

            var medico = await _medicoRepository.ObterPorId(id);
            if (medico == null)
                _notificacador.NotificarErro("Remover", "Médico não encontrado ou já removido!");

            if (!_notificacador.TemNotificacao())
            {
                await _medicoRepository.Remover(id);
                await _medicoRepository.SaveChanges();
            }
        }

        public bool ValidarMedico(Medico medico)
        {
            var result = new MedicoValidation().Validate(medico);
            if (!result.IsValid)
            {
                _notificacador.NotificarErros(result);
                return false;
            }

            return true;
        }

        public bool ValidarEspecialidade(Especialidade especialidade)
        {
            var result = new EspecialidadeValidation().Validate(especialidade);
            if (!result.IsValid)
            {
                _notificacador.NotificarErros(result);
                return false;
            }

            return true;
        }

        public bool ValidarEspecialidades(ICollection<Especialidade> especialidades)
        {
            foreach (var especialidade in especialidades)
                if (!ValidarEspecialidade(especialidade))
                    return false;

            return true;
        }


        private bool ValidarCPFJaCadastrado(Medico medico)
        {
            var result = _medicoRepository.ObterPorCPF(medico.CPF).Result;
            if (result != null)
            {
                _notificacador.NotificarErro(new Notificacao("CPF", "CPF já cadastrado!"));
                return false;
            }

            return true;
        }

        private bool ValidarCadastro(Medico medico)
        {
            if (!ValidarMedico(medico))
                return false;

            if (!ValidarEspecialidades(medico.Especialidades))
                return false;

            if (!ValidarCPFJaCadastrado(medico))
                return false;

            return true;
        }

        private bool ValidarAtualizacao(Medico medico)
        {
            if (!ValidarMedico(medico))
                return false;

            if (!ValidarEspecialidades(medico.Especialidades))
                return false;

            return true;
        }

        private bool OperacaoValida()
        {
            return !_notificacador.TemNotificacao();
        }


        private static IList<Medico> GerarMedicos(int quantidade = 1)
        {
            var _fakerMedico = new Faker("pt_BR");


            var medico = new Faker<Medico>("pt_BR").CustomInstantiator((f) => new Medico(_fakerMedico.Random.Guid(),
                                                      _fakerMedico.Person.FirstName,
                                                      _fakerMedico.Person.Cpf(true),
                                                      _fakerMedico.Random.String(10, 'a', 'z'),
                                                      GerarEspecialidades(_fakerMedico.Random.Int(1, 100))));

            return medico.Generate(quantidade);
        }


        private static ICollection<Especialidade> GerarEspecialidades(int quantidade = 1)
        {
            var _faker = new Faker("pt_BR");

            var especialidade = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(_faker.Random.Guid(),
                                                                                                              _faker.Random.String(5, 'a', 'z')));

            return especialidade.Generate(quantidade);
        }
    }
}
