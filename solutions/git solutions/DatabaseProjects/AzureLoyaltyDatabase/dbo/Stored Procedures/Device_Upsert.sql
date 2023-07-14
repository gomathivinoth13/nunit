CREATE procedure [dbo].[Device_Upsert]
(
	 @MemberId nvarchar(36) = null
	,@DeviceId nvarchar(32)
	,@LastLoginDateTime datetime = null
	,@FirebaseRegistrationId varchar(255)
	,@chain varchar(2)
)
AS

UPDATE [dbo].[Device]
   SET [MemberId] = ISNULL(@MemberId,[MemberId])
       ,[LastLoginDateTime] = ISNULL(@LastLoginDateTime,LastLoginDateTime)
	   ,FirebaseRegistrationId= @FirebaseRegistrationId
	   ,Chain = @chain
 WHERE [DeviceId] = @DeviceId

 if @@RowCount = 0
BEGIN

 

INSERT INTO [dbo].[Device]
           ([DeviceId]
           ,[MemberId]
           ,[CreateDateTime]
			,[FirebaseRegistrationId]
		    ,[CHAIN]
           ,[LastLoginDateTime])
     VALUES
           (@DeviceId 
           ,@MemberId 
           ,getdate()
			,@FirebaseRegistrationId
			,@chain
           ,ISNULL(@LastLoginDateTime,getdate()))
END
GO



