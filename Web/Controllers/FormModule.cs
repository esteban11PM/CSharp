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
    /// Controlador para la gestión de la relación entre formularios y módulos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormModuleController : ControllerBase
    {
        private readonly FormModuleBusiness _formModuleBusiness;
        private readonly ILogger<FormModuleController> _logger;

        /// <summary>
        /// Constructor del controlador de FormModule.
        /// </summary>
        /// <param name="formModuleBusiness">Capa de negocio de FormModule.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public FormModuleController(FormModuleBusiness formModuleBusiness, ILogger<FormModuleController> logger)
        {
            _formModuleBusiness = formModuleBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las asignaciones de formularios a módulos.
        /// </summary>
        /// <returns>Lista de FormModuleDTO.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFormModules()
        {
            try
            {
                var list = await _formModuleBusiness.GetAllFormModulesAsync();
                return Ok(list);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las asignaciones de formularios a módulos.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una asignación de formulario a módulo por su ID.
        /// </summary>
        /// <param name="id">ID del FormModule.</param>
        /// <returns>FormModuleDTO.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormModuleById(int id)
        {
            try
            {
                var record = await _formModuleBusiness.GetFormModuleByIdAsync(id);
                return Ok(record);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el FormModule con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "FormModule no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener FormModule con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva asignación de formulario a módulo.
        /// </summary>
        /// <param name="dto">Datos del FormModule a crear.</param>
        /// <returns>Registro creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(FormModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFormModule([FromBody] FormModuleCreateDTO dto)
        {
            try
            {
                var createdRecord = await _formModuleBusiness.CreateFormModuleAsync(dto);
                return CreatedAtAction(nameof(GetFormModuleById), new { id = createdRecord.Id }, createdRecord);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear FormModule.");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear FormModule.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una asignación de formulario a módulo existente.
        /// </summary>
        /// <param name="id">ID del registro a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>Resultado de la actualización.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormModule(int id, [FromBody] FormModuleCreateDTO dto)
        {
            try
            {
                var updated = await _formModuleBusiness.UpdateFormModuleAsync(dto);
                return Ok(updated);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar FormModule con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "FormModule no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar FormModule con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una asignación de formulario a módulo por su ID.
        /// </summary>
        /// <param name="id">ID del registro a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteFormModule(int id)
        {
            try
            {
                var eliminacion = await _formModuleBusiness.DeleteFormModuleAsync(id);
                return Ok(new{message = "Se eliminó el registro", eliminacion});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar FormModule con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "FormModule no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar FormModule con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
