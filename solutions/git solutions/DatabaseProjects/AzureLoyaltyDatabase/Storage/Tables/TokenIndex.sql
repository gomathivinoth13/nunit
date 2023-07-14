CREATE TABLE [Storage].[TokenIndex] (
    [RowKey]       NVARCHAR (100) NOT NULL,
    [TokenExpire]  DATETIME       NOT NULL,
    [EmailAddress] NVARCHAR (500) NULL,
    [MemberID]     NVARCHAR (50)  NULL,
    [SSO]          BIT            NOT NULL,
    CONSTRAINT [PK_Storage.TokenIndex] PRIMARY KEY CLUSTERED ([RowKey] ASC)
);

