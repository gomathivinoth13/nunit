CREATE TABLE [dbo].[ErrorCode] (
    [Code]               NVARCHAR (50)  NOT NULL,
    [Message]            NVARCHAR (400) NULL,
    [Condition]          NVARCHAR (MAX) NULL,
    [IsOmniError]        BIT            NULL,
    [Status]             NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [ResultCode]         INT            NULL,
    [Context]            NVARCHAR (MAX) NULL,
    [UserMessage]        NVARCHAR (MAX) NULL,
    [CreateDateTime]     DATETIME       CONSTRAINT [DF_dbo.ErrorCode_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]         NVARCHAR (100) CONSTRAINT [DF_dbo.ErrorCode_CreateUser] DEFAULT (suser_sname()) NULL,
    [LastUpdateDateTime] DATETIME       CONSTRAINT [DF_dbo.ErrorCode_LastUpdateDateTime] DEFAULT (getdate()) NOT NULL,
    [LastUpdateUser]     NVARCHAR (100) CONSTRAINT [DF_dbo.ErrorCode_LastUpdateUser] DEFAULT (suser_sname()) NULL,
    CONSTRAINT [PK_dbo.ErrorCode] PRIMARY KEY CLUSTERED ([Code] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CreateDateTime]
    ON [dbo].[ErrorCode]([CreateDateTime] ASC);

