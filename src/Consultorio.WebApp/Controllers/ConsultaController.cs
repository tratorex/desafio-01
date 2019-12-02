using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marvin.Sdk.Mvc;
using Marvin.Sdk.Search;
using Consultorio.Negocios.Entidades;
using Consultorio.Negocios.Repositorios;
using Consultorio.WebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Consultorio.WebApp.Controllers

{    /// <summary>
     /// Consulta
     /// </summary>
    [Route("api/[controller]")]
    public class ConsultaController : MarvinController<Consulta, IConsultaRepository>
    {
        protected override string[] ColunasPesquisaRapida => new[]
        {
            nameof(Consulta.NomePaciente),
            nameof(Consulta.DataNascimento),
            nameof(Consulta.DataInicial),
            nameof(Consulta.DataFinal)
        };

        public ConsultaController(IConsultaRepository repository, ISearcher<Consulta> pesquisador, IValidador<Consulta> validador)
        {
            this.validador = validador;
            base.Pesquisador = pesquisador;
            base.Repository = repository;
        }

        #region HttpGet

        /// <summary>
        /// Obter Objeto paginada ou Todos as Objetos
        /// </summary>
        /// <param name="request">Parametro de Pesquisa</param>
        /// <returns>Retorna entidade Encontrado(a)</returns>
        /// <response code="404">Não foi possível encontrar o Objeto correspondente</response>
        /// <response code="409">Ocorreu erro de validação</response>
        /// <response code="500">Ocorreu erro interno</response>
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Obter([FromQuery]UriRequest request)
        {
            if (request.Page == 0 && request.PageSize == 0)
            {
                var all = (await this.Repository.AllAsync()).Select((entity => new ConsultaViewModel(entity)));
                return ResponderJsonResult(all);
            }

            var pesquisador = CriarPesquisador(request);
            var pesquisa = pesquisador.SearchAsync(request.Search, (request.Page - 1) * request.PageSize, request.PageSize);
            var totalRegistros = pesquisador.CountAsync(request.Search);
            await Task.WhenAll(pesquisa, totalRegistros);

            var data = pesquisa.Result.Select(entity => new ConsultaViewModel(entity)).ToList();
            var grid = new GridResult<ConsultaViewModel>(data, request.Page, pesquisa.Result.Count, totalRegistros.Result);

            return ResponderJsonResult(grid);
        }

        /// <summary>
        /// Obter o Objeto por Id
        /// </summary>
        /// <param name="id">Id do Objeto que deseja consultar</param>
        /// <returns>Retorna entidade Encontrado(a)</returns>
        /// <response code="404">Não foi possível encontrar o Objeto correspondente</response>
        /// <response code="409">Ocorreu erro de validação</response>
        /// <response code="500">Ocorreu erro interno</response>
        [HttpGet, Route("{id}"), AllowAnonymous]
        public async Task<IActionResult> Obter(long id) => await this.ObterAsync<ConsultaViewModel>(id);
        #endregion

        #region HttpPost

        /// <summary>
        /// Cria um novo Item
        /// </summary>
        /// <remarks>
        /// Exemplo de Requisição:
        /// 
        ///     POST
        ///     {
        ///       "id": 0,
        ///       "nomePaciente": "Nome paciente",
        ///       "dataNascimento": "1992-10-25",
        ///       "dataInicial": "2019-11-30T10:00",
        ///       "dataFinal": "2019-11-30T12:00",
        ///       "observacoes": "nenhuma"
        ///     }
        /// </remarks>
        /// <param name="viewModel">Objeto que deseja incluir</param>
        /// <returns>Retorna Objeto Criado no Banco de Dados</returns>
        /// <response code="200">Objeto criado com sucesso</response>
        /// <response code="409">Ocorreu algum erro de validação</response>
        /// <response code="500">Ocorreu algum erro interno na inclusão</response>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Criar([FromBody]ConsultaViewModel viewModel) => await this.CriarAsync(viewModel);
        #endregion

        #region HttpPut

        /// <summary>
        /// Altera Objeto já criado no banco
        /// </summary>
        /// <param name="viewModel">Objeto que deseja alterar</param>
        /// <returns>Retorna arquivo Alterado</returns>
        [HttpPut, AllowAnonymous]
        public async Task<IActionResult> Atualizar([FromBody]ConsultaViewModel viewModel) => await this.AtualizarAsync(viewModel);
        #endregion

        #region HttpDelete

        /// <summary>
        /// Delete Objeto informando o Id
        /// </summary>
        /// <param name="id">Id do Objeto que deseja Deletar</param>
        /// <returns>
        /// Retorna Objeto deletado do banco de dados
        /// </returns>
        /// <response code="204">Retorna quando execução realizada com sucesso</response>
        /// <response code="404">Retorna quando não foi possível encontrar o registro que deseja deletar</response>
        /// <response code="500">Ocorrer quando não for possível excluir registro por existir relação com outras tabelas</response>
        [HttpDelete, Route("{id}"), AllowAnonymous]
        public async Task<IActionResult> Excluir(long id) => await this.ExcluirAsync(id);
        #endregion

    }
}