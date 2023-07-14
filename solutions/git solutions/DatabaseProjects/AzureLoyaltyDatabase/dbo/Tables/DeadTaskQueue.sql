CREATE TABLE [dbo].[DeadTaskQueue] (
    [DeadTaskQueueId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreateDateTime]  DATETIME       CONSTRAINT [DF_dbo.DeadTaskQueue_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [QueueName]       NVARCHAR (255) NULL,
    [Error]           NVARCHAR (500) NULL,
    [QueueTask]       NVARCHAR (MAX) NULL,
    [Exception]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.DeadTaskQueue] PRIMARY KEY CLUSTERED ([DeadTaskQueueId] ASC)
);

