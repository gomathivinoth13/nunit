CREATE TABLE [dbo].[CustomerBannerMetadata] (
    [MemberId]           NVARCHAR (36) NOT NULL,
    [ChainId]            CHAR (2)      NOT NULL,
    [CouponAlias]        NVARCHAR (20) NOT NULL,
    [CouponId]           NVARCHAR (36) NULL,
    [ShoppingListId]     NVARCHAR (36) NULL,
    [StoreNumber]        INT           NULL,
    [CreateDateTime]     DATETIME      CONSTRAINT [DF_dbo.CustomerBannerMetadata_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME      CONSTRAINT [DF_dbo.CustomerBannerMetadata_LastUpdateDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_dbo.CustomerBannerMetadata] PRIMARY KEY CLUSTERED ([MemberId] ASC, [ChainId] ASC),
    CONSTRAINT [FK_dbo.CustomerBannerMetadata_dbo.Customer_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Customer] ([MemberId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IXU_MemberId_ChainId]
    ON [dbo].[CustomerBannerMetadata]([MemberId] ASC, [ChainId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CouponAlias]
    ON [dbo].[CustomerBannerMetadata]([CouponAlias] ASC)
    INCLUDE([MemberId]);


GO


CREATE TRIGGER [dbo].[CustomerBannerMetadata_INSERT] on [dbo].[CustomerBannerMetadata]
INSTEAD OF INSERT
AS
BEGIN
  
 delete m from CustomerBannerMetaData m inner join inserted i on i.CouponAlias=m.CouponAlias and m.MemberId != i.MemberId
  --Build an INSERT statement ignoring inserted.ID and 
  --inserted.ComputedCol.
  INSERT INTO CustomerBannerMetaData
  select * from inserted
END;

GO
DISABLE TRIGGER [dbo].[CustomerBannerMetadata_INSERT]
    ON [dbo].[CustomerBannerMetadata];

