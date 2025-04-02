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
    public class RolFormPermissionBusiness
    {
        private readonly RolFormPermissionData _rolFormPermissionData;
        private readonly ILogger<RolFormPermissionBusiness> _logger;

        public RolFormPermissionBusiness(RolFormPermissionData rolFormPermissionData, ILogger<RolFormPermissionBusiness> logger)
        {
            _rolFormPermissionData = rolFormPermissionData;
            _logger = logger;
        }
        
        /// <summary>
        /// Obtiene todos los campos de RolFormPermission como DTOs.
        /// </summary>
        public async Task<IEnumerable<RolFormPermissionDTO>> GetAllRolFormPermissionsAsync()
        {
            try
            {
                var entities = await _rolFormPermissionData.GetAllAsync();
                return MapToDTOList(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de formulario de rol.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar los permisos", ex);
            }
        }
        
        /// <summary>
        /// Obtiene un RolFormPermission por ID como DTO.
        /// </summary>
        public async Task<RolFormPermissionDTO> GetRolFormPermissionByIdAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id", "El ID debe ser mayor que cero");

            try
            {
                var entity = await _rolFormPermissionData.GetByIdAsync(id);
                if (entity == null) throw new EntityNotFoundException("RolFormPermission", id);

                return MapToDTO(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }
        
        /// <summary>
        /// Crea un nuevo registro de RolFormPermission.
        /// </summary>
        public async Task<RolFormPermissionDTO> CreateRolFormPermissionAsync(RolFormPermissionDTO dto)
        {
            ValidateRolFormPermission(dto);

            try
            {
                var entity = MapToEntity(dto);
                var createdEntity = await _rolFormPermissionData.CreateAsync(entity);
                return MapToDTO(createdEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso de formulario de rol.");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }
        
        /// <summary>
        /// Actualiza un registro existente de RolFormPermission.
        /// </summary>
        public async Task<bool> UpdateRolFormPermissionAsync(RolFormPermissionDTO dto)
        {
            ValidateRolFormPermission(dto);

            try
            {
                var existingEntity = await _rolFormPermissionData.GetByIdAsync(dto.Id);
                if (existingEntity == null) throw new EntityNotFoundException("RolFormPermission", dto.Id);

                existingEntity.RoleId = dto.RoleId;
                existingEntity.PermissionId = dto.PermissionId;
                existingEntity.FormId = dto.FormId;

                return await _rolFormPermissionData.UpdateAsync(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID: {Id}", dto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el permiso con ID {dto.Id}", ex);
            }
        }

        /// <summary>
        /// Elimina un RolFormPermission por ID.
        /// </summary>
        public async Task DeleteRolFormPermissionAsync(int id)
        {
            if (id <= 0) throw new ValidationException("id", "El ID debe ser mayor que cero");

            try
            {
                var entity = await _rolFormPermissionData.GetByIdAsync(id);
                if (entity == null) throw new EntityNotFoundException("RolFormPermission", id);

                await _rolFormPermissionData.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el permiso con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida los datos de RolFormPermission.
        /// </summary>
        private void ValidateRolFormPermission(RolFormPermissionDTO dto)
        {
            if (dto == null) throw new ValidationException("El objeto no puede ser nulo");
            if (dto.RoleId <= 0) throw new ValidationException("RoleId", "El RoleId debe ser mayor que cero");
            if (dto.PermissionId <= 0) throw new ValidationException("PermissionId", "El PermissionId debe ser mayor que cero");
            if (dto.FormId <= 0) throw new ValidationException("FormId", "El FormId debe ser mayor que cero");
        }

        /// <summary>
        /// Mapea un objeto RolFormPermission a RolFormPermissionDTO.
        /// </summary>
        private RolFormPermissionDTO MapToDTO(RolFormPermission entity)
        {
            return new RolFormPermissionDTO
            {
                Id = entity.Id,
                RoleId = entity.RoleId,
                RoleName = entity.Rol?.Name,
                PermissionId = entity.PermissionId,
                PermissionName = entity.Permission?.Name,
                FormId = entity.FormId,
                FormName = entity.Form?.Name
            };
        }

        /// <summary>
        /// Mapea un objeto RolFormPermissionDTO a RolFormPermission.
        /// </summary>
        private RolFormPermission MapToEntity(RolFormPermissionDTO dto)
        {
            return new RolFormPermission
            {
                Id = dto.Id,
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId,
                FormId = dto.FormId
            };
        }

        /// <summary>
        /// Metodo para mapear una lista de RolFormPermission a una lista de RolFormPermissionDTO 
        /// </summary>
        /// <param name="rolFormPermissions"></param>
        /// <returns></returns>
        private IEnumerable<RolFormPermissionDTO> MapToDTOList(IEnumerable<RolFormPermission> rolFormPermissions)
        {
            var rolFormPermissionsDTO = new List<RolFormPermissionDTO>();
            foreach (var rolFormPermission in rolFormPermissions)
            {
                rolFormPermissionsDTO.Add(MapToDTO(rolFormPermission));
            }
            return rolFormPermissionsDTO;
        }
    }
}
