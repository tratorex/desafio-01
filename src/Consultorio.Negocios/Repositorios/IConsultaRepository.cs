using Consultorio.Negocios.Entidades;
using Marvin.Sdk.Business;
using System.Threading.Tasks;

namespace Consultorio.Negocios.Repositorios
{
    public interface IConsultaRepository : IRepository<Consulta>
    {
        Task<bool> ExisteConsulta(Consulta entity);
    }
}