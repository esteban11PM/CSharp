using Business.Services;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FormModuleController : ControllerBase
    {
        private readonly ModuleFormBusiness _formModuleBusiness;
        private readonly ILogger<FormModuleController> _logger;
        private readonly ApplicationDbContext _context;

        public FormModuleController(ApplicationDbContext context,  ModuleFormBusiness formModuleBusiness, ILogger<FormModuleController> logger)
        {
            _context = context;
            _formModuleBusiness = formModuleBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _formModuleBusiness.GetAllAsync();
                return Ok(result);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _formModuleBusiness.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT
        [HttpPost]
        [ProducesResponseType(typeof(ModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] FormModuleCreateDTO formModule)
        {
            try
            {
                var created = await _formModuleBusiness.CreateAsyncNew(formModule);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update([FromBody] FormModuleCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _formModuleBusiness.UpdateNew(model);
                return Ok(updated);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el Module con ID: {ModuleId}", model.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", model.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el Module con ID: {ModuleId}", model.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE=> PERSISTENT
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _formModuleBusiness.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Module no encontrado con ID: {ModuleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Module con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PATCH => LOGICAL
        [HttpPatch("Logical/{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LogicalDelete(int id)
        {
            try
            {
                var module = await _context.Set<FormModule>().FirstOrDefaultAsync(r => r.Id == id);

                if (module == null)
                {
                    _logger.LogInformation("Module no encontrado con ID: {ModuleId}", id);
                    return NotFound(new { message = "Module no encontrado" });
                }

                module.Active = false;
                _context.Set<FormModule>().Update(module);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Module desactivado correctamente" });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al desactivar el Module con ID: {ModuleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al desactivar el Module con ID: {ModuleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}