CREATE TABLE [dbo].[Photos] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [AppUserId] INT            NOT NULL,
    [Url]       NVARCHAR (MAX) NULL,
    [IsMain]    BIT            NOT NULL,
    [PublicId]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Photos_Users_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Photos_AppUserId]
    ON [dbo].[Photos]([AppUserId] ASC);

