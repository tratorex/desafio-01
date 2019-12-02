using FluentMigrator;

namespace Consultorio.Migrations
{
    [TimestampedMigration(2019, 11, 29, 18, 34)]
    public class Consulta_20191129183400 : Migration
    {
        public override void Down()
        {
            this.Delete.Table("Consulta");
        }

        public override void Up()
        {
            this.Create.Table("Consulta")
                       .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                       .WithColumn("NomePaciente").AsString().NotNullable()
                       .WithColumn("DataNascimento").AsDateTime().NotNullable()
                       .WithColumn("DataInicial").AsDateTime().NotNullable()
                       .WithColumn("DataFinal").AsDateTime().NotNullable()
                       .WithColumn("Observacoes").AsString().Nullable()
                       ;
        }
    }
}
