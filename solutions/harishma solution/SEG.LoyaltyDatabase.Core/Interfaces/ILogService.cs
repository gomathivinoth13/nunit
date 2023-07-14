using System;
using System.Threading.Tasks;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface ILogService
    {
        Task<bool> DeleteInternalLogRecordsByDateAsync(DateTime deleteDate);
    }
}