﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TDSA.Api.ViewModels
{
    public class AtualizarMedicoViewModel
    {
        [Required(ErrorMessage ="Campo Id é requerido para a atualização!")]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Crm { get; set; }
        public List<string> Especialidades { get; set; }
    }
}