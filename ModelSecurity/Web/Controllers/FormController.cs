using Business.Services;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormBusiness _formBusiness;
        private readonly ILogger<FormController> _logger;
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context, FormBusiness formBusiness, ILogger<FormController> logger)
        {
            _context = context;
            _formBusiness = formBusiness;
            _logger = logger;
        }

        // GET: api/Form
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<FormDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var forms = await _formBusiness.GetAllAsync();
                return Ok(forms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formularios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Form/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var form = await _formBusiness.GetByIdAsync(id);
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

        // POST: api/Form
        [HttpPost]
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDTO formDto)
        {
            try
            {
                var createdForm = await _formBusiness.CreateAsync(formDto);
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

        // PUT: api/Form/update
        [HttpPut("update")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateForm([FromBody] FormDTO formDto)
        {
            try
            {
                var updatedForm = await _formBusiness.UpdateAsync(formDto);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el formulario con ID: {FormId}", formDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", formDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el formulario con ID: {FormId}", formDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/Form/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var response = await _formBusiness.DeleteAsync(id);
                return Ok(new { message = "Formulario eliminado de la base correctamente" });

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PATCH: api/Form/Logical/5
        [HttpPatch("Logical/{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchLogicalForm(int id)
        {
            try
            {
                var form = await _context.Set<Form>().FirstOrDefaultAsync(f => f.Id == id);

                if (form == null)
                {
                    _logger.LogInformation("Formulario no encontrado con ID: {FormId}", id);
                    return NotFound(new { message = "Formulario no encontrado" });
                }

                form.Active = false;
                _context.Set<Form>().Update(form);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Formulario desactivado correctamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al desactivar el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al desactivar el formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}