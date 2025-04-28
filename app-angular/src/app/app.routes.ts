import { Routes } from '@angular/router';
import { LandingComponent } from './landing-component/landing-component.component';
import { FormUserComponent } from './user/create-user/create-user.component';
import { IndiceUserComponent } from './user/indice-user/indice-user.component';
import { UpdateUserComponent } from './user/update-user/update-user.component';
import { IndiceModuleComponent } from './module/indice-module/indice-module.component';
import { UpdateModuleComponent } from './module/update-module/update-module.component';
import { CreateModuleComponent } from './module/create-module/create-module.component';
import { IndiceRolComponent } from './rol/indice-rol/indice-rol.component';
import { CreateRolComponent } from './rol/create-rol/create-rol.component';
import { UpdateRolComponent } from './rol/update-rol/update-rol.component';
import { IndiceFormComponent } from './form/indice-form/indice-form.component';
import { CreateFormComponent } from './form/create-form/create-form.component';
import { UpdateFormComponent } from './form/update-form/update-form.component';
import { CreatePermissionComponent } from './permission/create-permission/create-permission.component';
import { IndicePermissionComponent } from './permission/indice-permission/indice-permission.component';
import { UpdatePermissionComponent } from './permission/update-permission/update-permission.component';
import { CreatePersonComponent } from './person/create-person/create-person.component';
import { IndicePersonComponent } from './person/indice-person/indice-person.component';
import { UpdatePersonComponent } from './person/update-person/update-person.component';
import { CreateRolUserComponent } from './rol-user/create-rol-user/create-rol-user.component';
import { IndiceRolUserComponent } from './rol-user/indice-rol-user/indice-rol-user.component';
import { UpdateRolUserComponent } from './rol-user/update-rol-user/update-rol-user.component';
import { CreateRolFormPermissionComponent } from './rolFormPermission/create-rol-form-permission/create-rol-form-permission.component';
import { IndiceRolFormPermissionComponent } from './rolFormPermission/indice-rol-form-permission/indice-rol-form-permission.component';
import { UpdateRolFormPermissionComponent } from './rolFormPermission/update-rol-form-permission/update-rol-form-permission.component';
import { CreateFormModuleComponent } from './form-module/create-form-module/create-form-module.component';
import { IndiceFormModuleComponent } from './form-module/indice-form-module/indice-form-module.component';
import { UpdateFormModuleComponent } from './form-module/update-form-module/update-form-module.component';

export const routes: Routes = [
    {path: '', component:LandingComponent},

    // User
    {path: 'user', component:IndiceUserComponent},
    {path: 'user/create', component: FormUserComponent },
    {path: 'user/update/:id', component: UpdateUserComponent },

    // Model
    {path: 'module', component: IndiceModuleComponent},
    {path: 'module/create', component:  CreateModuleComponent},
    {path: 'module/update/:id', component: UpdateModuleComponent },

    // Rol
    {path: 'rol', component: IndiceRolComponent},
    {path: 'rol/create', component: CreateRolComponent },
    {path: 'rol/update/:id', component: UpdateRolComponent},

    // Formularios
    {path: 'form', component: IndiceFormComponent},
    {path: 'form/create', component: CreateFormComponent },
    {path: 'form/update/:id', component: UpdateFormComponent },

    // Permisos
    {path: 'permission', component: IndicePermissionComponent},
    {path: 'permission/create', component: CreatePermissionComponent },
    {path: 'permission/update/:id', component: UpdatePermissionComponent },

    //Person
    {path: 'person', component: IndicePersonComponent},
    {path: 'person/create', component: CreatePersonComponent },
    {path: 'person/update/:id', component: UpdatePersonComponent },

    // Rol-User
    {path: 'rol-user', component: IndiceRolUserComponent},
    {path: 'rol-user/create', component: CreateRolUserComponent },
    {path: 'rol-user/update/:id', component: UpdateRolUserComponent },

    // Form-Module
    {path: 'form-module', component: IndiceFormModuleComponent},
    {path: 'form-module/create', component: CreateFormModuleComponent },
    {path: 'form-module/update/:id', component: UpdateFormModuleComponent },

    // Rol-Form-Permission
    {path: 'rol-form-permission', component: IndiceRolFormPermissionComponent},
    {path: 'rol-form-permission/create', component: CreateRolFormPermissionComponent },
    {path: 'rol-form-permission/update/:id', component: UpdateRolFormPermissionComponent },

    
];
