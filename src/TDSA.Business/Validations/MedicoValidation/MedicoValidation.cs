using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using TDSA.Business.Models;

namespace TDSA.Business.Validations.MedicoValidation
{
    public class MedicoValidation : AbstractValidator<Medico>
    {
        /*
         - nome não pode ser vazio ou nulo;
         - nome não pode ser maior que 255 caracteres;

         - cpf deve ser válido;

         - crm não pode ser vazio ou nulo;

         - deve conter no minimo uma especialidade;
         */

        public MedicoValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("{PropertyName} do Médico não informado");

            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} é obrigatório!")
                .MaximumLength(255)
                .WithMessage("{PropertyName} não pode ser maior que 255 caracteres!");

            RuleFor(x => x.CRM)
                .NotEmpty()
                .NotEmpty()
                .WithMessage("{PropertyName} é obrigatório!");

            RuleFor(x => x.Especialidades)
                .Must(ValidaQuantidadeMinimaDeEspecialidades)
                .WithMessage("{PropertyName} deve ter no mínimo uma especialidade!");

            RuleFor(x=>x.CPF)
                .Must(CpfValidacao.Validar)
                .WithMessage("{PropertyName} inválido!");
        }

        private bool ValidaQuantidadeMinimaDeEspecialidades(List<Especialidade> especialidades)
        {
            if (especialidades == null)
                return false;

            if (!especialidades.Any())
                return false;

            return true;
        }
    }
}
