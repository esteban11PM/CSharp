using Entity.Context;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class ModuleData : GenericData<Module>
    {
        public ModuleData(ApplicationDbContext context, ILogger<Module> logger) : base(context, logger)
        {
            
        }
    }
}


