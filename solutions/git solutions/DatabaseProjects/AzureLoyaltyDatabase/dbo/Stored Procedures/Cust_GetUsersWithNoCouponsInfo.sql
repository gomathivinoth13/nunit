CREATE procedure [dbo].[Cust_GetUsersWithNoCouponsInfo]

AS
select 
custmet.MemberId,custmet.ChainId,cust.FirstName,cust.LastName,cust.EmailAddress,cust.MobilePhoneNumber,custmet.CouponAlias, 
case when CHARINDEX('.', CouponAlias) > 0
	 then substring(custmet.CouponAlias, 0, CHARINDEX('.', CouponAlias))
	 else custmet.CouponAlias
end as CRC
from Customer (nolock) cust
inner join CustomerBannerMetadata (nolock) custmet
on cust.MemberId = custmet.MemberId
where custmet.ChainId = 3
and custmet.CouponAlias is not null
and (custmet.CouponId is null or custmet.ShoppingListId is null)