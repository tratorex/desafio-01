using Marvin.Sdk.Business;
using System;

namespace Consultorio.Negocios.Entidades
{
    public class Consulta : Entity
    {
        public Consulta() { }
        public Consulta(long id) : base(id) { }

        public virtual string NomePaciente { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataFinal { get; set; }
        public virtual string Observacoes { get; set; }
    }
}