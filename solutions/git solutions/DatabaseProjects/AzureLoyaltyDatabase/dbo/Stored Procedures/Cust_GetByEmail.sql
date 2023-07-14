CREATE procedure [dbo].[Cust_GetByEmail]
(
	@EmailAddress varchar(255)
)
AS
Select 
	* 
from 
	[dbo].[Customer]
WHERE 
	EmailAddress = @EmailAddress