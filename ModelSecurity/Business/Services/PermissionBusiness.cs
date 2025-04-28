using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class PermissionBusiness : GenericBusiness<Permission, PermissionDTO>
    {
        public PermissionBusiness(IDataFactoryGlobal factory, ILogger<Permission> logger, IMapper mapper) : base(factory.CreatePermissionData(), logger, mapper)
        {

        }

        protected override void Validate(PermissionDTO permission)
        {
            if (permission == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(permission.Name))
                throw new ValidationException("El título del formulario es obligatorio.");
        }
    }
}


