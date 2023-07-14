using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEG.ApiService.Models;
using SEG.ApiService.Models.Clubs;
using SEG.ApiService.Models.Payload;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.PrimaryStore;
using SEG.ApiService.Models.Request;
using SEG.LoyaltyService.Models.Results;

namespace SEG.CustomerWebService.Core
{
    public interface ICustomerService       
    {
        Task<PrimaryStoreResponse> GetPrimaryStoreAsync(string memberId, int chainId);
        Task<BabyClubRequest> GetBabyClubInfoAsync(string memberId);
        Task<BabyClubResponse> SaveBabyClubInfoAsync(BabyClubRequest babyClubInfo);
        Task<PetClubRequest> GetPetClubInfoAsync(string memberId);
        Task<PetClubResponse> SavePetClubInfoAsync(PetClubRequest petClubInfo);
        Task<PetTypeRequest> GetPetTypeInfoAsync(string petType);
        Task<PetTypeResponse> SavePetTypeInfoAsync(PetTypeRequest petTypeInfo);
        Task<CustomerPreferenceUpsertResponse> SaveCustomerEReceiptPreferenceAsync(CustomerPreference customerPreference);
        Task<CustomerPreferenceRetrieveResponse> GetCustomerEReceiptPreferenceAsync(string memberId = null, string mobilePhoneNumber = null, string emailAddess = null);

        string ConnectionString { get; }
        string BaseUrl { get; }
        int MaxNumberOfSearchResults { get; set; }

        Task<CustomerV2> GetCustomerAsync(string memberId);
        Task<CustomerPreferenceRetrieveResponse> GetCustomerPreference(string memberId, string chainId);

        /////##################################################################################################################################////////
        ////////////***********************************************Not required any more after CustODS redesign ******************************//////
        /////////////////########################################################################################################//////////////////
        Task<FRNStatusValidateResponse> GetFRNStatus(string memberid = null, string crcid = null, string aliasNumber = null);
        Task<PhoneNumberValidationResponse> PhoneNumberValidation(string phoneNumber = null);
        Task<CustomerResponse> SaveCustomerAsync(CustomerV2 customer);
        Task<CustomerPreferenceUpsertResponse> SaveCustomerPreference(CustomerPreference customerPreference);
        Task<SaveMemberAliasResult> SaveCustomerAlias(string crcId, MemberAlias alias, bool returnHydratedObject);
        Task<SaveCustomerPinPasswordResult> SaveCustomerPinPassword(SaveCustomerPinPasswordRequest request);       
        Task<SetValidatedPhoneNumberResponse> SetValidatedPhoneNumber(SetValidatedPhoneNumberRequest request);            
        bool VerifyCustomer(CustomerV2 customer, out List<string> errorMessages);
        bool VerifyCustomerAddress(CustomerAddress customerAddress, out List<string> errorMessages);
        bool VerifyCustomerChild(CustomerChild customerChild, out List<string> errorMessages);
        bool VerifyMemberAlias(MemberAlias alias, out List<string> errorMessages);
        Task<CustomerSearchResponse> CustomerSearchAsync(CustomerSearchRequest customerSearchRequest, int retryCount = 0, bool hardPhoneNumberSearch = true);
        Task<bool> DeleteCustomerChildAsync(string crcId, int childId);
        Task<CustomerSearchResponse> GetCustRecordAsync(CustomerSearchRequest customerSearchRequest);
        Task<CustomerAliasSearchResponse> CustAliasSearchAsync(CustomerSearchRequest customerSearchRequest);
        Task<PiiRequest> DeletePiiAsync(string memberId);
    }
}