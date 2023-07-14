using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using SEG.ApiService.Models.Catalina;
using SEG.ApiService.Models.CRC;
using SEG.ApiService.Models.Database;
using SEG.ApiService.Models.FutureStore;
using SEG.ApiService.Models.Mobile;
using SEG.ApiService.Models.Omni;
using SEG.ApiService.Models.Payload;
using SEG.LoyaltyDatabase.Core.Interfaces;
using SEG.LoyaltyDatabase.Core.Repositories;
using SEG.LoyaltyDatabase.Models;

namespace SEG.LoyaltyDatabase.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private IGenericRepository<AdHocSMSJobItem> _adHocSmsJobItemRepository;
        private IGenericRepository<AdhocSMSJob> _adhocSmsJobRepository;
        private IGenericRepository<ApplicationSetting> _applicationSettingRepository;
        private IGenericRepository<BonusOffer> _bonusOfferRepository;
        private IGenericRepository<BulkFileProcessing> _bulkFileProcessingRepository;
        private IGenericRepository<CardRange> _cardRangeRepository;
        private IGenericRepository<CatalinaWinners> _catalinaWinnersRepository;
        private IGenericRepository<CustomerPointTransaction> _customerPointTransactionRepository;
        private IGenericRepository<CustomerServiceRep> _customerServiceRepRepository;
        private IGenericRepository<CustomerServiceTicket> _customerServiceTicketRepository;
        private IGenericRepository<CustPhoneLookup> _custPhoneLookupRepository;
        private IGenericRepository<DeadTaskQueue> _deadTaskQueueRepository;
        private IGenericRepository<EmailPreference> _emailPreferenceRepository;
        private IGenericRepository<EmailPreference_TypeRepository> _emailPreferenceTypeRepository;
        private IGenericRepository<ErrorCode> _errorCodeRepository;
        private IGenericRepository<FutureStoreRequest> _futureStoreRequestRepository;
        private IGenericRepository<GoodwillAudit> _goodwillAuditRepository;
        private IGenericRepository<LinkingLog> _linkingLogRepository;
        private IGenericRepository<Log> _logRepository;
        private IGenericRepository<OfferDefinition> _offerDefinitionRepository;
        private IGenericRepository<PointRemediation> _pointRemediationRepository;
        private IGenericRepository<QueueConfiguration> _queueConfigurationRepository;
        private IGenericRepository<ReverseRedemptionResults> _reverseRedemptionResultsRepository;
        private IGenericRepository<ScheduledProcessBatchUpdate> _scheduledProcessBatchUpdateRepository;
        private IGenericRepository<ScheduledTaskDetail> _scheduledTaskDetailRepository;
        private IGenericRepository<ScheduledTask> _scheduledTaskRepository;
        private IGenericRepository<SMSErrorInfo> _smsErrorInfoRepository;
        private IGenericRepository<SMSHistory> _smsHistoryRepository;
        private IGenericRepository<SMSResponse> _smsResponseRepository;
        private IGenericRepository<SMSTemplate> _smsTemplateRepository;
        private IGenericRepository<StagedMembershipRecord> _stagedMembershipRecordRepository;
        private IGenericRepository<StagedUpdateRecord> _stagedUpdateRecordRepository;
        private IGenericRepository<StoreLaneOmniStatus> _storeLaneOmniStatusRepository;
        private IGenericRepository<UpdateHistoryRecord> _updateHistoryRecordRepository;
        private IGenericRepository<MobileUsageAuditRecord> _mobileUsageAuditRecordRespository;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IGenericRepository<AdHocSMSJobItem> AdHocSMSJobItemRepository => _adHocSmsJobItemRepository ??= new BaseRepository<AdHocSMSJobItem>(_dbConnection);

        public IGenericRepository<AdhocSMSJob> AdhocSMSJobRepository => _adhocSmsJobRepository ??= new BaseRepository<AdhocSMSJob>(_dbConnection);

        public IGenericRepository<ApplicationSetting> ApplicationSettingRepository => _applicationSettingRepository ??= new BaseRepository<ApplicationSetting>(_dbConnection);

        public IGenericRepository<BonusOffer> BonusOfferRepository => _bonusOfferRepository ??= new BaseRepository<BonusOffer>(_dbConnection);

        public IGenericRepository<BulkFileProcessing> BulkFileProcessingRepository => _bulkFileProcessingRepository ??= new BaseRepository<BulkFileProcessing>(_dbConnection);

        public IGenericRepository<CardRange> CardRangeRepository => _cardRangeRepository ??= new BaseRepository<CardRange>(_dbConnection);

        public IGenericRepository<CatalinaWinners> CatalinaWinnersRepository => _catalinaWinnersRepository ??= new BaseRepository<CatalinaWinners>(_dbConnection);

        public IGenericRepository<CustomerPointTransaction> CustomerPointTransactionRepository => _customerPointTransactionRepository ??= new BaseRepository<CustomerPointTransaction>(_dbConnection);

        public IGenericRepository<CustomerServiceRep> CustomerServiceRepRepository => _customerServiceRepRepository ??= new BaseRepository<CustomerServiceRep>(_dbConnection);

        public IGenericRepository<CustomerServiceTicket> CustomerServiceTicketRepository => _customerServiceTicketRepository ??= new BaseRepository<CustomerServiceTicket>(_dbConnection);

        public IGenericRepository<CustPhoneLookup> CustPhoneLookupRepository => _custPhoneLookupRepository ??= new BaseRepository<CustPhoneLookup>(_dbConnection);

        public IGenericRepository<DeadTaskQueue> DeadTaskQueueRepository => _deadTaskQueueRepository ??= new BaseRepository<DeadTaskQueue>(_dbConnection);

        public IGenericRepository<EmailPreference> EmailPreferenceRepository => _emailPreferenceRepository ??= new BaseRepository<EmailPreference>(_dbConnection);

        public IGenericRepository<EmailPreference_TypeRepository> EmailPreference_TypeRepository => _emailPreferenceTypeRepository ??= new BaseRepository<EmailPreference_TypeRepository>(_dbConnection);

        public IGenericRepository<ErrorCode> ErrorCodeRepository => _errorCodeRepository ??= new BaseRepository<ErrorCode>(_dbConnection);
        public IGenericRepository<FutureStoreRequest> FutureStoreRequestRepository => _futureStoreRequestRepository ??= new BaseRepository<FutureStoreRequest>(_dbConnection);

        public IGenericRepository<GoodwillAudit> GoodwillAuditRepository => _goodwillAuditRepository ??= new BaseRepository<GoodwillAudit>(_dbConnection);

        public IGenericRepository<LinkingLog> LinkingLogRepository => _linkingLogRepository ??= new BaseRepository<LinkingLog>(_dbConnection);

        public IGenericRepository<Log> LogRepository => _logRepository ??= new BaseRepository<Log>(_dbConnection);

        public IGenericRepository<OfferDefinition> OfferDefinitionRepository => _offerDefinitionRepository ??= new BaseRepository<OfferDefinition>(_dbConnection);
        public IGenericRepository<PointRemediation> PointRemediationRepository => _pointRemediationRepository ??= new BaseRepository<PointRemediation>(_dbConnection);

        public IGenericRepository<QueueConfiguration> QueueConfigurationRepository => _queueConfigurationRepository ??= new BaseRepository<QueueConfiguration>(_dbConnection);

        public IGenericRepository<ReverseRedemptionResults> ReverseRedemptionResultsRepository => _reverseRedemptionResultsRepository ??= new BaseRepository<ReverseRedemptionResults>(_dbConnection);

        public IGenericRepository<ScheduledProcessBatchUpdate> ScheduledProcessBatchUpdateRepository => _scheduledProcessBatchUpdateRepository ??= new BaseRepository<ScheduledProcessBatchUpdate>(_dbConnection);

        public IGenericRepository<ScheduledTaskDetail> ScheduledTaskDetailRepository => _scheduledTaskDetailRepository ??= new BaseRepository<ScheduledTaskDetail>(_dbConnection);

        public IGenericRepository<ScheduledTask> ScheduledTaskRepository => _scheduledTaskRepository ??= new BaseRepository<ScheduledTask>(_dbConnection);

        public IGenericRepository<SMSErrorInfo> SMSErrorInfoRepository => _smsErrorInfoRepository ??= new BaseRepository<SMSErrorInfo>(_dbConnection);

        public IGenericRepository<SMSHistory> SMSHistoryRepository => _smsHistoryRepository ??= new BaseRepository<SMSHistory>(_dbConnection);

        public IGenericRepository<SMSResponse> SMSResponseRepository => _smsResponseRepository ??= new BaseRepository<SMSResponse>(_dbConnection);

        public IGenericRepository<SMSTemplate> SMSTemplateRepository => _smsTemplateRepository ??= new BaseRepository<SMSTemplate>(_dbConnection);

        public IGenericRepository<StagedMembershipRecord> StagedMembershipRecordRepository => _stagedMembershipRecordRepository ??= new BaseRepository<StagedMembershipRecord>(_dbConnection);

        public IGenericRepository<StagedUpdateRecord> StagedUpdateRecordRepository => _stagedUpdateRecordRepository ??= new BaseRepository<StagedUpdateRecord>(_dbConnection);

        public IGenericRepository<StoreLaneOmniStatus> StoreLaneOmniStatusRepository => _storeLaneOmniStatusRepository ??= new BaseRepository<StoreLaneOmniStatus>(_dbConnection);

        public IGenericRepository<UpdateHistoryRecord> UpdateHistoryRecordRepository => _updateHistoryRecordRepository ??= new BaseRepository<UpdateHistoryRecord>(_dbConnection);
        
        public IGenericRepository<MobileUsageAuditRecord> MobileUsageAuditRecordRepository => _mobileUsageAuditRecordRespository ??= new BaseRepository<MobileUsageAuditRecord>(_dbConnection);
    }
}