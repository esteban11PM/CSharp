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
    /// Controlador para la gestión de módulos en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModuleController : ControllerBase
    {
        private readonly ModuleBusiness _moduleBusiness;
        private readonly ILogger<ModuleController> _logger;

        /// <summary>
        /// Constructor del controlador de módulos.
        /// </summary>
        /// <param name="moduleBusiness">Capa de negocio de módulos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public ModuleController(ModuleBusiness moduleBusiness, ILogger<ModuleController> logger)
        {
            _moduleBusiness = moduleBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los módulos registrados en el sistema.
        /// </summary>
        /// <returns>Lista de módulos.</returns>
        /// <response code="200">Retorna la lista de módulos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllModules()
        {
            try
            {
                var modules = await _moduleBusiness.GetAllModulesAsync();
                return Ok(modules);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de módulos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un módulo específico por su ID.
        /// </summary>
        /// <param name="id">ID del módulo.</param>
        /// <returns>Módulo solicitado.</returns>
        /// <response code="200">Retorna el módulo solicitado.</response>
        /// <response code="400">ID proporcionado no válido.</response>
        /// <response code="404">Módulo no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetModuleById(int id)
        {
            try
            {
                var module = await _moduleBusiness.GetModuleByIdAsync(id);
                return Ok(module);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el módulo con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Módulo no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener módulo con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo módulo en el sistema.
        /// </summary>
        /// <param name="moduleDto">Datos del módulo a crear.</param>
        /// <returns>Módulo creado.</returns>
        /// <response code="201">Retorna el módulo creado.</response>
        /// <response code="400">Datos del módulo no válidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModule([FromBody] ModuleDTO moduleDto)
        {
            try
            {
                var createdModule = await _moduleBusiness.CreateModuleAsync(moduleDto);
                return CreatedAtAction(nameof(GetModuleById), new { id = createdModule.Id }, createdModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear módulo");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear módulo");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de un módulo existente.
        /// </summary>
        /// <param name="id">ID del módulo a actualizar.</param>
        /// <param name="moduleDto">Datos actualizados del módulo.</param>
        /// <returns>Módulo actualizado.</returns>
        /// <response code="200">Módulo actualizado correctamente.</response>
        /// <response code="400">Datos no válidos.</response>
        /// <response code="404">Módulo no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] ModuleDTO moduleDto)
        {
            try
            {
                var updatedModule = await _moduleBusiness.UpdateModuleAsync(moduleDto); //se debe tener en cuenta el Id en el business
                return Ok(updatedModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar módulo con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Módulo no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar módulo con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un módulo existente por su ID.
        /// </summary>
        /// <param name="id">ID del módulo a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        /// <response code="204">El módulo se eliminó correctamente.</response>
        /// <response code="400">ID no válido.</response>
        /// <response code="404">Módulo no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                var eliminacion = await _moduleBusiness.DeleteModuleAsync(id);
                return Ok(new{message = "Se eliminó el registro", eliminacion});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar módulo con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Módulo no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar módulo con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un Module de manera logica
        /// </summary>
        [HttpDelete("Logical/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalModuleAsync(int id)
        {
            try
            {
                var deleted = await _moduleBusiness.DeleteModuleLogicalAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = "Module no encontrado o ya eliminado." });
                }

                return Ok(new { message = "Eliminación lógica exitosa." });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el Module con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Module de manera lógica con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
