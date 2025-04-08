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
    /// Controlador para la gestión de roles en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolController : ControllerBase
    {
        private readonly RolBusiness _rolBusiness;
        private readonly ILogger<RolController> _logger;

        /// <summary>
        /// Constructor del controlador de roles.
        /// </summary>
        /// <param name="rolBusiness">Capa de negocio de roles.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public RolController(RolBusiness rolBusiness, ILogger<RolController> logger)
        {
            _rolBusiness = rolBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles del sistema.
        /// </summary>
        /// <returns>Lista de roles.</returns>
        /// <response code="200">Retorna la lista de roles.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var roles = await _rolBusiness.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener roles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rol específico por su ID.
        /// </summary>
        /// <param name="id">ID del rol.</param>
        /// <returns>Rol solicitado.</returns>
        /// <response code="200">Retorna el rol solicitado.</response>
        /// <response code="400">ID proporcionado no válido.</response>
        /// <response code="404">Rol no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {
                var rol = await _rolBusiness.GetRolByIdAsync(id);
                return Ok(rol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo rol en el sistema.
        /// </summary>
        /// <param name="rolDto">Datos del rol a crear.</param>
        /// <returns>Rol creado.</returns>
        /// <response code="201">Retorna el rol creado.</response>
        /// <response code="400">Datos del rol no válidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRol([FromBody] RolDTO rolDto)
        {
            try
            {
                var createdRol = await _rolBusiness.CreateRolAsync(rolDto);
                // CreatedAtAction se usa para retornar el recurso creado y su ubicación
                return CreatedAtAction(nameof(GetRolById), new { id = createdRol.Id }, createdRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rol");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un rol existente.
        /// </summary>
        /// <param name="id">ID del rol a actualizar.</param>
        /// <param name="rolDto">Datos actualizados del rol.</param>
        /// <returns>Rol actualizado.</returns>
        /// <response code="200">Rol actualizado correctamente.</response>
        /// <response code="400">Datos no válidos.</response>\n        /// <response code="404">Rol no encontrado.</response>\n        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RolDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolDTO rolDto)
        {
            try
            {
                if (id != rolDto.Id)
                {
                    return BadRequest(new { message = "El ID de la ruta no coincide con el ID del objeto." });
                }
                var updatedRol = await _rolBusiness.UpdateRolAsync(rolDto);
                return Ok(updatedRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un rol existente por su ID.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        /// <response code="204">El rol se eliminó correctamente.</response>\n        /// <response code="400">ID no válido.</response>\n        /// <response code="404">Rol no encontrado.</response>\n        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                var eliminacion =await _rolBusiness.DeleteRolAsync(id);
                return Ok(new {message = "Se eliminó el registro", eliminacion});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar rol con ID: {RolId}", id);
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
            var deleted = await _rolBusiness.SoftDeleteRolAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Rol no encontrado o ya eliminado." });
            }
            try
            {
                return Ok(new { message = "Eliminación lógica exitosa." });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el Rol con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Rol de manera lógica con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
