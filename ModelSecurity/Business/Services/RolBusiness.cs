using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RolBusiness : GenericBusiness<Rol, RolDTO>
    {
        public RolBusiness(IDataFactoryGlobal factory, ILogger<Rol> logger, IMapper mapper) : base(factory.CreateRolData(), logger, mapper)
        {

        }

        protected override void Validate(RolDTO rol)
        {
            if (rol == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(rol.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }
    }
}


