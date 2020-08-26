using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using TDSA.Business.Interfaces;

namespace TDSA.Business.Notificacoes
{
    public class Notificador : INotificador
    {
        public Notificador()
        {
            Notificacoes = new List<Notificacao>();
        }
        public List<Notificacao> Notificacoes { get; set; }

        public void NotificarErro(Notificacao notificacao)
        {
            Notificacoes.Add(notificacao);
        }

        public void NotificarErro(string campo, string erro)
        {
            Notificacoes.Add(new Notificacao(campo, erro));
        }

        public void NotificarErro(string erro)
        {
            Notificacoes.Add(new Notificacao(erro));
        }

        public void NotificarErro(List<string> erros)
        {
            foreach (var erro in erros)
                Notificacoes.Add(new Notificacao(erro));
        }

        public void NotificarErros(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
                NotificarErro(new Notificacao(erro.PropertyName, erro.ErrorMessage));
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return Notificacoes;
        }

        public bool TemNotificacao()
        {
            return Notificacoes.Any();
        }
    }
}
