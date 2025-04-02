-- creación de la tabla User con relacion a Person
CREATE TABLE [dbo].[User] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [State] BIT NOT NULL,
    [PersonId] INT NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person]([Id])
);
GO

-- Creación de la tabla Person relación de uno a uno con User 
CREATE TABLE [dbo].[Person] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [PhoneNumber] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Address] NVARCHAR(200) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Creación de la tabla Rol relación de uno a muchos con RolUser
CREATE TABLE [dbo].[Rol] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Active] BIT NOT NULL,
    [Description] NVARCHAR(200) NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Creació de la tabla permission
CREATE TABLE [dbo].[Permission] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Creació de la tabla Form
CREATE TABLE [dbo].[Form] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    CONSTRAINT [PK_Form] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Creació de la tabla Module
CREATE TABLE [dbo].[Module] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- Creación de la tabla RolFormPermission
CREATE TABLE [dbo].[RolFormPermission] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [RoleId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    [FormId] INT NOT NULL,
    CONSTRAINT [PK_RolFormPermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolFormPermission_Rol] FOREIGN KEY ([RoleId])
        REFERENCES [dbo].[Rol]([Id]),
    CONSTRAINT [FK_RolFormPermission_Permission] FOREIGN KEY ([PermissionId])
        REFERENCES [dbo].[Permission]([Id]),
    CONSTRAINT [FK_RolFormPermission_Form] FOREIGN KEY ([FormId])
        REFERENCES [dbo].[Form]([Id])
);
GO

-- Creación de la tabla RolUSer
CREATE TABLE [dbo].[RolUser] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_RolUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolUser_User] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[User]([Id]),
    CONSTRAINT [FK_RolUser_Rol] FOREIGN KEY ([RoleId])
        REFERENCES [dbo].[Rol]([Id])
);
GO

-- creacion de la tabla FormModule
CREATE TABLE [dbo].[FormModule] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [FormId] INT NOT NULL,
    [ModuleId] INT NOT NULL,
    CONSTRAINT [PK_FormModule] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FormModule_Form] FOREIGN KEY ([FormId])
        REFERENCES [dbo].[Form]([Id]),
    CONSTRAINT [FK_FormModule_Module] FOREIGN KEY ([ModuleId])
        REFERENCES [dbo].[Module]([Id])
);
GO

-- Creación de la tabla AuitLog
CREATE TABLE [dbo].[AuditLog] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [TableName] NVARCHAR(100) NOT NULL,
    [AffectedId] INT NOT NULL,
    [PropertyName] NVARCHAR(100) NOT NULL,
    [OldValue] NVARCHAR(255) NULL,
    [NewValue] NVARCHAR(255) NULL,
    [Action] NVARCHAR(50) NOT NULL,
    [Timestamp] DATETIME2 NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditLog_User] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[User]([Id])
);
GO

-- Creación de la tabla 
CREATE TABLE [bdo].[RelatedPerson](
    [Id]  INT IDENTITY(1,1) NOT NULL,
    [TypeRelation] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(100) NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [PK_RelatedPerson] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RelatedPerson_User] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[User]([Id])
)