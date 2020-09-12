using System.Collections.Generic;

namespace TDSA.Api.ViewModels
{
    public struct CadastrarMedicoViewModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public IEnumerable<string> Especialidades { get; set; }
    }
}
