using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Data.Context;

namespace TDSA.Data.Repository
{
    public class EspecialidadeRepository : Repository<Especialidade>, IEspecialidadeRepository
    {
        public EspecialidadeRepository(TDSAContext dbContext) : base(dbContext)
        { }

        public async Task<List<Especialidade>> Listar(Guid medicoId)
        {
            return await Db.Especialidades.AsNoTracking()
                                          .Where(e => e.MedicoId == medicoId)
                                          .ToListAsync();
        }
    }
}
