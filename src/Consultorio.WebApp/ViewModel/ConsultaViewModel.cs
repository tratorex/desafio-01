using Consultorio.Negocios.Entidades;
using Marvin.Sdk.Business;
using Marvin.Sdk.Mvc;
using System;
using System.Threading.Tasks;

namespace Consultorio.WebApp.ViewModel
{
    public class ConsultaViewModel : IViewModel<Consulta>
    {
        #region Constructores
        public ConsultaViewModel() { }
        public ConsultaViewModel(Consulta entity)
        {
            this.Id = entity.Id;

            this.NomePaciente = entity.NomePaciente;
            this.DataNascimento = entity.DataNascimento;
            this.DataInicial = entity.DataInicial;
            this.DataFinal = entity.DataFinal;
            this.Observacoes = entity.Observacoes;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Chave primária 
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Nome do paciente
        /// </summary>
        public string NomePaciente { get; set; }

        /// <summary>
        /// Data de nascimento do paciente
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Data e hora inicial da consulta
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Data e hora final da consulta
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Observações sobre a consulta
        /// </summary>
        public string Observacoes { get; set; }

        #endregion

        #region Methods 
        public Task<Consulta> CriarAsync(IRepository<Consulta> repository) => AtualizarAsync(new Consulta(Id), repository);

        public async Task<Consulta> AtualizarAsync(Consulta entity, IRepository<Consulta> repository)
        {
            await Task.Delay(0);

            entity.NomePaciente = this.NomePaciente;
            entity.DataNascimento = this.DataNascimento;
            entity.DataInicial = this.DataInicial;
            entity.DataFinal = this.DataFinal;
            entity.Observacoes = this.Observacoes;

            return entity;
        }
        #endregion
    }
}
