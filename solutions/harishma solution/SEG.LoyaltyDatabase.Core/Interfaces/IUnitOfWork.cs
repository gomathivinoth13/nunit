using SEG.ApiService.Models.Catalina;
using SEG.ApiService.Models.CRC;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.Excentus;
using SEG.ApiService.Models.FutureStore;
using SEG.ApiService.Models.Mobile;
using SEG.ApiService.Models.Omni;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Repositories;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<AdHocSMSJobItem> AdHocSMSJobItemRepository { get; }
        public IGenericRepository<AdhocSMSJob> AdhocSMSJobRepository { get; }
        public IGenericRepository<ApplicationSetting> ApplicationSettingRepository { get; }
        public IGenericRepository<BonusOffer> BonusOfferRepository { get; }
        public IGenericRepository<BulkFileProcessing> BulkFileProcessingRepository { get; }
        public IGenericRepository<CardRange> CardRangeRepository { get; }
        public IGenericRepository<CatalinaWinners> CatalinaWinnersRepository { get; }
        public IGenericRepository<CustomerPointTransaction> CustomerPointTransactionRepository { get; }
        public IGenericRepository<CustomerServiceRep> CustomerServiceRepRepository { get; }
        public IGenericRepository<CustomerServiceTicket> CustomerServiceTicketRepository { get; }
        public IGenericRepository<CustPhoneLookup> CustPhoneLookupRepository { get; }
        public IGenericRepository<DeadTaskQueue> DeadTaskQueueRepository { get; }
        public IGenericRepository<EmailPreference> EmailPreferenceRepository { get; }
        public IGenericRepository<EmailPreference_TypeRepository> EmailPreference_TypeRepository { get; }
        public IGenericRepository<FutureStoreRequest> FutureStoreRequestRepository { get; }
        public IGenericRepository<ErrorCode> ErrorCodeRepository { get; }
        public IGenericRepository<GoodwillAudit> GoodwillAuditRepository { get; }
        public IGenericRepository<LinkingLog> LinkingLogRepository { get; }
        public IGenericRepository<Log> LogRepository { get; }
        public IGenericRepository<OfferDefinition> OfferDefinitionRepository { get; }
        public IGenericRepository<PointRemediation> PointRemediationRepository { get; }
        public IGenericRepository<QueueConfiguration> QueueConfigurationRepository { get; }
        public IGenericRepository<ReverseRedemptionResults> ReverseRedemptionResultsRepository { get; }
        public IGenericRepository<ScheduledProcessBatchUpdate> ScheduledProcessBatchUpdateRepository { get; }
        public IGenericRepository<ScheduledTaskDetail> ScheduledTaskDetailRepository { get; }
        public IGenericRepository<ScheduledTask> ScheduledTaskRepository { get; }
        public IGenericRepository<SMSErrorInfo> SMSErrorInfoRepository { get; }
        public IGenericRepository<SMSHistory> SMSHistoryRepository { get; }
        public IGenericRepository<SMSResponse> SMSResponseRepository { get; }
        public IGenericRepository<SMSTemplate> SMSTemplateRepository { get; }
        public IGenericRepository<StagedMembershipRecord> StagedMembershipRecordRepository { get; }
        public IGenericRepository<StagedUpdateRecord> StagedUpdateRecordRepository { get; }
        public IGenericRepository<StoreLaneOmniStatus> StoreLaneOmniStatusRepository { get; }
        public IGenericRepository<UpdateHistoryRecord> UpdateHistoryRecordRepository { get; }
        public IGenericRepository<MobileUsageAuditRecord> MobileUsageAuditRecordRepository { get; }
    }
}