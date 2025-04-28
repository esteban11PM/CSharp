using Entity.Context;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Global
{
    public class RolData : GenericData<Rol>
    {
        public RolData(ApplicationDbContext context, ILogger<Rol> logger) : base(context, logger)
        {
            
        }
    }
}


