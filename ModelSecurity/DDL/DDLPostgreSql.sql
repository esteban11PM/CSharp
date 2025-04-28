-- Eliminamos las tablas. Se usa CASCADE para que se eliminen las dependencias y no genere errores.
DROP TABLE IF EXISTS "FormModule" CASCADE;
DROP TABLE IF EXISTS "RolFormPermission" CASCADE;
DROP TABLE IF EXISTS "Module" CASCADE;
DROP TABLE IF EXISTS "Form" CASCADE;
DROP TABLE IF EXISTS "Permission" CASCADE;
DROP TABLE IF EXISTS "RolUser" CASCADE;
DROP TABLE IF EXISTS "User" CASCADE;
DROP TABLE IF EXISTS "Rol" CASCADE;
DROP TABLE IF EXISTS "Person" CASCADE;

-- Tabla Person
CREATE TABLE "Person" (
    Id SERIAL,
    Name VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    DocumentNumber VARCHAR(10) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Address VARCHAR(100) NOT NULL,
    DocumentType CHAR(3) NOT NULL,
    BlodType CHAR(3) NOT NULL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (Id)
);

-- Tabla User vinculada con Person
CREATE TABLE "User" (
    Id SERIAL,
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PersonId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (PersonId) REFERENCES "Person"(Id) ON DELETE CASCADE
);

-- Tabla Rol
CREATE TABLE "Rol" (
    Id SERIAL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(200),
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (Id)
);

-- Tabla pivote entre Rol y User
CREATE TABLE "RolUser" (
    Id SERIAL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (UserId) REFERENCES "User"(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES "Rol"(Id) ON DELETE CASCADE
);

-- Tabla Permission
CREATE TABLE "Permission" (
    Id SERIAL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(200) NOT NULL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (Id)
);

-- Tabla Form
CREATE TABLE "Form" (
    Id SERIAL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(200) NOT NULL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (Id)
);

-- Tabla Module
CREATE TABLE "Module" (
    Id SERIAL,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(200),
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (Id)
);

-- Tabla pivote RolFormPermission
CREATE TABLE "RolFormPermission" (
    Id SERIAL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    RolId INT NOT NULL,
    PermissionId INT NOT NULL,
    FormId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (RolId) REFERENCES "Rol"(Id) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES "Permission"(Id) ON DELETE CASCADE,
    FOREIGN KEY (FormId) REFERENCES "Form"(Id) ON DELETE CASCADE
);

-- Tabla pivote FormModule
CREATE TABLE "FormModule" (
    Id SERIAL,
    Active BOOLEAN NOT NULL DEFAULT TRUE,
    FormId INT NOT NULL,
    ModuleId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (FormId) REFERENCES "Form"(Id) ON DELETE CASCADE,
    FOREIGN KEY (ModuleId) REFERENCES "Module"(Id) ON DELETE CASCADE
);
