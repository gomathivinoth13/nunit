CREATE TABLE [dbo].[QueueConfiguration] (
    [QueueConfigurationId]         INT            NOT NULL,
    [CreateDateTime]               DATETIME       CONSTRAINT [DF_dbo.QueueConfiguration_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [QueueName]                    NVARCHAR (255) NULL,
    [MaxMessageCount]              INT            NOT NULL,
    [MaxNumberOfRetries]           INT            NOT NULL,
    [DefaultLockDurationInSeconds] INT            NULL,
    [IsActive]                     BIT            CONSTRAINT [DF_dbo.QueueConfiguration_IsActive] DEFAULT ((1)) NOT NULL,
    [MoveToErrorQueue]             BIT            CONSTRAINT [DF_dbo.QueueConfiguration_MoveToErrorQueue] DEFAULT ((1)) NOT NULL,
    [DeleteOnStartUp]              BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.QueueConfiguration] PRIMARY KEY CLUSTERED ([QueueConfigurationId] ASC)
);

