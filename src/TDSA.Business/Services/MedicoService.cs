﻿using System;
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

            await _medicoRepository.Atualizar(medico);
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

                RemoverEspecialidades(medicoBanco);
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

        public bool ValidarEspecialidades(List<Especialidade> especialidades)
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

        private bool RemoverEspecialidades(Medico medico)
        {
            var especialidades = medico.Especialidades;

            foreach (var especialidade in especialidades)
                _especialidadeRepository.Remover(especialidade.Id).Wait();

            _especialidadeRepository.SaveChanges().Wait();
            medico.LimparEspecialidades();

            return true;
        }

        private bool OperacaoValida()
        {
            return !_notificacador.TemNotificacao();
        }
    }
}
