using Data.Interfaces;
using Data.Repositories.Global;
using Entity.Context;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.Factories
{
    public class GlobalFactory : IDataFactoryGlobal
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        public GlobalFactory(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }
        public CrudBase<Person> CreatePersonData()
        {
            var logger = _loggerFactory.CreateLogger<Person>();
            return new PersonData(_context, logger);
        }
        public CrudBase<Rol> CreateRolData()
        {
            var logger = _loggerFactory.CreateLogger<Rol>();
            return new RolData(_context, logger);
        }
        public CrudBase<Form> CreateFormData()
        {
            var logger = _loggerFactory.CreateLogger<Form>();
            return new FormData(_context, logger);
        }

        public CrudBase<Module> CreateModuleData()
        {
            var logger = _loggerFactory.CreateLogger<Module>();
            return new ModuleData(_context, logger);
        }

        public CrudBase<FormModule> CreateModuleFormData()
        {
            var logger = _loggerFactory.CreateLogger<FormModule>();
            return new ModuleFormData(_context, logger);
        }

        public CrudBase<User> CreateUserData()
        {
            var logger = _loggerFactory.CreateLogger<User>();
            return new UserData(_context, logger);
        }

        public CrudBase<RolUser> CreateUserRolData()
        {
            var logger = _loggerFactory.CreateLogger<RolUser>();
            return new UserRolData(_context, logger);
        }

        public CrudBase<RolFormPermission> CreateRolFormPermissionData()
        {
            var logger = _loggerFactory.CreateLogger<RolFormPermission>();
            return new RolFormPermissionData(_context, logger);
        }

        public CrudBase<Permission> CreatePermissionData()
        {
            var logger = _loggerFactory.CreateLogger<Permission>();
            return new PermissionData(_context, logger);
        }
    }
}
