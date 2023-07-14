using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Payload;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class CustomerServiceTicketRepository : BaseRepository<CustomerServiceTicket>
    {
        public CustomerServiceTicketRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
