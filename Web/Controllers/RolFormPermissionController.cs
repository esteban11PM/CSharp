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
    /// Controlador para la gestión de permisos de formularios asociados a roles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFormPermissionController : ControllerBase
    {
        private readonly RolFormPermissionBusiness _rolFormPermissionBusiness;
        private readonly ILogger<RolFormPermissionController> _logger;

        /// <summary>
        /// Constructor del controlador de RolFormPermission.
        /// </summary>
        /// <param name="rolFormPermissionBusiness">Capa de negocio de RolFormPermission.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public RolFormPermissionController(RolFormPermissionBusiness rolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
        {
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de RolFormPermission.
        /// </summary>
        /// <returns>Lista de RolFormPermissionDTO.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFormPermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolFormPermissions()
        {
            try
            {
                var list = await _rolFormPermissionBusiness.GetAllRolFormPermissionsAsync();
                return Ok(list);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RolFormPermission");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un registro de RolFormPermission por su ID.
        /// </summary>
        /// <param name="id">ID del RolFormPermission.</param>
        /// <returns>RolFormPermissionDTO.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolFormPermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolFormPermissionById(int id)
        {
            try
            {
                var record = await _rolFormPermissionBusiness.GetRolFormPermissionByIdAsync(id);
                return Ok(record);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el RolFormPermission con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener RolFormPermission con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo RolFormPermission.
        /// </summary>
        /// <param name="dto">Datos del RolFormPermission a crear.</param>
        /// <returns>Registro creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RolFormPermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolFormPermission([FromBody] RolFormPermissionCreateDTO dto)
        {
            try
            {
                var createdRecord = await _rolFormPermissionBusiness.CreateRolFormPermissionAsync(dto);
                return CreatedAtAction(nameof(GetRolFormPermissionById), new { id = createdRecord.Id }, createdRecord);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear RolFormPermission");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear RolFormPermission");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un registro de RolFormPermission existente.
        /// </summary>
        /// <param name="id">ID del registro a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>Resultado de la actualización.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolFormPermission(int id, [FromBody] RolFormPermissionCreateDTO dto)
        {
            try
            {
                var updated = await _rolFormPermissionBusiness.UpdateRolFormPermissionAsync(dto);
                return Ok(updated);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar RolFormPermission con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar RolFormPermission con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un registro de RolFormPermission por su ID.
        /// </summary>
        /// <param name="id">ID del registro a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRolFormPermission(int id)
        {
            try
            {
                await _rolFormPermissionBusiness.DeleteRolFormPermissionAsync(id);
                return Ok(new {message = "Se eliminó el registro"});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar RolFormPermission con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar RolFormPermission con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}