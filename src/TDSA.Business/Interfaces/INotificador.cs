using FluentValidation.Results;
using System.Collections.Generic;
using TDSA.Business.Notificacoes;

namespace TDSA.Business.Interfaces
{
    public interface INotificador
    {
        void NotificarErro(List<string> erros);
        void NotificarErro(Notificacao notificacao);
        void NotificarErro(string erro);
        void NotificarErro(string campo, string erro);
        void NotificarErros(ValidationResult validationResult);
        List<Notificacao> ObterNotificacoes();
        bool TemNotificacao();
    }
}
