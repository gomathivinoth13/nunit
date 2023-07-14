CREATE TABLE [dbo].[DeliveryStatus] (
    [MessageSid]          NVARCHAR (34)  NOT NULL,
    [MessageStatus]       NVARCHAR (MAX) NULL,
    [ApiVersion]          NVARCHAR (MAX) NULL,
    [SmsSid]              NVARCHAR (MAX) NULL,
    [SmsStatus]           NVARCHAR (MAX) NULL,
    [AccountSid]          NVARCHAR (34)  NULL,
    [MessagingServiceSid] NVARCHAR (34)  NULL,
    [From]                NVARCHAR (15)  NULL,
    [To]                  NVARCHAR (15)  NULL,
    [MessageDate]         DATETIME       NULL,
    [ErrorCode]           NVARCHAR (8)   NULL,
    [ErrorMessage]        NVARCHAR (100) NULL,
    [Body]                NVARCHAR (500) NULL,
    [Price]               NVARCHAR (10)  NULL,
    [NumSegments]         INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.DeliveryStatus] PRIMARY KEY CLUSTERED ([MessageSid] ASC)
);

