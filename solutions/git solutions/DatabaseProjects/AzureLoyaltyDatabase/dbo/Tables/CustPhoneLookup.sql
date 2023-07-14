CREATE TABLE [dbo].[CustPhoneLookup](
	[MobilePhone] [nvarchar](20) NOT NULL,
	[TwilioSuccessCheck] [bit] NOT NULL,
	[LastUpdateDateTime] [datetime] NOT NULL,
	[LastUpdateSource] [nvarchar](50) NULL,
	[PhoneType] [nvarchar](500) NULL,
	[IsMobile] [bit] NULL,
 CONSTRAINT [PK_dbo.CustPhoneLookup] PRIMARY KEY CLUSTERED 
(
	[MobilePhone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].[CustPhoneLookup] ADD  CONSTRAINT [DF_dbo.CustPhoneLookup_LastUpdateDateTime]  DEFAULT (getdate()) FOR [LastUpdateDateTime]
GO
/****** Object:  Index [IX_CustPhoneLookup_PhoneType]    Script Date: 4/30/2018 7:44:40 AM ******/
CREATE NONCLUSTERED INDEX [IX_CustPhoneLookup_PhoneType] ON [dbo].[CustPhoneLookup]
(
	[PhoneType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO