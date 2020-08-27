using FluentValidation;
using TDSA.Business.Models;

namespace TDSA.Business.Validations.EspecialidadeValidation
{
    public class EspecialidadeValidation : AbstractValidator<Especialidade>
    {
        public EspecialidadeValidation()
        {
            RuleFor(c => c.Id)
            .NotEmpty().WithMessage("{PropertyName} da especialidade não informado");

            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} da especialidade é obrigatório!");
        }
    }
}
