using System;

namespace TDSA.Business.Models
{
    public class Especialidade : Entity
    {
        public Guid MedicoId { get; set; }

        public string Nome { get; set; }


        /* EF Relation */
        public Medico Medico { get; set; }

        private Especialidade() : base(Guid.Empty)
        {

        }

        public Especialidade(Guid id, string nome) : base(id)
        {
            Nome = nome;
        }
    }
}
