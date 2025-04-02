using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    ///<summary>
    ///Clase de negocio encargada de la logica relacionada con los Usuarios del sistema;
    ///</summary>
    public class UserBusiness
    {
        private readonly UserData _userData;
        private readonly ILogger<UserBusiness> _logger;

        public UserBusiness(UserData userData, ILogger<UserBusiness> logger)
        {
            _userData = userData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios como DTOs.
        /// </summary>
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userData.GetAllAsync();
                return MapToDTOList(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por ID como DTO.
        /// </summary>
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UserId}", id);
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }

            try
            {
                var user = await _userData.GetByIdAsync(id);
                return user == null
                    ? throw new EntityNotFoundException("Usuario", id)
                    : MapToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en la entidad.
        /// </summary>
        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            try
            {
                ValidateUser(userDTO);

                var user = MapToEntity(userDTO);
                var userCreated = await _userData.CreateAsync(user);

                return MapToDTO(userCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {Username}", userDTO?.Username ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente de la entidad.
        /// </summary>
        public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                ValidateUser(userDTO);

                var existingUser = await _userData.GetByIdAsync(userDTO.Id);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("Usuario", userDTO.Id);
                }
                //Actualiza las porpiedades de la entidad
                existingUser.Username = userDTO.Username;
                existingUser.State = userDTO.State;

                return await _userData.UpdateAsync(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", userDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario de la entidad.
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un usuario con ID inválido: {UserId}", id);
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }

            try
            {
                return await _userData.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el usuario con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida los datos del usuario.
        /// </summary>
        private void ValidateUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ValidationException("El objeto usuario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(userDTO.Username))
            {
                _logger.LogWarning("Se intentó crear/actualizar un usuario con nombre vacío");
                throw new ValidationException("Username", "El nombre de usuario es obligatorio");
            }
        }

        /// <summary>
        /// Mapea un objeto User a UserDTO.
        /// </summary>
        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                State = user.State
            };
        }

        /// <summary>
        /// Mapea un objeto UserDTO a User.
        /// </summary>
        private User MapToEntity(UserDTO userDTO)
        {
            return new User
            {
                Id = userDTO.Id,
                Username = userDTO.Username,
                State = userDTO.State
            };
        }

        /// <summary>
        /// Metodo para mapear una lista de User a una lista de UserDTO 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private IEnumerable<UserDTO> MapToDTOList(IEnumerable<User> users)
        {
            return users.Select(MapToDTO);
        }
    }
}
