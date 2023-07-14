using Dapper;
using SEG.ApiService.Models.Surveys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    /// <summary>
    /// Product Survey Data Access Layer
    /// </summary>
    public class ProductSurveyDAL : DapperDalBase
    {
        /// <summary>
        /// Save Product Survey
        /// </summary>
        public static async Task<bool> SaveProductSurveyAsync(ProductSurvey survey)
        {
            var isSaved = false;
            using (IDbConnection db = new SqlConnection(ConnectionStringSurvey))
            {
                var sql = @"BEGIN TRANSACTION; 
                            UPDATE OwnBrandsProductSurvey WITH (UPDLOCK, SERIALIZABLE)
                            SET LOCID = @LocId,
                            UPC_CODE = @UpcCode,
                            BANNER = @Banner,
                            SATISFACTION = @Satisfaction,
                            TASTE = @Taste,
                            PACKAGING = @Packaging,
                            SIZE = @Size,
                            VISUALAPPEAL = @VisualAppeal,
                            BUYITAGAIN = @BuyItAgain,
                            COMMENTS = @Comments
                            WHERE MEMBERID = @MemberId; 

                            IF @@ROWCOUNT = 0
                            BEGIN
                            INSERT OwnBrandsProductSurvey
                            (MEMBERID
                            ,CRCID
                            ,LOCID
                            ,UPC_CODE
                            ,TRANSACTIONDATETIME
                            ,BANNER
                            ,SATISFACTION
                            ,TASTE
                            ,PACKAGING
                            ,SIZE
                            ,VISUALAPPEAL
                            ,BUYITAGAIN
                            ,COMMENTS) 
                            VALUES(@MemberId
                            ,@CrcId
                            ,@LocId
                            ,@UpcCode
                            ,@TransactionDateTime
                            ,@Banner
                            ,@Satisfaction
                            ,@Taste
                            ,@Packaging
                            ,@Size
                            ,@VisualAppeal
                            ,@BuyItAgain
                            ,@Comments);
                            END 
                            COMMIT TRANSACTION;";
                var rowsEffected = await db.ExecuteAsync(sql, survey);
                isSaved = rowsEffected == 1;
            }

            return isSaved;
        }

        /// <summary>
        /// Get Product Survey by Member Id
        /// </summary>
        public static async Task<List<ProductSurvey>> GetProductSurveyByMemberIdAsync(string memberId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (IDbConnection db = new SqlConnection(ConnectionStringSurvey))
            {
                var sql = @"SELECT * FROM OwnBrandsProductSurvey WHERE MEMBERID = @memberId";
                var surveys = await db.QueryAsync<ProductSurvey>(sql, new { memberId = memberId });
                return surveys.ToList();
            }
        }
    }
}
