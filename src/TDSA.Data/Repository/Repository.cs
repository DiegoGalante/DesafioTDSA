using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDSA.Business.Interfaces;
using TDSA.Business.Models;
using TDSA.Data.Context;

namespace TDSA.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly TDSAContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(TDSAContext dbContext)
        {
            Db = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public async Task Adicionar(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task Adicionar(IList<TEntity> entity)
        {
            await DbSet.AddRangeAsync(entity);
        }

        public virtual void Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task<List<TEntity>> Listar()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task Remover(Guid id)
        {
            TEntity entity = await ObterPorId(id);
            DbSet.Remove(entity);
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
