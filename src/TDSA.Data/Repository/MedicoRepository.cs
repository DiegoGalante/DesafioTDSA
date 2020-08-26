using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSA.Business.Extensions;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Data.Context;

namespace TDSA.Data.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        public MedicoRepository(TDSAContext dbContext) : base(dbContext)
        { }


        public override async Task Atualizar(Medico medico)
        {
            Db.Medicos.Update(medico);
            Db.Especialidades.AddRange(medico.Especialidades);
        }


        public override async Task<Medico> ObterPorId(Guid id)
        {
            return await Db.Medicos.AsNoTracking()
                .Include(m => m.Especialidades)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public override async Task<List<Medico>> Listar()
        {
            return await Db.Medicos.AsNoTracking()
                .Include(m => m.Especialidades)
                .ToListAsync();

        }

        public async Task<List<Medico>> Listar(string especialidade)
        {
            return await Db.Medicos.AsNoTracking()
                .Include(m => m.Especialidades)
                .Where(e => e.Especialidades.Any(x => x.Nome != null &&
                                                x.Nome.Trim().ToLower().Contains(especialidade.FormataStringComLetrasMinusculas())))
                .ToListAsync();
        }

        public async Task<Medico> ObterPorCPF(string cpf)
        {
            return await Db.Medicos.AsNoTracking()
                                   .Where(x => x.CPF == cpf)
                                   .FirstOrDefaultAsync();
        }
    }
}
