using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ModuleBusiness : GenericBusiness<Module, ModuleDTO>
    {
        public ModuleBusiness(IDataFactoryGlobal factory, ILogger<Module> logger, IMapper mapper) : base(factory.CreateModuleData(), logger, mapper)
        {

        }

        protected override void Validate(ModuleDTO moduleDto)
        {
            if (moduleDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(moduleDto.Name))
                throw new ValidationException("El título del formulario es obligatorio.");
        }
    }

}

