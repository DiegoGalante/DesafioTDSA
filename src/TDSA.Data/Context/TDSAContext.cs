using Microsoft.EntityFrameworkCore;
using TDSA.Business.Models;

namespace TDSA.Data.Context
{
    public class TDSAContext : DbContext
    {
        public TDSAContext(DbContextOptions<TDSAContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
    }
}
