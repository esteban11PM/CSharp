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
    /// Controlador para la gestión de permisos en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _permissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos.
        /// </summary>
        /// <param name="permissionBusiness">Capa de negocio de permisos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public PermissionController(PermissionBusiness permissionBusiness, ILogger<PermissionController> logger)
        {
            _permissionBusiness = permissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos registrados en el sistema.
        /// </summary>
        /// <returns>Lista de permisos.</returns>
        /// <response code="200">Retorna la lista de permisos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var permissions = await _permissionBusiness.GetAllPermissionsAsync();
                return Ok(permissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso específico por su ID.
        /// </summary>
        /// <param name="id">ID del permiso.</param>
        /// <returns>Permiso solicitado.</returns>
        /// <response code="200">Retorna el permiso solicitado.</response>
        /// <response code="400">ID proporcionado no válido.</response>
        /// <response code="404">Permiso no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var permission = await _permissionBusiness.GetPermissionByIdAsync(id);
                return Ok(permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo permiso en el sistema.
        /// </summary>
        /// <param name="permissionDto">Datos del permiso a crear.</param>
        /// <returns>Permiso creado.</returns>
        /// <response code="201">Retorna el permiso creado.</response>
        /// <response code="400">Datos del permiso no válidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDTO permissionDto)
        {
            try
            {
                var createdPermission = await _permissionBusiness.CreatePermissionAsync(permissionDto);
                return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de un permiso existente.
        /// </summary>
        /// <param name="id">ID del permiso a actualizar.</param>
        /// <param name="permissionDto">Datos actualizados del permiso.</param>
        /// <returns>Permiso actualizado.</returns>
        /// <response code="200">Permiso actualizado correctamente.</response>
        /// <response code="400">Datos no válidos.</response>
        /// <response code="404">Permiso no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDTO permissionDto)
        {
            try
            {
                var updatedPermission = await _permissionBusiness.UpdatePermissionAsync(permissionDto); //en el business no se espera el id hay que modificarse.
                return Ok(updatedPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un permiso existente por su ID.
        /// </summary>
        /// <param name="id">ID del permiso a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        /// <response code="204">El permiso se eliminó correctamente.</response>
        /// <response code="400">ID no válido.</response>
        /// <response code="404">Permiso no encontrado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                await _permissionBusiness.DeletePermissionAsync(id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}