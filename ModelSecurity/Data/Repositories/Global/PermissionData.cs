using Entity;
using Entity.Context;
using Entity.Model;
using Microsoft.Extensions.Logging;
namespace Data.Repositories.Global
{
    public class PermissionData : GenericData<Permission>
    {
        public PermissionData(ApplicationDbContext context, ILogger<Permission> logger) : base(context, logger)
        {

        }
    }
}