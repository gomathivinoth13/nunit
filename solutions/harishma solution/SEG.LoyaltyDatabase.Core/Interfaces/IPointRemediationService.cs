using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Omni;
using SEG.LoyaltyDatabase.Models;
//SEG.ApiService.Models.Database;


namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IPointRemediationService
    {
        Task<string> InsertPointRemediationsAsync(PointRemediation pointRemediation);
        Task<string> InsertPointRemediationsV2Async(PointRemediation pointRemediation);

        /// <summary>
        /// Finds the Point Remediation points
        /// </summary>
        ///
        /// <remarks>   Mark Robinson </remarks>
        /// <param name="pointRemediation"></param>
        ///        
        ///
        /// <returns>   An asynchronous result that gets the Point Remediation points. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<List<PointRemediationViewModel>> GetPointRemediationAsync(PointRemediation pointRemediation);

        Task<List<PointRemediation>> GetPointRemediationV2Async(PointRemediation pointRemediation);

        Task<bool> CheckDocumentExistAsync(string documentName);
        Task<bool> CheckDocumentExistV2Async(string documentName);
    }
}