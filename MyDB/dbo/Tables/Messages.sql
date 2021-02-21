CREATE TABLE [dbo].[Messages] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]          INT            NOT NULL,
    [SenderUsername]    NVARCHAR (MAX) NULL,
    [RecipientId]       INT            NOT NULL,
    [RecipientUsername] NVARCHAR (MAX) NULL,
    [Content]           NVARCHAR (MAX) NULL,
    [DateRead]          DATETIME2 (7)  NULL,
    [MessageSent]       DATETIME2 (7)  NOT NULL,
    [SenderDeleted]     BIT            NOT NULL,
    [RecipientDeleted]  BIT            NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Users_RecipientId] FOREIGN KEY ([RecipientId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_SenderId]
    ON [dbo].[Messages]([SenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_RecipientId]
    ON [dbo].[Messages]([RecipientId] ASC);

