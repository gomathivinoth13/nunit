create procedure [dbo].[Cust_GetByMemberId]
(
	@MemberId varchar(36)
)
AS
Select 
	* 
from 
	[dbo].[Customer]
WHERE 
	MemberId = @MemberId  