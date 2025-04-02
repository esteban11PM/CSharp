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
    /// Controlador para la gestión de personas en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly PersonBusiness _personBusiness;
        private readonly ILogger<PersonController> _logger;

        /// <summary>
        /// Constructor del controlador de personas.
        /// </summary>
        /// <param name="personBusiness">Capa de negocio de personas.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public PersonController(PersonBusiness personBusiness, ILogger<PersonController> logger)
        {
            _personBusiness = personBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las personas registradas en el sistema.
        /// </summary>
        /// <returns>Lista de personas.</returns>
        /// <response code="200">Retorna la lista de personas.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var persons = await _personBusiness.GetAllPersonsAsync();
                return Ok(persons);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de personas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una persona específica por su ID.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <returns>Persona solicitada.</returns>
        /// <response code="200">Retorna la persona solicitada.</response>
        /// <response code="400">ID proporcionado no válido.</response>
        /// <response code="404">Persona no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {
                var person = await _personBusiness.GetPersonByIdAsync(id);
                return Ok(person);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la persona con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener persona con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva persona en el sistema.
        /// </summary>
        /// <param name="personDto">Datos de la persona a crear.</param>
        /// <returns>Persona creada.</returns>
        /// <response code="201">Retorna la persona creada.</response>
        /// <response code="400">Datos de la persona no válidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO personDto)
        {
            try
            {
                var createdPerson = await _personBusiness.CreatePersonAsync(personDto);
                return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear persona");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear persona");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de una persona existente.
        /// </summary>
        /// <param name="id">ID de la persona a actualizar.</param>
        /// <param name="personDto">Datos actualizados de la persona.</param>
        /// <returns>Persona actualizada.</returns>
        /// <response code="200">Persona actualizada correctamente.</response>
        /// <response code="400">Datos no válidos.</response>
        /// <response code="404">Persona no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonDTO personDto)
        {
            try
            {
                var updatedPerson = await _personBusiness.UpdatePersonAsync(personDto); // Cuidado en el id ingresado porque en el business no se espera ese Id
                return Ok(updatedPerson);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar persona con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar persona con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina una persona existente por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        /// <returns>Resultado de la eliminación.</returns>
        /// <response code="204">La persona se eliminó correctamente.</response>
        /// <response code="400">ID no válido.</response>
        /// <response code="404">Persona no encontrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                await _personBusiness.DeletePersonAsync(id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar persona con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar persona con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}