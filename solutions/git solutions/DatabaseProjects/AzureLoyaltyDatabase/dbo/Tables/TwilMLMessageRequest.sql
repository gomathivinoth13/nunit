CREATE TABLE [dbo].[TwilMLMessageRequest] (
    [MessageSid]          NVARCHAR (34)  NOT NULL,
    [Body]                NVARCHAR (500) NULL,
    [FromCity]            NVARCHAR (100) NULL,
    [FromState]           NVARCHAR (100) NULL,
    [FromZip]             NVARCHAR (10)  NULL,
    [ToCity]              NVARCHAR (100) NULL,
    [ToZip]               NVARCHAR (10)  NULL,
    [ToCountry]           NVARCHAR (50)  NULL,
    [AccountSid]          NVARCHAR (34)  NULL,
    [MessagingServiceSid] NVARCHAR (34)  NULL,
    [From]                NVARCHAR (15)  NULL,
    [To]                  NVARCHAR (15)  NULL,
    [MessageDate]         DATETIME       NULL,
    CONSTRAINT [PK_dbo.TwilMLMessageRequest] PRIMARY KEY CLUSTERED ([MessageSid] ASC)
);

