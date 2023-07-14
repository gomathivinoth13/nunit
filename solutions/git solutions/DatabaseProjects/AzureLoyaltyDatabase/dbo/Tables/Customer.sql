CREATE TABLE [dbo].[Customer] (
    [MemberId]                 NVARCHAR (36)  NOT NULL,
    [FirstName]                NVARCHAR (50)  NULL,
    [LastName]                 NVARCHAR (50)  NULL,
    [EmailAddress]             NVARCHAR (255) NULL,
    [MobilePhoneNumber]        NVARCHAR (20)  NULL,
    [AliasNumber]              NVARCHAR (60)  NULL,
    [CreateDateTime]           DATETIME       CONSTRAINT [DF_dbo.Customer_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime]       DATETIME       CONSTRAINT [DF_dbo.Customer_LastUpdateDateTime] DEFAULT (getdate()) NOT NULL,
    [EncryptedPin]             VARCHAR (64)   NULL,
    [MobilePhoneVerified]      BIT            NULL,
    [Password]                 NVARCHAR (200) NULL,
    [EmailVerified]            BIT            NULL,
    [EmailVerificationCode]    NVARCHAR (50) NULL,
    [ZipCode]                  NVARCHAR (6)   NULL,
    [PinCodeExpriration]       DATETIME       NULL,
    CONSTRAINT [PK_dbo.Customer] PRIMARY KEY CLUSTERED ([MemberId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxAzureCustomerEmailAddress]
    ON [dbo].[Customer]([EmailAddress] ASC);


GO
CREATE NONCLUSTERED INDEX [idxAzureCustomerMobilePhone]
    ON [dbo].[Customer]([MobilePhoneNumber] ASC);

