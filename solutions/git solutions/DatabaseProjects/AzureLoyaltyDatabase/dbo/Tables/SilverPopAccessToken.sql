CREATE TABLE [dbo].[SilverPopAccessToken] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [access_token]          NVARCHAR (MAX) NULL,
    [token_type]            NVARCHAR (MAX) NULL,
    [refresh_token]         NVARCHAR (MAX) NULL,
    [expires_in]            INT            NOT NULL,
    [TransactionId]         NVARCHAR (MAX) NULL,
    [ExpireAtDateTime]      DATETIME       NOT NULL,
    [SilverPopClientID]     NVARCHAR (MAX) NULL,
    [SilverPopClientSecret] NVARCHAR (MAX) NULL,
    [SilverPopListId]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.SilverPopAccessToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);

