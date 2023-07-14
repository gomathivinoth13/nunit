using System.Linq;
using System.Threading.Tasks;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class CustomerServiceTicketService : ICustomerServiceTicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerServiceTicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerServiceTicket> GetByTicketNumberAsync(string number)
        {
            var results = await _unitOfWork.CustomerServiceTicketRepository.GetAsync(c => c.TicketNumber == number);
            return results.FirstOrDefault();
        }

        public bool Add(ref CustomerServiceTicket ticket)
        {
            var sql = "INSERT INTO Audit.CustomerServiceTicket (TicketNumber, TicketDate, Title, Description, Comments, CustomerServiceRep_UserId) Values (@TicketNumber, @TicketDate, @Title, @Description, @Comments, @CustomerServiceRep_UserId);";
            var objTicket = new { ticket.TicketNumber, ticket.TicketDate, ticket.Title, ticket.Description, ticket.Comments, CustomerServiceRep_UserId = ticket.CustomerServiceRep.UserId };
            return _unitOfWork.CustomerServiceTicketRepository.ExecuteSql(sql, objTicket);
        }
    }
}