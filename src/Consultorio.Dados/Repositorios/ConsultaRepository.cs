using System.Threading.Tasks;
using Consultorio.Negocios.Entidades;
using Consultorio.Negocios.Repositorios;
using Marvin.Sdk.NH;
using NHibernate;

namespace Consultorio.Dados.Repositorios
{
    public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(ISession session) : base(session) { }

        public Task<bool> ExisteConsulta(Consulta entity) =>
            this.ContainsAsync(x => (entity.DataInicial >= x.DataInicial && entity.DataInicial < x.DataFinal) ||
                (entity.DataFinal >= x.DataInicial && entity.DataFinal < x.DataFinal));
    }
}