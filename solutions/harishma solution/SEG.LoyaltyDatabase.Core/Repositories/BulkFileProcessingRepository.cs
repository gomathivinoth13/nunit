using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Database;
using System.Data;

namespace SEG.LoyaltyDatabase.Core.Repositories
{
    public class BulkFileProcessingRepository : BaseRepository<BulkFileProcessing>
    {
        public BulkFileProcessingRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}