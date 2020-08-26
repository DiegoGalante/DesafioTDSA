using FluentValidation;
using TDSA.Business.Models;

namespace TDSA.Business.Validations.EspecialidadeValidation
{
    public class EspecialidadeValidation : AbstractValidator<Especialidade>
    {
        public EspecialidadeValidation()
        {
            RuleFor(c => c.Id)
            .NotEmpty().WithMessage("{PropertyName} do Médico não informado");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio ou nulo!");
        }
    }
}
