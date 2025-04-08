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
    /// Controlador para la gestión de asignaciones de roles a usuarios.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolUserController : ControllerBase
    {
        private readonly RolUserBusiness _rolUserBusiness;
        private readonly ILogger<RolUserController> _logger;

        /// <summary>
        /// Constructor del controlador de RolUser.
        /// </summary>
        /// <param name="rolUserBusiness">Capa de negocio de RolUser.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public RolUserController(RolUserBusiness rolUserBusiness, ILogger<RolUserController> logger)
        {
            _rolUserBusiness = rolUserBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las asignaciones de roles a usuarios.
        /// </summary>
        /// <returns>Lista de RolUserDTO.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolUserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolUsers()
        {
            try
            {
                var list = await _rolUserBusiness.GetAllRolUsersAsync();
                return Ok(list);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las asignaciones de roles a usuarios.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una asignación de rol a usuario por su ID.
        /// </summary>
        /// <param name="id">ID del RolUser.</param>
        /// <returns>RolUserDTO.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolUserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserById(int id)
        {
            try
            {
                var record = await _rolUserBusiness.GetRolUserByIdAsync(id);
                return Ok(record);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el RolUser con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener RolUser con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva asignación de rol a usuario.
        /// </summary>
        /// <param name="dto">Datos del RolUser a crear.</param>
        /// <returns>Registro creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RolUserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolUser([FromBody] RolUserCreateDTO rolUserCreateDTO)
        {
            try
            {
                var createdRecord = await _rolUserBusiness.CreateRolUserAsync(rolUserCreateDTO);
                return CreatedAtAction(nameof(GetRolUserById), new { id = createdRecord.Id }, createdRecord);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear RolUser.");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear RolUser.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una asignación de rol a usuario existente.
        /// </summary>
        /// <param name="id">ID del registro a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>Resultado de la actualización.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolUser([FromBody] RolUserCreateDTO dto)
        {
            try
            {
                var updated = await _rolUserBusiness.UpdateRolUserAsync(dto);
                return Ok(updated);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar RolUser con ID: {Id}", dto);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {Id}", dto);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar RolUser con ID: {Id}", dto);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una asignación de rol a usuario por su ID.
        /// </summary>
        /// <param name="id">ID del registro a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRolUser(int id)
        {
            try
            {
                await _rolUserBusiness.DeleteRolUserAsync(id);
                return Ok(new {message = "Se eliminó el registro"});
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar RolUser con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolUser no encontrado con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar RolUser con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina de manera logica un formModule del sistema
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Logical/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogicalRolUserAsync(int id)
        {
            try
            {
                var deleted = await _rolUserBusiness.DeleteRolUserLogicalAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = "RolUSer no encontrado o ya eliminado." });
                }

                return Ok(new { message = "Eliminación lógica exitosa." });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el RolUser con ID: {RolUserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolUser de manera lógica con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
