using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Api.Controllers;
using TDSA.Api.ViewModels;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;

namespace TDSA.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/medico")]
    public class MedicoController : MainController
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService,
                                INotificador notificacador) : base(notificacador)
        {
            _medicoService = medicoService;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(CadastrarMedicoViewModel medicoViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var especialidades = MontaEspecialidades(medicoViewModel.Especialidades);
            var medico = new Medico(Guid.NewGuid(), medicoViewModel.Nome, medicoViewModel.Cpf, medicoViewModel.Crm, especialidades);

            var retornoId = await _medicoService.Cadastrar(medico);

            if (!OperacaoValida() || retornoId == Guid.Empty)
                return CustomResponse();

            return Ok(new
            {
                id = retornoId
            });
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar(AtualizarMedicoViewModel medicoViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var especialidades = MontaEspecialidades(medicoViewModel.Especialidades);
            var medico = new Medico(medicoViewModel.Id, medicoViewModel.Nome, medicoViewModel.Cpf, medicoViewModel.Crm, especialidades);

            var retorno = await _medicoService.Atualizar(medico);

            if (!OperacaoValida() || retorno == null)
                return CustomResponse();

            var viewModel = MontaMedicoViewModel(medico);
            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var medicos = await _medicoService.Listar();
            if (!medicos.Any())
                return NotFound(medicos);

            var viewModel = new List<MedicoViewModel>();

            foreach (var medico in medicos)
                viewModel.Add(MontaMedicoViewModel(medico));

            return Ok(viewModel);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var medico = await _medicoService.ObterPorId(id);
            if (medico == null)
                return NotFound();

            var viewModel = MontaMedicoViewModel(medico);

            return Ok(viewModel);
        }


        [HttpGet("{especialidade}")]
        public async Task<IActionResult> ListarEspecialidade(string especialidade)
        {
            var medicos = await _medicoService.Listar(especialidade);
            if (medicos == null)
                return NotFound();

            var viewModel = new List<MedicoViewModel>();

            foreach (var medico in medicos)
                viewModel.Add(MontaMedicoViewModel(medico));

            return Ok(viewModel);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await _medicoService.Remover(id);

            if (!OperacaoValida())
                return CustomResponse();

            return Ok();
        }

        private MedicoViewModel MontaMedicoViewModel(Medico medico)
        {
            if (medico == null)
                return null;

            return new MedicoViewModel()
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Cpf = medico.CPF,
                Crm = medico.CRM,
                Especialidades = medico.Especialidades?.Select(x => x.Nome).ToList()
            };

        }

        private List<Especialidade> MontaEspecialidades(List<string> especialidadesViewModel)
        {
            var especialidades = new List<Especialidade>();

            if (especialidadesViewModel == null)
                return especialidades;

            if (!especialidadesViewModel.Any())
                return especialidades;

            foreach (var especialidade in especialidadesViewModel)
                especialidades.Add(new Especialidade(Guid.NewGuid(), especialidade));

            return especialidades;
        }
    }
}
