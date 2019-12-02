
using FluentNHibernate.Mapping;
using Consultorio.Negocios.Entidades;

namespace Consultorio.Dados.Mappings
{
    public class ConsultaMappings : ClassMap<Consulta>
    {
        public ConsultaMappings()
        {
            this.Table("Consulta");
            this.Id(x => x.Id).Not.Nullable();

            this.Map(x => x.NomePaciente).Not.Nullable();
            this.Map(x => x.DataNascimento).Not.Nullable();
            this.Map(x => x.DataInicial).Not.Nullable();
            this.Map(x => x.DataFinal).Not.Nullable();
            this.Map(x => x.Observacoes).Nullable();
        }
    }
}
