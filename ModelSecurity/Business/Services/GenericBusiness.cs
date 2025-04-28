
using AutoMapper;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public abstract class GenericBusiness<T, Dto> where T : class
    {
        protected readonly CrudBase<T> _data;
        protected readonly ILogger<T> _logger;
        protected readonly IMapper _mapper;

        public GenericBusiness(CrudBase<T> data, ILogger<T> logger, IMapper mapper)
        {
            _data = data;
            _logger = logger;
            _mapper = mapper;
        }

         // QUERY ALL
        public virtual async Task<IEnumerable<Dto>> GetAllAsync()
        {
            try
            {
                var list = await _data.GetAllAsync();
                return _mapper.Map<IEnumerable<Dto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todos los {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al obtener todos los {typeof(T).Name}", ex);
            }
        }

        // Query By Id
        public virtual async Task<Dto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero");

            try
            {
                var entity = await _data.GetByIdAsync(id);
                if (entity == null)
                    throw new EntityNotFoundException(typeof(T).Name, id);

                return _mapper.Map<Dto>(entity); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener {typeof(T).Name} con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al obtener {typeof(T).Name}", ex);
            }
        }

        // INSERT
        public virtual async Task<Dto> CreateAsync(Dto entityDto)
        {
            try
            {
                Validate(entityDto);
                var entity = _mapper.Map<T>(entityDto); 
                var created = await _data.CreateAsync(entity);
                return _mapper.Map<Dto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al crear {typeof(T).Name}", ex);
            }
        }

       // UPDATE
        public virtual async Task<Object> UpdateAsync(Dto dto)
        {
            if (dto == null)
                throw new ValidationException("Entidad", $"{typeof(T).Name} no puede ser nulo");

            try
            {
                Validate(dto);

                var idProp = typeof(Dto).GetProperty("Id")?.GetValue(dto);
                if (idProp == null || (int)idProp <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var entity = _mapper.Map<T>(dto); 
                var updated = await _data.UpdateAsync(entity);

                if (!updated)
                    throw new ExternalServiceException("Base de datos", $"No se pudo actualizar {typeof(T).Name}");

                return new { rowAfects = updated };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar {typeof(T).Name}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar {typeof(T).Name}", ex);
            }
        }

        
        // DELETE PERSISTENT
        public virtual async Task<Object> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero");

            try
            {
                var deleted = await _data.DeletePersistentAsync(id);
                if (deleted == null)
                    throw new ExternalServiceException("Base de datos", $"No se pudo eliminar {typeof(T).Name}");

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar {typeof(T).Name} con ID {id}");
                throw new ExternalServiceException("Base de datos", $"Error al eliminar {typeof(T).Name} con ID {id}", ex);
            }
        }

        protected abstract void Validate(Dto entity);
    }
}

