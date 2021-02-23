CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   INT                IDENTITY (1, 1) NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [DateOfBirth]          DATETIME2 (7)      NOT NULL,
    [KnownAs]              NVARCHAR (MAX)     NULL,
    [Created]              DATETIME2 (7)      NOT NULL,
    [LastActive]           DATETIME2 (7)      NOT NULL,
    [Gender]               NVARCHAR (MAX)     NULL,
    [Introduction]         NVARCHAR (MAX)     NULL,
    [LookingFor]           NVARCHAR (MAX)     NULL,
    [Interests]            NVARCHAR (MAX)     NULL,
    [City]                 NVARCHAR (MAX)     NULL,
    [Country]              NVARCHAR (MAX)     NULL,
    [AccessFailedCount]    INT                DEFAULT ((0)) NOT NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [LockoutEnabled]       BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [TwoFactorEnabled]     BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);

