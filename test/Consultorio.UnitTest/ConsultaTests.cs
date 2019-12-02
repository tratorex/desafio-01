using Consultorio.Negocios.Entidades;
using Consultorio.WebApp.Validadores;
using FluentAssertions;
using System;
using Xunit;

namespace Consultorio.UnitTest
{
    public class ConsultaTests
    {
        public ConsultaTests() { }

        [Fact(DisplayName = "Consulta Válida")]
        public async void ConsultaValida()
        {
            var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            var consulta = new Consulta()
            {
                NomePaciente = "Jhonny Riley",
                DataNascimento = new DateTime(1992, 10, 25),
                DataInicial = new DateTime(2019, 11, 30, 10, 0, 0),
                DataFinal = new DateTime(2019, 11, 30, 12, 0, 0),
            };
            var consultaValidador = new ConsultaValidador(null);
            await consultaValidador.Validar(consulta, modelState);
            modelState.ErrorCount.Should().Be(0, "Objeto inválido");
        }

        [Fact(DisplayName = "Consulta Inválida - Data Inicial anterior a Data Final")]
        public async void ConsultaInvalida()
        {
            var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            var consulta = new Consulta()
            {
                NomePaciente = "Jhonny Riley",
                DataNascimento = new DateTime(1992, 10, 25),
                DataInicial = new DateTime(2019, 11, 30, 13, 0, 0),
                DataFinal = new DateTime(2019, 11, 30, 12, 0, 0),
            };
            var consultaValidador = new ConsultaValidador(null);
            await consultaValidador.Validar(consulta, modelState);
            modelState.ErrorCount.Should().Be(1, "Objeto inválido");
        }
    }
}
