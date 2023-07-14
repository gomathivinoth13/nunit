CREATE TABLE [dbo].[DeviceMetadata] (
    [DeviceId]         NVARCHAR (32)   NOT NULL,
    [Category]         NVARCHAR (100)  NOT NULL,
    [Subcategory]      NVARCHAR (100)  NULL,
    [Key]              NVARCHAR (100)  NOT NULL,
    [Value]            NVARCHAR (1000) NULL,
    [DeviceMetadataId] BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_dbo.DeviceMetadata] PRIMARY KEY CLUSTERED ([DeviceMetadataId] ASC),
    CONSTRAINT [FK_dbo.DeviceMetadata_dbo.Device_DeviceId] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Device] ([DeviceId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IXU_DeviceId_Category_Subcategory_Key]
    ON [dbo].[DeviceMetadata]([DeviceId] ASC, [Category] ASC, [Subcategory] ASC, [Key] ASC);

