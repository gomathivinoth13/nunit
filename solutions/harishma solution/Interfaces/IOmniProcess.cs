////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	OmniProcess.cs
//
// summary:	Implements the omni process class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Models;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Core.Interfaces
{
    public interface IOmniProcess
    {
        void Dispose();
        Task<GoodwillAudit> ProcessGoodWillAsync(ProcessGoodwillEventPayload payload, bool excentusRewards);
        Task<PointRemediation> ProcessEEPointRedemptionAsync(ExcentusPointsRedemptionPayloadV2 payloadV2);
    }
}