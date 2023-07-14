using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ICustomerServiceTicketService
    {
        Task<CustomerServiceTicket> GetByTicketNumberAsync(string number);
        bool Add(ref CustomerServiceTicket ticket);
    }
}