CREATE TABLE [dbo].[Likes] (
    [SourceUserId] INT NOT NULL,
    [LikedUserId]  INT NOT NULL,
    CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED ([SourceUserId] ASC, [LikedUserId] ASC),
    CONSTRAINT [FK_Likes_AspNetUsers_LikedUserId] FOREIGN KEY ([LikedUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Likes_AspNetUsers_SourceUserId] FOREIGN KEY ([SourceUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Likes_LikedUserId]
    ON [dbo].[Likes]([LikedUserId] ASC);

