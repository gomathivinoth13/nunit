using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.FutureStore;
using SEG.LoyaltyDatabase.Core.Interfaces;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class FutureStoreRequestService : IFutureStoreRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FutureStoreRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> FutureStoreRequestAsync(FutureStoreRequest futureStore)
        {
            try
            {
                if (futureStore == null) return "Failure - Null object sent";

                var result = await _unitOfWork.FutureStoreRequestRepository.InsertAsync(futureStore, includePrimaryKey: true);
                return result ? "Success" : "Failure";
            }
            catch(Exception ex)
            {
                return $"Failure - {ex.Message}";
            }
        }
        public async Task<List<FutureStoreRequest>> GetFutureStoreRequests(FutureStoreRequest futureStoreRequest)
        {
            IEnumerable<FutureStoreRequest> queryResult;
            try
            {
                if (futureStoreRequest == null) return null;

                    if (futureStoreRequest.Id < 0)
                    {
                    //get everything
                        var results = await _unitOfWork.FutureStoreRequestRepository.GetAllAsync();
                        return results.ToList();
                    }
                    else
                    {
                        //Check for Id , 
                        if (futureStoreRequest.Id == 0)
                        {
                            // check for the other two values
                            bool hasZipCode = false;
                            bool hasEmailAddress = false;

                            if (!String.IsNullOrWhiteSpace(futureStoreRequest.ZipCode))
                            {
                                hasZipCode = true;
                            }
                            if (!String.IsNullOrWhiteSpace(futureStoreRequest.EmailAddress))
                            {
                                hasEmailAddress = true;
                            }

                            if (hasZipCode && hasEmailAddress)
                            {
                                queryResult = await _unitOfWork.FutureStoreRequestRepository.GetAsync(f => f.ZipCode == futureStoreRequest.ZipCode && f.EmailAddress == futureStoreRequest.EmailAddress);
                                return queryResult.ToList();
                            }
                            else if (hasZipCode)
                            {
                                queryResult = await _unitOfWork.FutureStoreRequestRepository.GetAsync(f => f.ZipCode == futureStoreRequest.ZipCode);
                                return queryResult.ToList();

                            }
                            else if (hasEmailAddress)
                            {
                                queryResult = await _unitOfWork.FutureStoreRequestRepository.GetAsync(f => f.EmailAddress == futureStoreRequest.EmailAddress);
                                return queryResult.ToList();
                            }
                            else
                            {
                                //Id =0, no data in other strings.. return null

                                return null;
                            }


                        }
                        else
                        {
                        //gettting for a particualr id
                            queryResult = await _unitOfWork.FutureStoreRequestRepository.GetAsync(f => f.Id == futureStoreRequest.Id);
                            return queryResult.ToList();

                        }
                    }


            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
       

    }
}
