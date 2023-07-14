CREATE TABLE [dbo].[Device] (
    [DeviceId]          NVARCHAR (32) NOT NULL,
    [MemberId]          NVARCHAR (36) NULL,
    [CreateDateTime]    DATETIME2      CONSTRAINT [DF_dbo.Device_CreateDateTime] DEFAULT (getdate()) NULL,
    [LastLoginDateTime] DATETIME2      NULL,
    [FirebaseRegistrationId] VARCHAR(255) NULL, 
    [Chain] VARCHAR(2) NULL, 
    CONSTRAINT [PK_dbo.Device] PRIMARY KEY CLUSTERED ([DeviceId] ASC),
    CONSTRAINT [FK_dbo.Device_dbo.Customer_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Customer] ([MemberId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MemberId]
    ON [dbo].[Device]([MemberId] ASC);


GO

CREATE INDEX [IX_Device_ChainMember] ON [dbo].[Device] ([Chain], [MemberID])

GO
