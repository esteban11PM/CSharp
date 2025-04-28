using Business;
using Business.Services;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;


namespace Web.ContPersonlers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly PersonBusiness _PersonBusiness;
        private readonly ILogger<PersonController> _logger;

        private readonly ApplicationDbContext _context;

        /// Constructor del contPersonador de permisos
        public PersonController(ApplicationDbContext context, PersonBusiness PersonBusiness, ILogger<PersonController> logger)
        {
            _context = context;
            _PersonBusiness = PersonBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var Persons = await _PersonBusiness.GetAllAsync();
                return Ok(Persons);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {
                var Person = await _PersonBusiness.GetByIdAsync(id);
                return Ok(Person);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAvailablePersons()
        {
            try
            {
                var availablePersons = await _PersonBusiness.GetAvailableAsync();
                return Ok(availablePersons);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener personas disponibles para usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT 
        [HttpPost]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO PersonDto)
        {
            try
            {
                var createdPerson = await _PersonBusiness.CreateAsync(PersonDto);
                return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
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

        // UPDATE
        [HttpPut("update")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonDTO PersonDto)
        {
            try
            {
                var update = await _PersonBusiness.UpdateAsync(PersonDto);
                return Ok(update);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el Person con ID: {PersonId}", PersonDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Person no encontrado con ID: {PersonId}", PersonDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el Person con ID: {PersonId}", PersonDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //// DELETE => PERSISTENT
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var response = await _PersonBusiness.DeleteAsync(id);
                return Ok(response); // Código 204: Eliminación exitosa sin contenido
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el Person con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Person no encontrado con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Person con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE => LOGICAL
        [HttpPatch("Logical/{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchLogicaPerson(int id)
        {
            try
            {
                var person = await _context.Set<Person>().FirstOrDefaultAsync(r => r.Id == id);

                if (person == null)
                {
                    _logger.LogInformation("Person no encontrado con ID: {PersonId}", id);
                    return NotFound(new { message = "Person no encontrado" });
                }

                person.Status = false;
                _context.Set<Person>().Update(person);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Person desactivado correctamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al desactivar el Person con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al desactivar el Person con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
