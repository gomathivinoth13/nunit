using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Runtime.InteropServices;

namespace SEG.Shared
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct ApiQueues
    {
        public static string GoodWill => ConfigurationManager.AppSettings["QUEUES.ProcessGoodWillQueueName"];

        public static string Promotions => ConfigurationManager.AppSettings["QUEUES.ProcessPromotionQueueName"];

        public static string LinkCustomer => ConfigurationManager.AppSettings["QUEUES.LinkCustomerQueueName"];

        public static string ElasticUpdate => ConfigurationManager.AppSettings["QUEUES.ElasticUpdateQueueName"];

        public static string StatisticsQueue => ConfigurationManager.AppSettings["QUEUES.ElasticStatsQueueName"];

        public static string Azure => ConfigurationManager.AppSettings["QUEUES.AzureQueueName"];

        public static string AzureVolatile => ConfigurationManager.AppSettings["QUEUES.AzureVolatileQueueName"];

        public static string AccountProcessing => ConfigurationManager.AppSettings["QUEUES.AccountProcessingQueueName"];

        public static string MembershipProcessing => ConfigurationManager.AppSettings["QUEUES.MembershipProcessingQueueName"];

        public static string Error => ConfigurationManager.AppSettings["QUEUES.ErrorQueueName"];

        public static string ErrorProcessing => ConfigurationManager.AppSettings["QUEUES.ErrorProcessingQueueName"];

        public static string PointsProcessing => ConfigurationManager.AppSettings["QUEUES.PointsProcessing"];

        public static string ScheduledTask => ConfigurationManager.AppSettings["QUEUES.ScheduledTaskQueueName"];
    }
}

