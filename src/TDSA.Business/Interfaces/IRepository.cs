using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDSA.Business.Models;

namespace TDSA.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task Atualizar(TEntity entity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> Listar();
        Task Remover(Guid id);
        Task<int> SaveChanges();
    }
}
