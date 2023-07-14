//using SEG.ApiService.Models;
//using SEG.ApiService.Models.Database;
//using SEG.ApiService.Models.SilverPop;
//using SEG.LoyaltyDatabase.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure.Interception;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SEG.AzureLoyaltyDatabase
//{
    //[DbConfigurationType(typeof(AzureDatabaseConfiguration))]
    //public class AzureLoyaltyDatabaseContext : DbContext
    //{

    //    public AzureLoyaltyDatabaseContext() : base("LoyaltyConnection")
    //    {
    //        DbInterception.Add(new IsolationLevelInterceptor(System.Data.IsolationLevel.ReadUncommitted));
           
    //    }

       // public DbSet<QueueConfiguration> QueueConfigurations { get; set; }
     //   public DbSet<AzureLog> Logs { get; set; }
      //  public DbSet<ErrorQueueLog> ErrorQueueLogs { get; set; }
        //public DbSet<AzureCustomer> AzureCustomers { get; set; }
      //  public DbSet<AzureCustomerBannerMetadata> AzureCustomerBanners { get; set; }
        //public DbSet<DeadTaskQueue> DeadTaskQueues { get; set; }
        //public DbSet<Device> Devices { get; set; }
       // public DbSet<DeviceMetadata> DeviceMetadata { get; set; }
        //public DbSet<AzureCustomerExernalLogins> ExternalLogins { get; set; }
        
       // public DbSet<SilverPopAccessToken> SilverPopAccessTokens { get; set; }
       // public DbSet<TokenIndex> TokenIndex { get; set; }
      //  public DbSet<ErrorCode> ErrorCodes { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.Entity<AzureCustomerBannerMetadata>().Property(x => x.ChainId).HasColumnType(SqlColumnType.Char).HasMaxLength(2);
           
        //    // modelBuilder.Entity<AzureCustomer>().Property(p => p.RowVersion).IsConcurrencyToken();
        //}
 //   }
//}