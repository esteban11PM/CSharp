using System.ComponentModel;
using Data.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class GenericData<T> : CrudBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<T> _logger;

        public GenericData(ApplicationDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Recupera todos los datos
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>()
                            .Where(s => EF.Property<bool>(s, "Active") == true)
                            .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "No se logró obtener a las formas");
                throw;
            }
        }

        // Recupera los datos por ID
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una forma por Id {id}");
                throw;
            }
        }

        // Inserta nuevos datos
        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"No se pudo insertar la nueva forma {entity}");
                throw;
            }
        }

        // Actualiza datos
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);

                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {entity}");
                throw;
            }
        }

        // Elimina de manera permanente
        public virtual async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                var Delete = await GetByIdAsync(id);
                if (Delete == null) return new {Active = false};

                _context.Set<T>().Remove(Delete);
                await _context.SaveChangesAsync();
                return new {Active = true};
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, $" Error al eiminar {ex.Message}");
                return false;
            }
        }

        // Eliminar de manera lógica
        public virtual async Task<Object> DeleteLogicalAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);

                if (entity == null) return new {Active = false};

                await _context.SaveChangesAsync();
                return new {Active = true};
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error al realizar la eliminación logica con LINQ {ex.Message}");
                return false;
            }
        }
    }    
}


