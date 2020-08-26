using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDSA.Business.Models;

namespace TDSA.Business.Interfaces
{
    public interface IEspecialidadeRepository : IRepository<Especialidade>
    {
        Task<List<Especialidade>> Listar(Guid medicoId);
    }
}
