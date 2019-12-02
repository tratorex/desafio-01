using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Marvin.Sdk.Mvc;
using Consultorio.Negocios.Entidades;
using Consultorio.Negocios.Repositorios;

namespace Consultorio.WebApp.Validadores
{
    public class ConsultaValidador : IValidador<Consulta>
    {
        private IConsultaRepository repository;

        public ConsultaValidador(IConsultaRepository repository)
        {
            this.repository = repository;
        }

        public async Task Validar(Consulta entity, ModelStateDictionary modelState)
        {
            await ValidarAsync(entity, modelState);
        }

        public async Task Validar(Consulta entity, Consulta entityUpdate, ModelStateDictionary modelState)
        {
            await ValidarAsync(entity, modelState);
        }

        private async Task ValidarAsync(Consulta entity, ModelStateDictionary modelState)
        {
            await Task.Delay(0);

            if (entity == null)
                modelState.AddModelError("Consulta", "A consulta não deve ser nula");
            if (entity?.DataInicial == null)
                modelState.AddModelError("Consulta", "O campo data de inicio da consulta não pode ser nulo");
            if (entity?.DataFinal == null)
                modelState.AddModelError("Consulta", "O campo data final da consulta não pode ser nulo");
            if (entity?.DataInicial > entity?.DataFinal)
                modelState.AddModelError("Consulta", "A data final não pode ser anterior a data inicial da consulta");
            if (this.repository != null)
            {
                if (await this.repository.ExisteConsulta(entity))
                    modelState.AddModelError("Consulta", "Já existe uma consulta marcada dentro do período informado");
            }
        }
    }
}
