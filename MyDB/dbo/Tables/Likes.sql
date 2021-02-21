CREATE TABLE [dbo].[Likes] (
    [SourceUserId] INT NOT NULL,
    [LikedUserId]  INT NOT NULL,
    CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED ([SourceUserId] ASC, [LikedUserId] ASC),
    CONSTRAINT [FK_Likes_Users_LikedUserId] FOREIGN KEY ([LikedUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Likes_Users_SourceUserId] FOREIGN KEY ([SourceUserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Likes_LikedUserId]
    ON [dbo].[Likes]([LikedUserId] ASC);

