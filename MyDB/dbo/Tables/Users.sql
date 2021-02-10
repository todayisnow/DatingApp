CREATE TABLE [dbo].[Users] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (MAX)  NULL,
    [PasswordHash] VARBINARY (MAX) NULL,
    [PasswordSalt] VARBINARY (MAX) NULL,
    [DateOfBirth]  DATETIME2 (7)   NOT NULL,
    [KnownAs]      NVARCHAR (MAX)  NULL,
    [Created]      DATETIME2 (7)   NOT NULL,
    [LastActive]   DATETIME2 (7)   NOT NULL,
    [Gender]       NVARCHAR (MAX)  NULL,
    [Introduction] NVARCHAR (MAX)  NULL,
    [LookingFor]   NVARCHAR (MAX)  NULL,
    [Interests]    NVARCHAR (MAX)  NULL,
    [City]         NVARCHAR (MAX)  NULL,
    [Country]      NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

