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

        // Método para obtener todos los RolUser como DTOs
        public async Task<IEnumerable<RolUserDTO>> GetAllRolUsersAsync()
        {
            try
            {
                var rolesUsers = await _rolUserData.GetAllAsync();
                return rolesUsers.Select(MapToDTO);
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
        public async Task<RolUserDTO> CreateRolUserAsync(RolUserDTO rolUserDTO)
        {
            try
            {
                ValidateRolUser(rolUserDTO);

                var rolUser = MapToEntity(rolUserDTO);
                var rolCreado = await _rolUserData.CreateAsync(rolUser);

                return MapToDTO(rolCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo RolUser: {RolUserId}", rolUserDTO?.Id ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el RolUser", ex);
            }
        }

        // Método para actualizar un RolUser desde un DTO
        public async Task<bool> UpdateRolUserAsync(RolUserDTO rolUserDTO)
        {
            try
            {
                ValidateRolUser(rolUserDTO);

                var rolUser = new RolUser
                {
                    Id = rolUserDTO.Id,
                    UserId = rolUserDTO.UserId,
                    RoleId = rolUserDTO.RoleId
                };

                var actualizado = await _rolUserData.UpdateAsync(rolUser);

                if (!actualizado)
                {
                    _logger.LogWarning("No se pudo actualizar el RolUser con ID: {RolUserId}", rolUserDTO.Id);
                    throw new ExternalServiceException("Base de datos", $"No se pudo actualizar el RolUser con ID {rolUserDTO.Id}");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RolUser con ID: {RolUserId}", rolUserDTO.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el RolUser con ID {rolUserDTO.Id}", ex);
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
        private void ValidateRolUser(RolUserDTO rolUserDTO)
        {
            if (rolUserDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto RolUser no puede ser nulo");
            }
        }

        // Método para mapear el RolUser a RolUserDTO
        private RolUserDTO MapToDTO(RolUser rolUser)
        {
            return new RolUserDTO
            {
                Id = rolUser.Id,
                UserId = rolUser.UserId,
                RoleId = rolUser.RoleId
            };
        }

        // Método para mapear de RolUserDTO a RolUser
        private RolUser MapToEntity(RolUserDTO rolUserDTO)
        {
            return new RolUser
            {
                Id = rolUserDTO.Id,
                UserId = rolUserDTO.UserId,
                RoleId = rolUserDTO.RoleId
            };
        }
    }
}
