using SEG.ApiService.Models.Clubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Core.Interfaces
{
    public interface IClubProcess
    {
        Task<BabyClubRequest> GetBabyClubInfoAsync(string memberId);
        Task<BabyClubResponse> SaveBabyClubInfoAsync(BabyClubRequest babyClubInfo);
        Task<PetClubRequest> GetPetClubInfoAsync(string memberId);
        Task<PetClubResponse> SavePetClubInfoAsync(PetClubRequest petClubInfo);
        Task<PetTypeRequest> GetPetTypeInfoAsync(string petType);
        Task<PetTypeResponse> SavePetTypeInfoAsync(PetTypeRequest petTypeRequests);
    }
}
