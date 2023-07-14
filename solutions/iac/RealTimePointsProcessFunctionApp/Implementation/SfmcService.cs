using SEG.SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SalesForceLibrary.SalesForceAPIM;
using System.Threading.Tasks;
using SEG.EagleEyeLibrary.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RealTimePointsProcessFunctionApp.Interface;
using RealTimePointsProcessFunctionApp.Constants;

namespace RealTimePointsProcessFunctionApp.Implementation
{
    public class SfmcService : ISfmcService
    {
        public readonly SalesForceAPIMService salesForceService;
        private readonly string SfmcDataExtensionKey;
        ILogger<SfmcService> _log;
        public SfmcService(SalesForceAPIMService salesForceAPIMService, IConfiguration configuration, ILogger<SfmcService> logger)
        {
            salesForceService = salesForceAPIMService ?? throw new ArgumentNullException(nameof(salesForceAPIMService));
            //  SfmcDataExtensionKey = Environment.GetEnvironmentVariable("SEG_Key") ?? throw new ArgumentNullException(nameof(SfmcDataExtensionKey));

            SfmcDataExtensionKey = "SEG_All_Cust_Data_QA" ?? throw new ArgumentNullException(nameof(SfmcDataExtensionKey));
            _log = logger ?? throw new ArgumentNullException(nameof(salesForceAPIMService));
        }

        public async Task<DataExtentionsResponse> SetRealTimePointData(int Current_point_balance, string member_id, int Expiring_points, DateTime Next_exp_date)
        {
            var dataExtentionRequest = new DataExtentionsRequest();
            var dataExtentionsResponse = new DataExtentionsResponse();
            dataExtentionRequest.items = new List<Item>()
                                    {
                                  new Item
                                  {
                                  Current_Points_Balance = Current_point_balance,
                                  MEMBER_ID              = member_id,
                                  Expiring_Points        = Expiring_points,
                                  Next_Exprn_Dt          = Next_exp_date
                                } };
            if (dataExtentionRequest.items.Count > 0)
            {
                dataExtentionsResponse = await InsertToSfmc(dataExtentionRequest);
            }
            return dataExtentionsResponse;
        }

        public async Task<DataExtentionsResponse> InsertToSfmc(DataExtentionsRequest dataExtentionRequest)
        {
            try
            {
                var dataExtentionsResponse = new DataExtentionsResponse();
                if (dataExtentionRequest.items.Count > 0)
                {

                    dataExtentionsResponse = await salesForceService.UpsertAsync(dataExtentionRequest, SfmcDataExtensionKey);
                }
                else
                {
                    dataExtentionsResponse.errorcode = ResponseMessage.ErrorMessage;
                }
                return dataExtentionsResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ResponseMessage.ErrorMessage);
                throw;
            }
        }

        public Task<DataExtentionsResponse> RealTimeDataProcess(Account account)
        {
            Item item = new Item();

            if (account.PointsInfo != null)
            {
                Point point = account.PointsInfo.OrderBy(a => a.ValidTo).First();
                item.Expiring_Points = (int)point.Points;
                item.Next_Exprn_Dt = (DateTime)point.ValidTo;
                if (account.Balances != null)
                {
                    item.Current_Points_Balance = account.Balances.Usable;
                }
                else
                {
                    _log.LogInformation("Balance are not present for current wallet id.");
                }

                _log.LogInformation($"Points and balance present for current wallet id. " +
                    $"Expiring_points: " + $"{item.Expiring_Points}, " +
                    $"Next_Exprn_Dt: {item.Next_Exprn_Dt} ," +
                    $" balance {account.Balances.Usable}");

                var dataExtentionsResponse = SetRealTimePointData(item.Expiring_Points, item.MEMBER_ID, item.Current_Points_Balance, item.Next_Exprn_Dt);
                return dataExtentionsResponse;
            }
            else
            {
                var dataExtentionsResponse = SetRealTimePointData(0, item.MEMBER_ID, 0, DateTime.Parse("01/01/2000 10:00:00 AM"));
                _log.LogInformation("Points are not present for current wallet id.");
                return dataExtentionsResponse;
            }

        }

    }
}
