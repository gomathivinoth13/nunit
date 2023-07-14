CREATE TABLE [dbo].[ExternalLogins] (
    [Provider] NVARCHAR (50)  NOT NULL,
    [UserId]   NVARCHAR (100) NOT NULL,
    [MemberId] NVARCHAR (36)  NULL,
    CONSTRAINT [PK_dbo.ExternalLogins] PRIMARY KEY CLUSTERED ([Provider] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.ExternalLogins_dbo.Customer_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Customer] ([MemberId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MemberId]
    ON [dbo].[ExternalLogins]([MemberId] ASC);

