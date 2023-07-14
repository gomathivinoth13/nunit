using SEG.ApiService.Models.Clubs;
using SEG.CustomerWebService.Core;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyService.Process.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Core
{
    public class ClubProcess : IClubProcess
    {
        private readonly ICustomerService _customerService;

        public ClubProcess(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<BabyClubRequest> GetBabyClubInfoAsync(string memberId)
        {
            return await _customerService.GetBabyClubInfoAsync(memberId);
        }

        public async Task<PetClubRequest> GetPetClubInfoAsync(string memberId)
        {
            return await _customerService.GetPetClubInfoAsync(memberId);
        }

        public async Task<PetTypeRequest> GetPetTypeInfoAsync(string petType)
        {
            return await _customerService.GetPetTypeInfoAsync(petType);
        }

        public async Task<BabyClubResponse> SaveBabyClubInfoAsync(BabyClubRequest babyClubInfo)
        {
            return await _customerService.SaveBabyClubInfoAsync(babyClubInfo);
        }

        public async Task<PetClubResponse> SavePetClubInfoAsync(PetClubRequest petClubInfo)
        {
            return await _customerService.SavePetClubInfoAsync(petClubInfo);
        }

        public async Task<PetTypeResponse> SavePetTypeInfoAsync(PetTypeRequest petTypeRequests)
        {
            return await _customerService.SavePetTypeInfoAsync(petTypeRequests);
        }
    }
}
