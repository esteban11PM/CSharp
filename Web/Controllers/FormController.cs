using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de formularios en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormBusiness _formBusiness;
        private readonly ILogger<FormController> _logger;

        /// <summary>
        /// Constructor del controlador de formularios.
        /// </summary>
        /// <param name="formBusiness">Capa de negocio de formularios.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public FormController(FormBusiness formBusiness, ILogger<FormController> logger)
        {
            _formBusiness = formBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios registrados en el sistema.
        /// </summary>
        /// <returns>Lista de formularios.</returns>
        /// <response code="200">Retorna la lista de formularios.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var forms = await _formBusiness.GetAllFormsAsync();
                return Ok(forms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de formularios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un formulario específico por su ID.
        /// </summary>
        /// <param name="id">ID del formulario.</param>
        /// <returns>Formulario solicitado.</returns>
        /// <response code="200">Retorna el formulario solicitado.</response>
        /// <response code="400">ID proporcionado no válido.</response>
        /// <response code="404">Formulario no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var form = await _formBusiness.GetFormByIdAsync(id);
                return Ok(form);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo formulario en el sistema.
        /// </summary>
        /// <param name="formDto">Datos del formulario a crear.</param>
        /// <returns>Formulario creado.</returns>
        /// <response code="201">Retorna el formulario creado.</response>
        /// <response code="400">Datos del formulario no válidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDTO formDto)
        {
            try
            {
                var createdForm = await _formBusiness.CreateFormAsync(formDto);
                return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear formulario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de un formulario existente.
        /// </summary>
        /// <param name="id">ID del formulario a actualizar.</param>
        /// <param name="formDto">Datos actualizados del formulario.</param>
        /// <returns>Formulario actualizado.</returns>
        /// <response code="200">Formulario actualizado correctamente.</response>
        /// <response code="400">Datos no válidos.</response>
        /// <response code="404">Formulario no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateForm([FromBody] FormDTO formDto)
        {
            try
            {
                var updatedForm = await _formBusiness.UpdateFormAsync(formDto);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar formulario con ID: {FormId}", formDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", formDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar formulario con ID: {FormId}", formDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un formulario existente por su ID.
        /// </summary>
        /// <param name="id">ID del formulario a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        /// <response code="204">El formulario se eliminó correctamente.</response>
        /// <response code="400">ID no válido.</response>
        /// <response code="404">Formulario no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var eliminacion =await _formBusiness.DeleteFormAsync(id);
                return Ok(new {message = "Se eliminó el registro", eliminacion});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Elimina de manera logica un form del sistema
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Logical/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalFormAsync(int id)
        {
            try
            {
                var deleted = await _formBusiness.DeleteFormLogicalAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = "Formulario no encontrado o ya eliminado." });
                }

                return Ok(new { message = "Eliminación lógica exitosa." });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el formulario con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el formulario de manera lógica con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}