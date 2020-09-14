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


        public override void Atualizar(Medico medico)
        {
            SincronizarEspecialidades(medico);
            Db.Medicos.Update(medico);
        }

        private void SincronizarEspecialidades(Medico medico)
        {
            var especialidadesNoBanco = Db.Especialidades.Where(x => x.MedicoId == medico.Id).ToList();

            Db.Especialidades.AddRange(medico.Especialidades.Where(x => !especialidadesNoBanco.Any(y => y.MedicoId == x.MedicoId &&
                                                                                                   y.Id == x.Id)));
            Db.Especialidades.UpdateRange(medico.Especialidades.Where(x => especialidadesNoBanco.Any(y => y.MedicoId == x.MedicoId &&
                                                                                        y.Id == x.Id)));
            Db.Especialidades.RemoveRange(especialidadesNoBanco.Where(x => !medico.Especialidades.Any(y => y.MedicoId == x.MedicoId &&
                                                                                         y.Id == x.Id)));
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
