CREATE TABLE [dbo].[Log] (
    [LogId]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [MessageId]        UNIQUEIDENTIFIER NOT NULL,
    [CreateDateTime]   DATETIME         CONSTRAINT [DF_dbo.Log_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [Machine]          NVARCHAR (255)   NOT NULL,
    [Thread]           NVARCHAR (255)   NOT NULL,
    [Level]            NVARCHAR (50)    NOT NULL,
    [Logger]           NVARCHAR (255)   NOT NULL,
    [ApiTransactionId] NVARCHAR (255)   NULL,
    [Message]          NVARCHAR (500)   NULL,
    [Exception]        NVARCHAR (MAX)   NULL,
    [Request]          NVARCHAR (MAX)   NULL,
    [Response]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_MessageId]
    ON [dbo].[Log]([MessageId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreateDateTime]
    ON [dbo].[Log]([CreateDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Level]
    ON [dbo].[Log]([Level] ASC);

