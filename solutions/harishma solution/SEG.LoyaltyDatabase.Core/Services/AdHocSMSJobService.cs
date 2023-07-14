using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class AdHocSMSJobService : IAdHocSMSJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdHocSMSJobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AdhocSMSJob>> GetJobsAsync(Expression<Func<AdhocSMSJob, bool>> expression)
        {
            return await _unitOfWork.AdhocSMSJobRepository.GetAsync(expression);
        }

        public async Task<bool> AddAsync(AdhocSMSJob job)
        {
            return await _unitOfWork.AdhocSMSJobRepository.InsertAsync(job, includePrimaryKey: false);
        }
    }
}