using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using TDSA.Business.Extensions;
using TDSA.Business.Validations;

namespace TDSA.Business.Models
{
    public class Medico : Entity
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string CRM { get; private set; }
        public ICollection<Especialidade> Especialidades { get; private set; }

        private Medico() : base(Guid.Empty) { }
        public Medico(Guid id, string nome, string cpf, string crm, ICollection<Especialidade> especialidades) : base(id)
        {
            Nome = nome;
            CPF = cpf;
            CRM = crm;
            Especialidades = especialidades;
        }

        public void AtualizarNome(string nome)
        {
            if (nome.EhVazio())
                throw new Exception("Nome é obrigatório!");

            if (nome.Length >= 255)
                throw new Exception("Nome não pode ser maior que 255 caracteres!");

            Nome = nome;
        }

        public void AtualizarCRM(string crm)
        {
            if (crm.EhVazio())
                throw new Exception("CRM é obrigatório!");

            CRM = crm;
        }

        public void AtualizarCPF(string cpf)
        {
            if (cpf.EhVazio())
                throw new Exception("CPF inválido!");

            if (!CpfValidacao.Validar(cpf.Trim()))
                throw new Exception("CPF inválido!");

            CPF = cpf;
        }

        public void AdicionarEspecialidade(Especialidade especialidade)
        {
            if (Especialidades == null)
                Especialidades = new List<Especialidade>();

            if (especialidade == null)
                throw new Exception("Especialidade é obrigatória!");

            if (especialidade.Nome.EhVazio())
                throw new Exception("Especialidade é obrigatória!");

            Especialidades.Add(especialidade);
        }

        public void AdicionarEspecialidades(ICollection<Especialidade> especialidades)
        {
            foreach (var especialidade in especialidades)
                AdicionarEspecialidade(especialidade);
        }

        public void LimparEspecialidades()
        {
            Especialidades.Clear();
        }
    }
}
