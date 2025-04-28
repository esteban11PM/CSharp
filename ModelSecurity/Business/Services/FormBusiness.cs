using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.Factories;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class FormBusiness : GenericBusiness<Form, FormDTO>
    {
        public FormBusiness(IDataFactoryGlobal factory, ILogger<Form> logger, IMapper mapper) : base(factory.CreateFormData(), logger, mapper)
        {

        } 

        protected override void Validate(FormDTO form)
        {
            if (form == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(form.Name))
                throw new ValidationException("El título del formulario es obligatorio.");
        }
    }
}


