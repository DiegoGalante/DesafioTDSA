namespace TDSA.Business.Notificacoes
{
    public class Notificacao
    {
        public string Campo { get; set; }
        public string Erro { get; set; }

        public Notificacao(string erro)
        {
            Erro = erro;
        }

        public Notificacao(string campo, string erro)
        {
            Campo = campo;
            Erro = erro;
        }
    }
}
