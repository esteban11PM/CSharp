using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class ModuleFormBusiness : GenericBusiness<FormModule, FormModuleDTO>
    {
        public ModuleFormBusiness(IDataFactoryGlobal factory, ILogger<FormModule> logger, IMapper mapper) : base(factory.CreateModuleFormData(), logger, mapper)
        {

        }
        protected override void Validate(FormModuleDTO moduleFormDto)
        {
            if (moduleFormDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            //if (string.IsNullOrWhiteSpace(moduleFormDto.Name))
            //    throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }

        protected void Validate(FormModuleCreateDTO moduleFormDto)
        {
            if (moduleFormDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            //if (string.IsNullOrWhiteSpace(moduleFormDto.Name))
            //    throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }

        public async Task<FormModuleCreateDTO> CreateAsyncNew(FormModuleCreateDTO dtoExp)
        {
            try
            {
                Validate(dtoExp);
                var entity = _mapper.Map<FormModule>(dtoExp);
                var created = await _data.CreateAsync(entity);
                return _mapper.Map<FormModuleCreateDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el ModuleForm { dtoExp.Id }");
                throw new ExternalServiceException("Base de datos", $"Error al crear { dtoExp.Id }", ex);
            }
        }

        public async Task<FormModuleCreateDTO> UpdateNew(FormModuleCreateDTO dtoExp)
        {
            if (dtoExp == null)
                throw new ValidationException("Entidad", $"{dtoExp.Id} no puede ser nulo");

            try
            {
                Validate(dtoExp);

                if (dtoExp.Id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var entity = _mapper.Map<FormModule>(dtoExp);
                var updated = await _data.UpdateAsync(entity);

                if (!updated)
                    throw new ExternalServiceException("Base de datos", $"No se pudo actualizar {dtoExp.Id}");

                return dtoExp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar {dtoExp.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar {dtoExp.Id}", ex);
            }
        }
    }
}


