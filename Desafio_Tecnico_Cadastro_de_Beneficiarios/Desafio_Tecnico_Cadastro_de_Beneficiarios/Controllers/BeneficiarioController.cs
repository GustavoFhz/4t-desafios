using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiarioController : ControllerBase
    {
        
        private readonly IBeneficiarioInterface _beneficiarioInterface;

        // Construtor para injeção de dependência
        public BeneficiarioController(IBeneficiarioInterface beneficiarioInterface)
        {
            _beneficiarioInterface = beneficiarioInterface;
        }

        /// <summary>
        /// Criar beneficiário
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarBeneficiario([FromBody] BeneficiarioCriacaoDto dto)
        {
            var beneficiario = await _beneficiarioInterface.CriarBeneficiario(dto);

            // Verifica se a operação falhou
            if (!beneficiario.Status)
            {
                // Se o erro for de validação (ex: CPF duplicado), retorna 409 Conflict
                if (beneficiario.Error == "ValidationError")
                    return Conflict(beneficiario);

                // Para qualquer outro tipo de erro, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            // Se a operação foi bem sucedida, retorna 201 Created com detalhes do beneficiário
            return CreatedAtAction(nameof(Detalhe), new { id = beneficiario.Dados.Id }, beneficiario);
        }

        /// <summary>
        /// Lista todos os beneficiários
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarBeneficiarios()
        {
            var beneficiario = await _beneficiarioInterface.ListarBeneficiarios();

            // Se houver falha ao listar os beneficiários, retorna 500
            if (!beneficiario.Status)
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);

            // Se a operação for bem sucedida, retorna 200 OK
            return Ok(beneficiario);
        }

        /// <summary>
        /// Retorna um beneficiário pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Detalhe(int id)
        {
            var beneficiario = await _beneficiarioInterface.BuscarBeneficiariosPorId(id);

            // Se a operação falhar
            if (!beneficiario.Status)
            {
                // Se o erro for de validação (beneficiário não encontrado), retorna 404 Not Found
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                // Para qualquer outro tipo de erro, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            // Se a operação for bem sucedida, retorna 200 OK com os dados do beneficiário
            return Ok(beneficiario);
        }

        /// <summary>
        /// Edita um beneficiário existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarBeneficiario([FromBody] BeneficiarioEdicaoDto dto)
        {
            var beneficiario = await _beneficiarioInterface.EditarBeneficiarios(dto);

            // Se a operação falhar
            if (!beneficiario.Status)
            {
                // Se o beneficiário não for encontrado, retorna 404 Not Found
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                // Para qualquer outro erro, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            // Se a edição foi bem sucedida, retorna 200 OK
            return Ok(beneficiario);
        }

        /// <summary>
        /// Deleta um beneficiário pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarBeneficiario(int id)
        {
            var beneficiario = await _beneficiarioInterface.DeletarBeneficiario(id);

            // Se a operação falhar
            if (!beneficiario.Status)
            {
                // Se o beneficiário não for encontrado, retorna 404 Not Found
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                // Para qualquer outro erro, retorna 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            // Se a exclusão for bem sucedida, retorna 200 OK com mensagem de sucesso
            return Ok(beneficiario);
        }
    }
}
