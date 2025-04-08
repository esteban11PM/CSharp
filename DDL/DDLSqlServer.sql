-- Creación para la tabla Persona con relación con User
CREATE TABLE Person (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(30) NOT NULL,
    LastName NVARCHAR(30) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    DocumentNumber NVARCHAR(10) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(100) NOT NULL,
    DocumentType CHAR(3) NOT NULL,
    BlodType CHAR(3) NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
);


-- Creación para la tabla User con relación con Person y Rol
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    PersonId INT NOT NULL,
    CONSTRAINT FK_User_Person FOREIGN KEY (PersonId) REFERENCES Person(Id) ON DELETE CASCADE
);

-- Creación de la tabla Rol con relacion con User, permission y form
CREATE TABLE Rol (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NULL,
    Active BIT NOT NULL DEFAULT 1
);

-- Creación de la tabla pivote entre Rol y User
CREATE TABLE RolUser (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Active BIT NOT NULL DEFAULT 1,
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_RolUser_User FOREIGN KEY (UserId) REFERENCES [User](Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolUser_Rol FOREIGN KEY (RoleId) REFERENCES Rol(Id) ON DELETE CASCADE
);

-- Creación de la tabla Permission con relación con rol y form
CREATE TABLE Permission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    Active BIT NOT NULL DEFAULT 1
);

-- Creación de la entidad Form relacionada con rol, permission y module
CREATE TABLE Form (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    Active BIT NOT NULL DEFAULT 1
);

-- Creación de la Tabla Module relacionada con form
CREATE TABLE Module (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NULL,
    Active BIT NOT NULL DEFAULT 1
);

--  Creación de la Tabla pivote RolFormPermission
CREATE TABLE RolFormPermission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Active BIT NOT NULL DEFAULT 1,
    RolId INT NOT NULL,
    PermissionId INT NOT NULL,
    FormId INT NOT NULL,
    CONSTRAINT FK_RolFormPermission_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolFormPermission_Permission FOREIGN KEY (PermissionId) REFERENCES Permission(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolFormPermission_Form FOREIGN KEY (FormId) REFERENCES Form(Id) ON DELETE CASCADE
);

-- Creación de la tabla pivote FormModule
CREATE TABLE FormModule (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Active BIT NOT NULL DEFAULT 1,
    FormId INT NOT NULL,
    ModuleId INT NOT NULL,
    CONSTRAINT FK_FormModule_Form FOREIGN KEY (FormId) REFERENCES Form(Id) ON DELETE CASCADE,
    CONSTRAINT FK_FormModule_Module FOREIGN KEY (ModuleId) REFERENCES Module(Id) ON DELETE CASCADE
);