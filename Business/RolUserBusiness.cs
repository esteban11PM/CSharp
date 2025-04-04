using System;
using System.Collections.Generic;
using System.Data.Common;
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
    ///Clase de negocio encargada de la lógica relacionada con los roles de usuario del sistema.
    ///</summary>
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger<RolUserBusiness> logger)
        {
            _rolUserData = rolUserData;
            _logger = logger;
        }

         /// <summary>
        /// Obtiene todos los RolUser como DTOs.
        /// </summary>
        public async Task<IEnumerable<RolUserDTO>> GetAllRolUsersAsync()
        {
            try
            {
                var rolesUsers = await _rolUserData.GetAllAsync();
                return MapToDTOList(rolesUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles de usuario", ex);
            }
        }

        // Método para obtener un RolUser por ID como DTO
        public async Task<RolUserDTO> GetRolUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un RolUser con ID inválido: {RolUserId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del RolUser debe ser mayor que cero");
            }

            try
            {
                var rolUser = await _rolUserData.GetByIdAsync(id);
                if (rolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún RolUser con ID: {RolUserId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }

                return MapToDTO(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolUser con ID {id}", ex);
            }
        }

        // Método para crear un RolUser desde un DTO
        public async Task<RolUserCreateDTO> CreateRolUserAsync(RolUserCreateDTO rolUserDTO)
        {
            try
            {
                // ValidateRolUser(rolUserDTO);

                var rolUser = MapCreateToEntity(rolUserDTO);
                var rolCreado = await _rolUserData.CreateAsync(rolUser);

                //Se reobtiene el usuario con persona
                var rolWithUser = (await _rolUserData.GetAllAsync()).FirstOrDefault(ru => ru.Id == rolCreado.Id);

                return MapToCreateDTO(rolWithUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo RolUser: {RolUserId}", rolUserDTO?.Id ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el RolUser", ex);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        public async Task<RolUserDTO> UpdateRolUserAsync(RolUserCreateDTO rolUserCreateDTO)
        {
            var existingRolUser = await _rolUserData.GetByIdAsync(rolUserCreateDTO.Id);
            if (existingRolUser == null)
            {
                throw new EntityNotFoundException("Rol", rolUserCreateDTO.Id);
            }
            try
            {
                // Actualizar propiedades
                existingRolUser.UserId = rolUserCreateDTO.UserId;
                existingRolUser.RoleId = rolUserCreateDTO.RoleId; 

                // Llamar al Data layer para actualizar
                var updated = await _rolUserData.UpdateAsync(existingRolUser);
                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "No se pudo actualizar el usuario", null);
                }

                // Vuelve a obtener el usuario actualizado incluyendo la información de Person
                    var userWithPerson = await _rolUserData.GetByIdAsync(rolUserCreateDTO.Id);
                    return MapToDTO(userWithPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", rolUserCreateDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }


        // Método para eliminar un RolUser por ID
        public async Task DeleteRolUserAsync(int id)
        {
            try
            {
                var rolUser = await _rolUserData.GetByIdAsync(id);
                if (rolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún RolUser con ID: {RolUserId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }

                await _rolUserData.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el RolUser", ex);
            }
        }

        // Método para validar el DTO
        // private void ValidateRolUser(RolUserDTO rolUserDTO)
        // {
        //     if (rolUserDTO == null)
        //     {
        //         throw new Utilities.Exceptions.ValidationException("El objeto RolUser no puede ser nulo");
        //     }
        // }

        /// <summary>
        /// Mapea un objeto RolUser a RolUserDTO.
        /// </summary>
        private RolUserDTO MapToDTO(RolUser rolUser)
        {
            return new RolUserDTO
            {
                Id = rolUser.Id,
                UserId = rolUser.UserId,
                UserName = rolUser.User?.Username,
                RoleId = rolUser.RoleId,
                RoleName = rolUser.Role?.Name
            };
        }

        /// <summary>
        /// Mapea un objeto RolUserDTO a RolUser.
        /// </summary>
        private RolUser MapToEntity(RolUserDTO rolUserDTO)
        {
            return new RolUser
            {
                Id = rolUserDTO.Id,
                UserId = rolUserDTO.UserId,
                RoleId = rolUserDTO.RoleId
            };
        }

        /// <summary>
        /// Mapea un objeto User a UserCreateDTO.
        /// </summary>
        private RolUserCreateDTO MapToCreateDTO(RolUser rolUser)
        {
            return new RolUserCreateDTO
            {
                Id = rolUser.Id,
                UserId = rolUser.UserId,
                RoleId = rolUser.RoleId
            };
        }

        /// <summary>
        /// Mapea un objeto UserCreateDTO a User.
        /// </summary>
        private RolUser MapCreateToEntity(RolUserCreateDTO rolUserCreate)
        {
            return new RolUser
            {
                Id = rolUserCreate.Id,
                UserId = rolUserCreate.UserId,
                RoleId = rolUserCreate.RoleId
            };
        }

        /// <summary>
        /// Metodo para mapear una lista de User a una lista de UserDTO 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private IEnumerable<RolUserDTO> MapToDTOList(IEnumerable<RolUser> rolesUsers)
        {
            return rolesUsers.Select(MapToDTO);
        }
    }
}
