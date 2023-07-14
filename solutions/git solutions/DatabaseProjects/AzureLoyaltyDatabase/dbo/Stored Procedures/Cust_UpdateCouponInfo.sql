CREATE procedure [dbo].[Cust_UpdateCouponInfo]
(
	 @MemberId nvarchar(36) 
	,@ChainId char(2)
	,@CouponId varchar(36) 
	,@ShoppingListId varchar(36) 	
)
AS

UPDATE [dbo].[CustomerBannerMetadata]
	SET 
		 [CouponId] = @CouponId
		,[ShoppingListId] = @ShoppingListId
		,[LastUpdateDateTime] = GETDATE()
	WHERE 
		MemberId = @MemberId
	AND
		ChainId = @ChainId