CREATE TABLE [dbo].[orchard_Orchard_Roles_RolesPermissionsRecord] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [Role_id]       INT NULL,
    [Permission_id] INT NULL,
    [RoleRecord_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

