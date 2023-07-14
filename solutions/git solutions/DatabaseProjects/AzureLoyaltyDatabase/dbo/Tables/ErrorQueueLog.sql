CREATE TABLE [dbo].[ErrorQueueLog] (
    [ErrorQueueLogId]           INT      IDENTITY (1, 1) NOT NULL,
    [StartDateTime]             DATETIME NOT NULL,
    [CompleteDateTime]          DATETIME NULL,
    [MessageReprocessCount]     INT      NOT NULL,
    [MessageMaxErrorLimitCount] INT      NOT NULL,
    CONSTRAINT [PK_dbo.ErrorQueueLog] PRIMARY KEY CLUSTERED ([ErrorQueueLogId] ASC)
);

