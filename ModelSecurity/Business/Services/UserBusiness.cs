using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Business.Services
{
    public class UserBusiness : GenericBusiness<User, UserDTO>
    {
        public UserBusiness(IDataFactoryGlobal factory, ILogger<User> logger, IMapper mapper) : base(factory.CreateUserData(), logger, mapper)
        {

        }


        public async Task<UserCreateDTO> CreateAsyncNew(UserCreateDTO dtoExp)
        {
            try
            {
                Validate(dtoExp);
                var entity = _mapper.Map<User>(dtoExp);
                var created = await _data.CreateAsync(entity);
                return _mapper.Map<UserCreateDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el UserRol {dtoExp.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al crear {dtoExp.Id}", ex);
            }
        }

        public async Task<UserCreateDTO> UpdateNew(UserCreateDTO dtoExp)
        {
            if (dtoExp == null)
                throw new ValidationException("Entidad", $"{dtoExp.Id} no puede ser nulo");

            try
            {
                Validate(dtoExp);


                if (dtoExp.Id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var entity = _mapper.Map<User>(dtoExp);
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

        protected override void Validate(UserDTO user)
        {
            if (user == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            // if (string.IsNullOrWhiteSpace(user.Username))
            //     throw new ValidationException("El título del formulario es obligatorio.");
        }

        protected void Validate(UserCreateDTO userRolDto)
        {
            if (userRolDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");
        }
    }
}


