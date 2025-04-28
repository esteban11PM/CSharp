using Data.Repositories.Global;
using Entity.Context;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class FormData: GenericData<Form>
    {
        public FormData(ApplicationDbContext context, ILogger<Form> logger) : base(context, logger)
        {

        }
    }
}


