using System;
using System.Collections.Generic;

namespace TDSA.Business.Models
{
    public class Medico : Entity
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string CRM { get; private set; }
        public List<Especialidade> Especialidades { get; private set; }

        private Medico() : base(Guid.Empty)
        {

        }
        public Medico(Guid id, string nome, string cpf, string crm, List<Especialidade> especialidades) : base(id)
        {
            Nome = nome;
            CPF = cpf;
            CRM = crm;
            Especialidades = especialidades;
        }
    }
}
