CREATE procedure [dbo].[Customer_Upsert]
(
	 @MemberId nvarchar(36) 
	,@FirstName nvarchar(50)
	,@LastName nvarchar(50)
	,@EmailAddress nvarchar(255) 
	,@MobilePhoneNUmber nvarchar(49) 	
	,@AliasNumber nvarchar(60) 	
	,@MobilePhoneVerified bit = null
	,@Password nvarchar(200)
	,@EmailVerified bit = null
	,@EmailVerificationCode nvarchar(max)
	,@ZipCode nvarchar(6)
	,@PinCodeExpriration datetime
)
AS

IF EXISTS (Select top 1 MemberID from Customer where MEMBERID = @MemberID)
 
UPDATE [dbo].[Customer]
   SET [FirstName] = @FirstName
      ,[LastName] = @LastName
      ,[EmailAddress] = @EmailAddress
      ,[MobilePhoneNumber] = @MobilePhoneNumber
      ,[AliasNumber] = @AliasNumber
      ,[LastUpdateDateTime] = getdate()
      ,[MobilePhoneVerified] = @MobilePhoneVerified
      ,[EmailVerified] = @EmailVerified
      ,[EmailVerificationCode] = @EmailVerificationCode
      ,[ZipCode] = @ZipCode
	  ,[Password] = @Password
      ,[PinCodeExpriration] = @PinCodeExpriration
 WHERE  [MemberId] = @MemberId
      
 


ELSE
 

INSERT INTO [dbo].[Customer]
           ([MemberId]
           ,[FirstName]
           ,[LastName]
           ,[EmailAddress]
           ,[MobilePhoneNumber]
           ,[AliasNumber]
           ,[CreateDateTime]
           ,[LastUpdateDateTime]
           ,[MobilePhoneVerified]
           ,[Password]
           ,[EmailVerified]
           ,[EmailVerificationCode]
           ,[ZipCode]
           ,[PinCodeExpriration])
     VALUES
           (@MemberId
           ,@FirstName
           ,@LastName
           ,@EmailAddress
           ,@MobilePhoneNumber
           ,@AliasNumber
           ,getdate()
           ,getdate()
           ,@MobilePhoneVerified
           ,@Password
           ,@EmailVerified
           ,@EmailVerificationCode
           ,@ZipCode
           ,@PinCodeExpriration)
GO


