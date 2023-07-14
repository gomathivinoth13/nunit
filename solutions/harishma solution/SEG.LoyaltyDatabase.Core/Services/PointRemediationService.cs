using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Omni;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.ApiService.Models.Database;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Services
{
    public class PointRemediationService : IPointRemediationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string queryString = @"SELECT * FROM dbo.PointRemediation /**where**/"; 
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts the Point Remediation points
        /// </summary>
        ///
        /// <remarks>   Mark Robinson </remarks>
        /// <param name="pointRemediation"></param>
        ///        
        ///
        /// <returns>   An asynchronous result that adds the Point Remediation points. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public PointRemediationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> InsertPointRemediationsAsync(PointRemediation pointRemediation)
        {
            string error = "";

            if (pointRemediation == null) return "Failure - Null object sent";

            if (pointRemediation.ErrorMessages != null && pointRemediation.ErrorMessages.Length != 0)
            {
                error = pointRemediation.ErrorMessages[1].ToString();
            }
            else
            {
                error = "";
            }

            pointRemediation.ErrorMessages = new[] { error };
            var sql = "INSERT INTO PointRemediation (Status, Comment, OfferID, CRC, Store, Points, ExpDate, RecordID, RecordType, UserId, TransmitDate, ChainId, ReceiptNumber, DocumentName, ErrorMessages,OrderNumber) Values (@Status, @Comment, @OfferID, @CRC, @Store, @Points, @ExpDate, @RecordID, @RecordType, @UserId, @TransmitDate, @ChainId, @ReceiptNumber, @DocumentName, @ErrorMessages,@OrderNumber);";
            var result = await _unitOfWork.PointRemediationRepository.ExecuteSqlAsync(sql, pointRemediation);
            return result ? "Success" : "Failure";
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts the Point Remediation points
        /// </summary>
        ///
        /// <remarks>   Mark Robinson </remarks>
        /// <param name="pointRemediation"></param>
        ///        
        ///
        /// <returns>   An asynchronous result that adds the Point Remediation points. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<string> InsertPointRemediationsV2Async(PointRemediation pointRemediation)
        {
            try
            {
                string error = "";

                if (pointRemediation == null)
                {
                    return "Failure - Null object sent";
                }


                if (pointRemediation.ErrorMessages != null && pointRemediation.ErrorMessages.Length != 0)
                {
                    error = string.Join(", ", pointRemediation.ErrorMessages);
                }
                else
                {
                    error = "";
                }

                pointRemediation.ErrorMessages = new[] { error };
                var sql = "INSERT INTO PointRemediation (Status, Comment, OfferID, CRC, Store, Points, ExpDate, RecordID, RecordType, UserId, TransmitDate, ChainId, ReceiptNumber, DocumentName, ErrorMessages, OrderNumber) Values (@Status, @Comment, @OfferID, @CRC, @Store, @Points, @ExpDate, @RecordID, @RecordType, @UserId, @TransmitDate, @ChainId, @ReceiptNumber, @DocumentName, @ErrorMessages, @OrderNumber);";
                var result = await _unitOfWork.PointRemediationRepository.ExecuteSqlAsync(sql, pointRemediation);
                return result ? "Success" : "Failure";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<List<PointRemediation>> GetPointRemediationV2Async(PointRemediation pointRemediation)
        {
            if (pointRemediation == null)
            {
                return null;
            }

            var builder = new SqlBuilder();
            var searchSQL = builder.AddTemplate(queryString);

            if (!string.IsNullOrEmpty(pointRemediation.Status))
            {
                builder.Where("Status = @Status", new { pointRemediation.Status });
            }
            if (!string.IsNullOrEmpty(pointRemediation.UserId))
            {
                builder.Where("UserId = @UserId", new { pointRemediation.UserId });
            }
            if (pointRemediation.FromTransmitDate != default(DateTime) && pointRemediation.ToTransmitDate == default(DateTime))
            {
                builder.Where("TransmitDate between @FromTransmitDate and GETDATE()", new { pointRemediation.FromTransmitDate });
            }
            if (pointRemediation.FromTransmitDate != default(DateTime) && pointRemediation.ToTransmitDate != default(DateTime))
            {
                builder.Where("TransmitDate between @FromTransmitDate and @ToTransmitDate", new { pointRemediation.FromTransmitDate, pointRemediation.ToTransmitDate });
            }
            if (!string.IsNullOrEmpty(pointRemediation.Store))
            {
                builder.Where("Store = @Store", new { pointRemediation.Store });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CustomerServiceTicket?.TicketNumber))
            {
                builder.Where("TicketNumber = @TicketNumber", new { pointRemediation.CustomerServiceTicket.TicketNumber });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CustomerServiceTicket?.Description))
            {
                builder.Where("TicketDescription = @TicketDescription", new { TicketDescription = pointRemediation.CustomerServiceTicket.Description });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CRC))
            {
                builder.Where("CRC = @CRC", new { pointRemediation.CRC });
            }
            if (!string.IsNullOrEmpty(pointRemediation.OfferID))
            {
                builder.Where("OfferID = @OfferID", new { pointRemediation.OfferID });
            }
            if (!string.IsNullOrEmpty(pointRemediation.OrderNumber))
            {
                builder.Where("OrderNumber = @OrderNumber", new { pointRemediation.OrderNumber });
            }

            var result = await _unitOfWork.PointRemediationRepository.GetAsync<PointRemediation>(searchSQL.RawSql, searchSQL.Parameters);
            return result.ToList();
        }

        public async Task<bool> CheckDocumentExistV2Async(string documentName)
        {
            var queryString3 = @"Select Count(1) from dbo.PointRemediation where DocumentName=@documentName";
            if (string.IsNullOrEmpty(documentName)) return false;

            var results = await _unitOfWork.PointRemediationRepository.GetAsync<int>(queryString3, new { documentName });
            return results.FirstOrDefault() > 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<List<PointRemediationViewModel>> GetPointRemediationAsync(PointRemediation pointRemediation)
        {
            if (pointRemediation == null) return null;

            var builder = new SqlBuilder();
            var searchSQL = builder.AddTemplate(queryString);

            if (pointRemediation.RecordID != Guid.Empty)
            {
                builder.Where("RecordID = @RecordID", new { RecordId = pointRemediation.RecordID });
            }
            if (!string.IsNullOrEmpty(pointRemediation.Status))
            {
                builder.Where("Status = @Status", new { pointRemediation.Status });
            }
            if (!string.IsNullOrEmpty(pointRemediation.UserId))
            {
                builder.Where("UserId = @UserId", new { pointRemediation.UserId });
            }
            if (pointRemediation.TransmitDate != default(DateTime))
            {
                builder.Where("FORMAT(TransmitDate,'M/dd/yyyy') between FORMAT(@FromTransmitDate,'M/dd/yyyy') and FORMAT(@ToTransmitDate,'M/dd/yyyy')", new { pointRemediation.FromTransmitDate, pointRemediation.ToTransmitDate });
            }
            if (!string.IsNullOrEmpty(pointRemediation.Store))
            {
                builder.Where("Store = @Store", new { pointRemediation.Store });
            }
            if (!string.IsNullOrEmpty(pointRemediation.RecordType))
            {
                builder.Where("RecordType = @RecordType", new { pointRemediation.RecordType });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CustomerServiceTicket?.TicketNumber))
            {
                builder.Where("TicketNumber = @TicketNumber", new { pointRemediation.CustomerServiceTicket.TicketNumber });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CustomerServiceTicket?.Description))
            {
                builder.Where("TicketDescription = @TicketDescription", new { TicketDescription = pointRemediation.CustomerServiceTicket.Description });
            }
            if (!string.IsNullOrEmpty(pointRemediation.CRC))
            {
                builder.Where("CRC = @CRC", new { pointRemediation.CRC });
            }
            if (!string.IsNullOrEmpty(pointRemediation.OfferID))
            {
                builder.Where("OfferID = @OfferID", new { pointRemediation.OfferID });
            }
            if (!string.IsNullOrEmpty(pointRemediation.Comment))
            {
                builder.Where("Comment = @Comment", new { pointRemediation.Comment });
            }
            if (!string.IsNullOrEmpty(pointRemediation.OrderNumber))
            {
                builder.Where("OrderNumber = @OrderNumber", new { pointRemediation.OrderNumber });
            }

            var result = await _unitOfWork.PointRemediationRepository.GetAsync<PointRemediationViewModel>(searchSQL.RawSql, searchSQL.Parameters);
            return result.ToList();
        }

        public async Task<bool> CheckDocumentExistAsync(string documentName)
        {
            return await CheckDocumentExistV2Async(documentName);
        }
    }
}
