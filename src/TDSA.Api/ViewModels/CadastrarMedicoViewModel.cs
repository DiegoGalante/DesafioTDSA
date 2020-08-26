using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TDSA.Api.ViewModels
{
    public class CadastrarMedicoViewModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public List<string> Especialidades { get; set; }
    }
}
