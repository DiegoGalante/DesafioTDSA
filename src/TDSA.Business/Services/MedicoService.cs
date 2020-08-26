using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Business.Notificacoes;
using TDSA.Business.Validations.MedicoValidation;

namespace TDSA.Business.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly INotificador _notificacador;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IEspecialidadeRepository _especialidadeRepository;
        public MedicoService(IMedicoRepository medicoRepository,
                             IEspecialidadeRepository especialidadeRepository,
                             INotificador notificacador)
        {
            _medicoRepository = medicoRepository;
            _notificacador = notificacador;
            _especialidadeRepository = especialidadeRepository;
        }

        public async Task<Guid> Cadastrar(Medico medico)
        {
            Validar(medico);

            if (_notificacador.TemNotificacao())
                return Guid.Empty;

            await _medicoRepository.Adicionar(medico);
            await _medicoRepository.SaveChanges();

            return medico.Id;
        }

        public async Task<Medico> Atualizar(Medico medico)
        {
            ValidarMedico(medico);

            if (!RemoverEspecialidades(medico, false))
                _notificacador.NotificarErro("Atualizar Médico", "Não foi possível atualizar as especialidades");

            if (_notificacador.TemNotificacao())
                return null;

            await _especialidadeRepository.SaveChanges();
            await _medicoRepository.Atualizar(medico);
            await _medicoRepository.SaveChanges();

            return medico;
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

        public void ValidarMedico(Medico medico)
        {
            var result = new MedicoValidation().Validate(medico);
            if (!result.IsValid)
                _notificacador.NotificarErros(result);
        }


        private void ValidarCPFJaCadastrado(Medico medico)
        {
            var result = _medicoRepository.ObterPorCPF(medico.CPF).Result;
            if (result != null)
                _notificacador.NotificarErro(new Notificacao("CPF", "CPF já cadastrado!"));
        }

        private void Validar(Medico medico)
        {
            ValidarMedico(medico);
            ValidarCPFJaCadastrado(medico);
        }

        private bool RemoverEspecialidades(Medico medico, bool salvarAlteracoesNoMetodo = true)
        {
            var especialidades = _especialidadeRepository.Listar(medico.Id).Result;
            if (especialidades == null)
                return false;

            foreach (var especialidade in especialidades)
                _especialidadeRepository.Remover(especialidade.Id).Wait();

            if (salvarAlteracoesNoMetodo)
                _especialidadeRepository.SaveChanges().Wait();

            return true;
        }
    }
}
