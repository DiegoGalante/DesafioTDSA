using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDSA.Business.Models;

namespace TDSA.Business.Interfaces
{
    public interface IMedicoService
    {
        Task<Medico> ObterPorId(Guid id);
        Task<IList<Medico>> Listar();
        Task<IList<Medico>> Listar(string especialidade);

        Task<Guid> Cadastrar(Medico medico);
        Task<Medico> Atualizar(Medico medico);
        void Remover(Guid id);
    }
}
