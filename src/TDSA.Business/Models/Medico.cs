using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using TDSA.Business.Validations;

namespace TDSA.Business.Models
{
    public class Medico : Entity
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string CRM { get; private set; }
        public ICollection<Especialidade> Especialidades { get; private set; }

        private Medico() : base(Guid.Empty)
        {

        }
        public Medico(Guid id, string nome, string cpf, string crm, ICollection<Especialidade> especialidades) : base(id)
        {
            Nome = nome;
            CPF = cpf;
            CRM = crm;
            Especialidades = especialidades;
        }

        public void AtualizarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome?.Trim()))
                throw new Exception("Nome é obrigatório!");

            if (nome.Length >= 255)
                throw new Exception("Nome não pode ser maior que 255 caracteres!");

            Nome = nome;
        }

        public void AtualizarCRM(string crm)
        {
            if (string.IsNullOrEmpty(crm?.Trim()))
                throw new Exception("CRM é obrigatório!");

            CRM = crm;
        }

        public void AtualizarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf?.Trim()))
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
                throw new Exception("Especialidade é obrigatório!");

            if (string.IsNullOrEmpty(especialidade.Nome?.Trim()))
                throw new Exception("Especialidade é obrigatório!");

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
