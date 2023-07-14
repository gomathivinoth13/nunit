using Dapper;
using SEG.ApiService.Models.MobileFirst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    public class CustomerRewardPromoDAL : DapperDalBase
    {

        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerRewardPromo()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerRewardPromo(db).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV2>> GetCustomerRewardPromoV2() // updated to v3
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerRewardPromoV2(db).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV3>> GetCustomerRewardPromoV3(string language = null) 
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerRewardPromoV3(db, language:language).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerRewardPromoDealOfWeek(DateTime currentDate, string chainId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerRewardPromoDealOfWeek(db, currentDate, chainId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerRewardPromo(IDbConnection connection, IDbTransaction transaction = null)
        {
            var promoDictionary = new Dictionary<string, CustomerRewardPromo>();
            string getQuery = @"SELECT Id, ImageUrl, crp.ActionId, crp.ActionValue, rpa.ActionDescription, StartDateTime, EndDateTime, Banner, Rank, Enabled, CreateDateTime,
            LastUpdateDateTime, LastUpdatedUser, crp.TileId, rpt.TileDescription, Language, EECouponObject, crp.ActionId2, crp.ActionValue2,
            rpa2.ActionDescription as ActionDescription2 FROM dbo.CustomerRewardPromo (nolock) crp 
                        left join dbo.RewardPromoAction rpa on crp.ActionId = rpa.ActionId
                        left join dbo.RewardPromoAction rpa2 on crp.ActionId2 = rpa2.ActionId
                        left join dbo.RewardPromoTile rpt on crp.TileId = rpt.TileId
                        where getdate() between StartDateTime and EndDateTime";

            return (await connection.QueryAsync<CustomerRewardPromo>(getQuery, transaction).ConfigureAwait(false)).ToList();
        }

        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV2>> GetCustomerRewardPromoV2(IDbConnection connection, IDbTransaction transaction = null)
        {
            // updated to v3
            var promoDictionary = new Dictionary<string, CustomerRewardPromoV2>();
            string getQuery = @"SELECT Id, ImageUrl, crp.ActionId, crp.ActionValue, rpa.ActionDescription, StartDateTime, EndDateTime, Banner, Rank, Enabled, CreateDateTime,
            LastUpdateDateTime, LastUpdatedUser, crp.TileId, rpt.TileDescription, Language, EECouponObject, crp.ActionId2, crp.ActionValue2,
            rpa2.ActionDescription as ActionDescription2,PersonalizedPromo FROM dbo.CustomerRewardPromo (nolock) crp 
                        left join dbo.RewardPromoAction rpa on crp.ActionId = rpa.ActionId
                        left join dbo.RewardPromoAction rpa2 on crp.ActionId2 = rpa2.ActionId
                        left join dbo.RewardPromoTile rpt on crp.TileId = rpt.TileId
                        where getdate() between StartDateTime and EndDateTime";

            var promos = (await connection.QueryAsync<CustomerRewardPromoV2>(getQuery, transaction).ConfigureAwait(false)).ToList();
            var filteredPromos = promos.Where(p => p.StartDateTime.CompareTo(DateTime.Now) <= 0 && p.EndDateTime.CompareTo(DateTime.Now) > 0).ToList();

            return filteredPromos;
        }


        /// <summary>
        /// GetCustomerPromoSlider
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromoV3>> GetCustomerRewardPromoV3(IDbConnection connection, IDbTransaction transaction = null, string language = null)
        {
            var addLanguage = language != null ? $" and Language = '{ language }'" : "";
            var promoDictionary = new Dictionary<string, CustomerRewardPromoV3>();
            string getQuery = $@"SELECT Id, ImageUrl, crp.ActionId, crp.ActionValue, rpa.ActionDescription, StartDateTime, EndDateTime, Banner, Rank, Enabled, CreateDateTime,
            LastUpdateDateTime, LastUpdatedUser, crp.TileId, rpt.TileDescription, Language, EECouponObject, crp.ActionId2, crp.ActionValue2,
            rpa2.ActionDescription as ActionDescription2,PersonalizedPromo, SliderTitle, SliderSubtitle, SliderCTA FROM dbo.CustomerRewardPromo (nolock) crp 
                        left join dbo.RewardPromoAction rpa on crp.ActionId = rpa.ActionId
                        left join dbo.RewardPromoAction rpa2 on crp.ActionId2 = rpa2.ActionId
                        left join dbo.RewardPromoTile rpt on crp.TileId = rpt.TileId
                        where getdate() between StartDateTime and EndDateTime
                        { addLanguage }";
            
            return (await connection.QueryAsync<CustomerRewardPromoV3>(getQuery, transaction).ConfigureAwait(false)).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentDate"></param>
        /// <param name="chainId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<CustomerRewardPromo>> GetCustomerRewardPromoDealOfWeek(IDbConnection connection, DateTime currentDate, string chainId, IDbTransaction transaction = null)
        {

            CustomerRewardPromo couponOfweek = new CustomerRewardPromo();
            var result = (await connection.QueryAsync<CustomerRewardPromo>("Select top 1 * from  CustomerRewardPromo (nolock) where  @currentDate >= StartDateTime and @currentDate<= EndDateTime and Banner = @chainId and ActionId = 4 order by StartDateTime desc", new { currentDate, chainId }, transaction).ConfigureAwait(false)).ToList();

            return result;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<CustomerRewardPromo> GetCustomerRewardPromoById(Guid id, IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerRewardPromoById(id, db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<CustomerRewardPromo> GetCustomerRewardPromoById(Guid id, IDbConnection connection, IDbTransaction transaction)
        {
            string getQuery = @"Select * FROM [dbo].[CustomerRewardPromo] WHERE Id = @Id";

            return (await connection.QueryAsync<CustomerRewardPromo>(getQuery, new { id }, transaction).ConfigureAwait(false)).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<RewardPromoTile>> GetRewardPromoTiles(IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetRewardPromoTiles(db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<List<RewardPromoTile>> GetRewardPromoTiles(IDbConnection connection, IDbTransaction transaction)
        {
            string getQuery = @"Select * FROM [dbo].[RewardPromoTile]";

            return (await connection.QueryAsync<RewardPromoTile>(getQuery, transaction).ConfigureAwait(false)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<RewardPromoAction>> GetRewardPromoActions(IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetRewardPromoActions(db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<List<RewardPromoAction>> GetRewardPromoActions(IDbConnection connection, IDbTransaction transaction)
        {
            string getQuery = @"Select * FROM [dbo].[RewardPromoAction]";

            return (await connection.QueryAsync<RewardPromoAction>(getQuery, transaction).ConfigureAwait(false)).ToList();
        }

        public static async Task<int> InsertCustomerRewardPromo(CustomerRewardPromo customerRewardPromo, IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await InsertCustomerRewardPromo(customerRewardPromo, db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<int> InsertCustomerRewardPromo(CustomerRewardPromo customerRewardPromo, IDbConnection connection, IDbTransaction transaction)
        {
            customerRewardPromo.CreateDateTime = DateTime.Now;
            customerRewardPromo.LastUpdateDateTime = DateTime.Now;

            string insertQuery = @"INSERT INTO [dbo].[CustomerRewardPromo] ([ImageUrl] ,[ActionId] ,[ActionValue] ,[StartDateTime] ,[EndDateTime] ,[Banner] ,[Rank] ,[Enabled] ,[CreateDateTime] ,[LastUpdateDateTime], [LastUpdatedUser], [TileId], [Language], [EECouponObject], [ActionId2], [ActionValue2]) VALUES (@ImageUrl ,@ActionId ,@ActionValue ,@StartDateTime ,@EndDateTime ,@Banner ,@Rank ,@Enabled ,@CreateDateTime ,@LastUpdateDateTime, @LastUpdatedUser, @TileId, @Language, @EECouponObject, @ActionId2, @ActionValue2)";

            return (await connection.ExecuteAsync(insertQuery, customerRewardPromo, transaction).ConfigureAwait(false));
        }

        public static async Task<int> DeleteCustomerRewardPromo(Guid id, IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await DeleteCustomerRewardPromo(id, db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<int> DeleteCustomerRewardPromo(Guid id, IDbConnection connection, IDbTransaction transaction)
        {
            string deleteQuery = @"DELETE FROM [dbo].[CustomerRewardPromo] WHERE Id = @Id";

            return (await connection.ExecuteAsync(deleteQuery, new { id }, transaction).ConfigureAwait(false));
        }

        public static async Task<int> UpdateCustomerRewardPromo(CustomerRewardPromo customerRewardPromo, IDbTransaction transaction = null)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await UpdateCustomerRewardPromo(customerRewardPromo, db, transaction).ConfigureAwait(false);
            }
        }

        private static async Task<int> UpdateCustomerRewardPromo(CustomerRewardPromo customerRewardPromo, IDbConnection connection, IDbTransaction transaction)
        {
            string updateQuery = @"UPDATE [dbo].[CustomerRewardPromo] SET [ImageUrl] = @ImageUrl ,[ActionId] = @ActionId ,[ActionValue] = @ActionValue ,[StartDateTime] = @StartDateTime ,[EndDateTime] = @EndDateTime ,[Banner] = @Banner ,[Rank] = @Rank ,[Enabled] = @Enabled ,[CreateDateTime] = @CreateDateTime ,[LastUpdateDateTime] = @LastUpdateDateTime, [LastUpdatedUser] = @LastUpdatedUser, [TileId] = @TileId, [Language] = @Language, [EECouponObject] = @EECouponObject,
            [ActionId2] = @ActionId2, [ActionValue2] = @ActionValue2 WHERE [Id] = @Id;";

            return (await connection.ExecuteAsync(updateQuery, new
            {
                Id = customerRewardPromo.Id,
                ImageUrl = customerRewardPromo.ImageUrl,
                ActionId = customerRewardPromo.ActionId,
                ActionValue = customerRewardPromo.ActionValue,
                StartDateTime = customerRewardPromo.StartDateTime,
                EndDateTime = customerRewardPromo.EndDateTime,
                Banner = customerRewardPromo.Banner,
                Rank = customerRewardPromo.Rank,
                Enabled = customerRewardPromo.Enabled,
                CreateDateTime = customerRewardPromo.CreateDateTime,
                LastUpdateDateTime = DateTime.Now,
                LastUpdatedUser = customerRewardPromo.LastUpdatedUser,
                TileId = customerRewardPromo.TileId,
                Language = customerRewardPromo.Language,
                EECouponObject = customerRewardPromo.EECouponObject,
                ActionId2 = customerRewardPromo.ActionId2,
                ActionValue2 = customerRewardPromo.ActionValue2
            }, transaction).ConfigureAwait(false));
        }
    }
}
