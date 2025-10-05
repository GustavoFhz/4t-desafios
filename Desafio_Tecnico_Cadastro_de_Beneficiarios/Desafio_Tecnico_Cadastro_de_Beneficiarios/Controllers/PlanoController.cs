using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoController : ControllerBase
    {
        
        private readonly IPlanoInterface _planoInterface;

        // Construtor para injeção de dependência
        public PlanoController(IPlanoInterface planoInterface)
        {
            _planoInterface = planoInterface;
        }

        /// <summary>
        /// Criar Plano
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarPlano([FromBody] PlanoCriacaoDto planoCriacaoDto)
        {
            var plano = await _planoInterface.CriarPlano(planoCriacaoDto);

            // Se a operação falhar
            if (!plano.Status)
            {
                // Se o erro for de validação (ex: plano duplicado), retorna 409 Conflict
                if (plano.Error == "ValidationError")
                    return Conflict(plano);

                // Para qualquer outro erro inesperado, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            // Se a criação for bem sucedida, retorna 201 Created com detalhes do plano
            return CreatedAtAction(nameof(Detalhe), new { id = plano.Dados.Id }, plano);
        }

        /// <summary>
        /// Lista todos os planos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarPlanos()
        {
            var plano = await _planoInterface.ListarPlanos();

            // Se houver falha ao listar os planos, retorna 500
            if (!plano.Status)
                return StatusCode(StatusCodes.Status500InternalServerError, plano);

            // Se a operação for bem sucedida, retorna 200 OK
            return Ok(plano);
        }

        /// <summary>
        /// Retorna um plano pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Detalhe(int id)
        {
            var plano = await _planoInterface.BuscarPlanoPorId(id);

            // Se a operação falhar
            if (!plano.Status)
            {
                // Se o erro for de validação (plano não encontrado), retorna 404 Not Found
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                // Para qualquer outro erro inesperado, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            // Se a operação for bem sucedida, retorna 200 OK com os dados do plano
            return Ok(plano);
        }

        /// <summary>
        /// Edita um plano existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarPlano([FromBody] PlanoEdicaoDto planoEdicaoDto)
        {
            var plano = await _planoInterface.EditarPlano(planoEdicaoDto);

            // Se a operação falhar
            if (!plano.Status)
            {
                // Se o plano não for encontrado, retorna 404 Not Found
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                // Para qualquer outro erro inesperado, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            // Se a edição for bem sucedida, retorna 200 OK
            return Ok(plano);
        }

        /// <summary>
        /// Deleta um plano pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarPlano(int id)
        {
            var plano = await _planoInterface.DeletarPlano(id);

            // Se a operação falhar
            if (!plano.Status)
            {
                // Se o plano não for encontrado, retorna 404 Not Found
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                // Para qualquer outro erro inesperado, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            // Se a exclusão for bem sucedida, retorna 200 OK com mensagem de sucesso
            return Ok(plano);
        }
    }
}
