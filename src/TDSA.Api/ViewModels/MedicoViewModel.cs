using System;
using System.Collections.Generic;

namespace TDSA.Api.ViewModels
{
    public struct MedicoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }

        public string Crm { get; set; }

        public IEnumerable<string> Especialidades { get; set; }
    }
}
