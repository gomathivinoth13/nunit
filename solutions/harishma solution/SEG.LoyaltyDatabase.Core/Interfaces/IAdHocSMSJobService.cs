using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IAdHocSMSJobService
    {
        Task<IEnumerable<AdhocSMSJob>> GetJobsAsync(Expression<Func<AdhocSMSJob, bool>> expression);
        Task<bool> AddAsync(AdhocSMSJob job);
    }
}