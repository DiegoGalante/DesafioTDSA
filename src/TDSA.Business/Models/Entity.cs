using System;

namespace TDSA.Business.Models
{
    public abstract class Entity
    {
        public DateTime DataCadastro { get; private set; }
        public Guid Id { get; private set; }

        protected Entity(Guid id)
        {
            Id = id;
            DataCadastro = DateTime.UtcNow;
        }
    }
}
