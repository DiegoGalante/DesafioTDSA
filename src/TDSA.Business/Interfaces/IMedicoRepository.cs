using System.Collections.Generic;
using System.Threading.Tasks;
using TDSA.Business.Models;

namespace TDSA.Business.Interfaces
{
    public interface IMedicoRepository : IRepository<Medico>
    {
        Task<Medico> ObterPorCPF(string cpf);
        Task<List<Medico>> Listar(string especialidade);
    }
}
