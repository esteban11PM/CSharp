using Data.Interfaces;
using Entity.Model;

namespace Data.Factories
{
    public interface IDataFactoryGlobal
    {
        CrudBase<Person> CreatePersonData();
        CrudBase<Rol> CreateRolData();
        CrudBase<Form> CreateFormData();

        CrudBase<Module> CreateModuleData();

        CrudBase<FormModule> CreateModuleFormData();

        CrudBase<User> CreateUserData();
        CrudBase<RolUser> CreateUserRolData();
        CrudBase<RolFormPermission> CreateRolFormPermissionData();
        CrudBase<Permission> CreatePermissionData();

    }
}
