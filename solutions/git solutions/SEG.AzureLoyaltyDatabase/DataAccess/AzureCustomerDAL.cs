////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	dataaccess\azurecustomerdal.cs
//
// summary:	Implements the azurecustomerdal class
////////////////////////////////////////////////////////////////////////////////////////////////////

using SEG.ApiService.Models;
using SEG.ApiService.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using SEG.ApiService.Models.Attributes;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Dynamic;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An azure customer dal. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class AzureCustomerDAL : DapperDalBase, IDisposable
    {
        const string WDChainId = "1";   ///< Identifier for the wd chain
        const string BILOChainId = "2"; ///< Identifier for the bilo chain
        const string HARVEYSChainId = "3";  ///< Identifier for the harveys chain
        private static log4net.ILog Logging = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  ///< The logging
        //  static AzureLoyaltyDatabaseContext azureLoyaltyDBContext = new AzureLoyaltyDatabaseContext();

        #region Constants

        const string WinnDixieCouponAliasPrefix = "9800";   ///< The winn dixie coupon alias prefix

        #endregion Constants

        #region Public Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by dictionary. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="querys">           The querys. </param>
        /// <param name="connection">       The connection. </param>
        /// <param name="transaction">      (Optional) The transaction. </param>
        /// <param name="queryIsOrClause">  (Optional) True if query is or clause. </param>
        /// <param name="overrideClause">   (Optional) The override clause. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by dictionary. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByDictionary(Dictionary<string, object> querys, IDbConnection connection, IDbTransaction transaction = null, bool queryIsOrClause = false, string overrideClause = null)
        {

            var sql = @"SELECT distinct C.*, m.*, D.*, 
                               dm.DeviceMetaDataId, dm.deviceid, dm.[Category],dm.[Subcategory],dm.[Key],dm.[Value]
                        FROM [dbo].[Customer] C (nolock) LEFT JOIN
	                        [dbo].[CustomerBannerMetadata] M (nolock) on M.MemberID = C.MemberId LEFT JOIN
	                        [dbo].[Device] D (nolock) on D.MemberID = C.MemberID   LEFT JOIN
                            [dbo].DeviceMetaData dm (nolock) on dm.DeviceID = d.deviceId
                         WHERE ";
            StringBuilder sqlBuilder = new StringBuilder(sql);
            bool first = true;
            dynamic request = new ExpandoObject();

            foreach (var item in querys.Keys)
            {
                if (string.IsNullOrWhiteSpace(overrideClause))
                {
                    if (!first)
                    {
                        sqlBuilder.Append(queryIsOrClause ? " OR " : " AND ");
                        sqlBuilder.Append(item);
                    }
                    else
                    {
                        first = false;
                        sqlBuilder.Append(item);
                    }
                }
                AddProperty(request, item.Replace(" like '%' + ", "=").Split('=')[1].Trim().Replace("@", string.Empty), querys[item]);
            }

            if (!string.IsNullOrWhiteSpace(overrideClause))
            {
                sqlBuilder.Append(overrideClause);
            }

            Dictionary<string, AzureCustomer> customerDictionary = new Dictionary<string, AzureCustomer>();

            var customers = (await connection.QueryAsync<AzureCustomer, AzureCustomerBannerMetadata, Device, DeviceMetadata, AzureCustomer>(sqlBuilder.ToString(),
                 (azurecustomer, metadata, device, dm) =>
                 {

                     if (!customerDictionary.TryGetValue(azurecustomer.MemberId.Trim(), out AzureCustomer found))
                     {
                         found = azurecustomer;

                         customerDictionary.Add(azurecustomer.MemberId.Trim(), found);
                     }

                     if (metadata != null)
                     {
                         if (found.AzureCustomerBannerMetadata == null) found.AzureCustomerBannerMetadata = new List<AzureCustomerBannerMetadata>();
                         if (!found.AzureCustomerBannerMetadata.Any(a => a.ChainId == metadata.ChainId))
                             found.AzureCustomerBannerMetadata.Add(metadata);
                     }
                     if (device != null)
                     {
                         if (found.Devices == null) found.Devices = new List<Device>();
                         if (!found.Devices.Any(a => a.DeviceId == device.DeviceId))
                             found.Devices.Add(device);

                     }
                     if (dm != null)
                     {
                         var dev = found.Devices.FirstOrDefault(a => a.DeviceId == dm.DeviceId);
                         if (dev.DeviceMetadata == null) dev.DeviceMetadata = new List<DeviceMetadata>();
                         if (!dev.DeviceMetadata.Any(a => a.DeviceMetadataId == dm.DeviceMetadataId))
                             dev.DeviceMetadata.Add(dm);
                     }
                     customerDictionary[found.MemberId] = found;
                     return found;
                 },
                 (object)request, transaction: transaction, splitOn: "MemberId, MemberID, DeviceId, DeviceMetaDataId").ConfigureAwait(false)).ToList();



            return customers.FirstOrDefault();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="querys"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="queryIsOrClause"></param>
        /// <param name="overrideClause"></param>
        /// <returns></returns>
        public static async Task<List<AzureCustomer>> GetCustomerByDictionaryListMobile(Dictionary<string, object> querys, IDbConnection connection, IDbTransaction transaction = null, bool queryIsOrClause = false, string overrideClause = null)
        {

            var sql = @"SELECT distinct C.*, m.*, D.*, 
                               dm.DeviceMetaDataId, dm.deviceid, dm.[Category],dm.[Subcategory],dm.[Key],dm.[Value]
                        FROM [dbo].[Customer] C (nolock) LEFT JOIN
	                        [dbo].[CustomerBannerMetadata] M (nolock) on M.MemberID = C.MemberId LEFT JOIN
	                        [dbo].[Device] D (nolock) on D.MemberID = C.MemberID   LEFT JOIN
                            [dbo].DeviceMetaData dm (nolock) on dm.DeviceID = d.deviceId
                         WHERE ";
            StringBuilder sqlBuilder = new StringBuilder(sql);
            bool first = true;
            dynamic request = new ExpandoObject();

            foreach (var item in querys.Keys)
            {
                if (string.IsNullOrWhiteSpace(overrideClause))
                {
                    if (!first)
                    {
                        sqlBuilder.Append(queryIsOrClause ? " OR " : " AND ");
                        sqlBuilder.Append(item);
                    }
                    else
                    {
                        first = false;
                        sqlBuilder.Append(item);
                    }
                }
                AddProperty(request, item.Replace(" like '%' + ", "=").Split('=')[1].Trim().Replace("@", string.Empty), querys[item]);
            }

            if (!string.IsNullOrWhiteSpace(overrideClause))
            {
                sqlBuilder.Append(overrideClause);
            }

            Dictionary<string, AzureCustomer> customerDictionary = new Dictionary<string, AzureCustomer>();

            var customers = (await connection.QueryAsync<AzureCustomer, AzureCustomerBannerMetadata, Device, DeviceMetadata, AzureCustomer>(sqlBuilder.ToString(),
                 (azurecustomer, metadata, device, dm) =>
                 {

                     if (!customerDictionary.TryGetValue(azurecustomer.MemberId.Trim(), out AzureCustomer found))
                     {
                         found = azurecustomer;

                         customerDictionary.Add(azurecustomer.MemberId.Trim(), found);
                     }

                     if (metadata != null)
                     {
                         if (found.AzureCustomerBannerMetadata == null) found.AzureCustomerBannerMetadata = new List<AzureCustomerBannerMetadata>();
                         if (!found.AzureCustomerBannerMetadata.Any(a => a.ChainId == metadata.ChainId))
                             found.AzureCustomerBannerMetadata.Add(metadata);
                     }
                     if (device != null)
                     {
                         if (found.Devices == null) found.Devices = new List<Device>();
                         if (!found.Devices.Any(a => a.DeviceId == device.DeviceId))
                             found.Devices.Add(device);

                     }
                     if (dm != null)
                     {
                         var dev = found.Devices.FirstOrDefault(a => a.DeviceId == dm.DeviceId);
                         if (dev.DeviceMetadata == null) dev.DeviceMetadata = new List<DeviceMetadata>();
                         if (!dev.DeviceMetadata.Any(a => a.DeviceMetadataId == dm.DeviceMetadataId))
                             dev.DeviceMetadata.Add(dm);
                     }
                     customerDictionary[found.MemberId] = found;
                     return found;
                 },
                 (object)request, transaction: transaction, splitOn: "MemberId, MemberID, DeviceId, DeviceMetaDataId").ConfigureAwait(false)).ToList();



            return customers.Distinct().ToList();

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a property. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="expando">          The expando. </param>
        /// <param name="propertyName">     Name of the property. </param>
        /// <param name="propertyValue">    The property value. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        #region GetCustomerByMemberId

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByMemberId(string memberId)
        {

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByMemberId(memberId, db).ConfigureAwait(false);

            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by member identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by member identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByMemberId(string memberId, IDbConnection connection, IDbTransaction transaction = null)
        {

            Dictionary<string, object> request = new Dictionary<string, object>() { { "C.MemberID=@memberId", memberId } };

            return await GetCustomerByDictionary(request, connection, transaction);

        }
        #endregion

        #region GetCustomerByExternalLoginAsync

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by external login asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="provider"> The provider. </param>
        /// <param name="userId">   Identifier for the user. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by external login. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByExternalLoginAsync(string provider, string userId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByExternalLoginAsync(provider, userId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by external login asynchronous. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="provider">     The provider. </param>
        /// <param name="userId">       Identifier for the user. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by external login. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByExternalLoginAsync(string provider, string userId, IDbConnection connection, IDbTransaction transaction = null)
        {

            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "C.Provider=@provider", provider },
                { "E.UserId=@userid", userId } };

            return await GetCustomerByDictionary(request, connection, transaction);


        }
        #endregion

        #region GetCustomerByEmail

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by email. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">    The email. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by email. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByEmail(string email)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByEmail(email, db).ConfigureAwait(false);
            }
        }

      


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer"> The customer. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task SaveCustomer(AzureCustomer customer)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    await SaveCustomer(customer, db, trans);
                    trans.Commit();
                }

            }
        }



        internal static async Task SaveCustomerBanner(AzureCustomer customer)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    await SaveCustomer(customer, db, trans);
                    trans.Commit();
                }

            }
        }


        internal static async Task SaveCustomerMobile(AzureCustomer customer)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var trans = db.BeginTransaction())
                {
                    await SaveCustomerMobile(customer, db, trans);
                    trans.Commit();
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves a customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer">     The customer. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task SaveCustomer(AzureCustomer customer, IDbConnection connection, IDbTransaction transaction = null)
        {
            //create anonymous object 
            var payload = new
            {
                MemberId = customer.MemberId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                MobilePhoneNumber = customer.MobilePhoneNumber,
                AliasNumber = customer.AliasNumber,
                MobilePhoneVerified = customer.MobilePhoneVerified,
                Password = customer.Password,
                EmailVerified = customer.EmailVerified,
                EmailVerificationCode = customer.EmailVerificationCode,
                ZipCode = customer.ZipCode,
                PinCodeExpriration = customer.PinCodeExpriration
            };

            // await connection.ExecuteAsync("dbo.Customer_Upsert", customer, transaction, commandType: CommandType.StoredProcedure);

            await connection.ExecuteAsync("dbo.Customer_Upsert", payload, transaction, commandType: CommandType.StoredProcedure);

            //if (customer.Devices != null && customer.Devices.Any())
            //{
            //    foreach (var device in customer.Devices)
            //    {
            //        await DeviceDAL.SaveDevice(device, connection, transaction);
            //    }
            //}
            //if (customer.AzureCustomerBannerMetadata != null && customer.AzureCustomerBannerMetadata.Any())
            //{
            //    foreach (var metadata in customer.AzureCustomerBannerMetadata)
            //    {
            //        await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(metadata, connection, transaction);
            //    }
            //}
        }



        internal static async Task SaveCustomerMobile(AzureCustomer customer, IDbConnection connection, IDbTransaction transaction = null)
        {
            //create anonymous object 
            var payload = new
            {
                MemberId = customer.MemberId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                MobilePhoneNumber = customer.MobilePhoneNumber,
                AliasNumber = customer.AliasNumber,
                MobilePhoneVerified = customer.MobilePhoneVerified,
                Password = customer.Password,
                EmailVerified = customer.EmailVerified,
                EmailVerificationCode = customer.EmailVerificationCode,
                ZipCode = customer.ZipCode,
                PinCodeExpriration = customer.PinCodeExpriration
            };

            // await connection.ExecuteAsync("dbo.Customer_Upsert", customer, transaction, commandType: CommandType.StoredProcedure);

            await connection.ExecuteAsync("dbo.Customer_Upsert", payload, transaction, commandType: CommandType.StoredProcedure);

            if (customer.Devices != null && customer.Devices.Any())
            {
                foreach (var device in customer.Devices)
                {
                    await DeviceDAL.SaveDevice(device, connection, transaction);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by email. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">        The email. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by email. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<AzureCustomer> GetCustomerByEmail(string email, IDbConnection connection, IDbTransaction transaction = null)
        {

            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "C.EmailAddress=@email", email}

            };

            return await GetCustomerByDictionary(request, connection, transaction);


        }
        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Queries if a given customer exists. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="MemberId">     Identifier for the member. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> CustomerExists(string MemberId, IDbConnection connection, IDbTransaction transaction = null)
        {

            var result = await connection.ExecuteScalarAsync<int>("Select count(*) from Customer (nolock) where MemberID=@MemberId", new { MemberId }, transaction).ConfigureAwait(false);
            return (result > 0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Queries if a given customer exists. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="MemberId"> Identifier for the member. </param>
        ///
        /// <returns>   An asynchronous result that yields true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<bool> CustomerExists(string MemberId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await CustomerExists(MemberId, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customers by email or phone. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="emailOrPhone"> The email or phone. </param>
        ///
        /// <returns>   An asynchronous result that yields the customers by email or phone. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AzureCustomer>> GetCustomersByEmailOrPhone(string emailOrPhone)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomersByEmailOrPhone(emailOrPhone, db).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <returns></returns>
        public static async Task<List<AzureCustomer>> GetCustomersByEmailOrPhoneMobile(string emailOrPhone)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomersByEmailOrPhoneMobile(emailOrPhone, db).ConfigureAwait(false);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by alias number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber"> The email or phone. </param>
        ///
        /// <returns>   An asynchronous result that yields the customers by AliasNumber. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AzureCustomer>> GetCustomerByAliasNumber(string AliasNumber)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return await GetCustomerByAliasNumber(AliasNumber, db).ConfigureAwait(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customers by email or phone. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="emailOrPhone"> The email or phone. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customers by email or phone. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AzureCustomer>> GetCustomersByEmailOrPhone(string emailOrPhone, IDbConnection connection, IDbTransaction transaction = null)
        {

            //emailOrPhone = emailOrPhone.Replace("+", "");

            //List<string> PhoneNumbers = new List<string>();

            //if (System.Text.RegularExpressions.Regex.IsMatch(emailOrPhone, @"^\d+$"))
            //{
            //    emailOrPhone.TrimStart('1');
            //}

            //Dictionary<string, object> request = new Dictionary<string, object>() {
            //    { "C.EmailAddress=@EmailOrPhone", emailOrPhone },
            //    { "C.MobilePhoneNumber like '%' + @EmailOrPhone", emailOrPhone } };

            //// return await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");

            ///*
            // * For backward compatibility., changed it to a list , although the GetCustomerByDictionary returns only one object
            // * 
            // * */

            //var cust = await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");
            //List<AzureCustomer> lst = new List<AzureCustomer>();
            //if (cust != null)
            //{
            //    lst.Add(cust);
            //}
            //return lst;



            Dictionary<string, object> request = null;
            List<AzureCustomer> lst = new List<AzureCustomer>();

            if (!string.IsNullOrEmpty(emailOrPhone))
            {
                emailOrPhone = emailOrPhone.Trim();

                emailOrPhone = emailOrPhone.Replace("+", "");
                //if (!emailOrPhone.Contains("@"))
                //{
                //    emailOrPhone.TrimStart('1');
                //}


                List<string> PhoneNumbers = new List<string>();

                if (System.Text.RegularExpressions.Regex.IsMatch(emailOrPhone, @"^\d+$") && emailOrPhone.Count() > 10)
                {
                    emailOrPhone = emailOrPhone.TrimStart('1');
                }

                //-------------------------------------------------------------------------------------------------commented out the like operation 
                //Dictionary<string, object> request = new Dictionary<string, object>() {
                //    { "C.EmailAddress=@EmailOrPhone", emailOrPhone },
                //    { "C.MobilePhoneNumber like '%' + @EmailOrPhone", emailOrPhone } };

                if (emailOrPhone.Contains("@"))
                {
                    request = new Dictionary<string, object>()
                {
                    { "C.EmailAddress=@EmailOrPhone", emailOrPhone }
                };
                }
                else
                {
                    string phoneNumberwithplusone = string.Format("+1{0}", emailOrPhone);
                    string phoneNumberwithone = string.Format("1{0}", emailOrPhone);
                    string phoneNumberwithspace = string.Format(" {0}", emailOrPhone);

                    request = new Dictionary<string, object>()
                {

                    { "C.MobilePhoneNumber = @EmailOrPhone", emailOrPhone },
                    {"C.MobilePhoneNumber = @phoneNumberwithplusone" , phoneNumberwithplusone },
                    {"C.MobilePhoneNumber = @phoneNumberwithone" , phoneNumberwithone},
                    {"C.MobilePhoneNumber = @phoneNumberwithspace" , phoneNumberwithspace}
                };

                }

                // return await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");

                /*
                 * For backward compatibility., changed it to a list , although the GetCustomerByDictionary returns only one object
                 * 
                 * */

                if (request != null)
                {
                    var cust = await GetCustomerByDictionary(request, connection, transaction, true);

                    if (cust != null)
                    {
                        lst.Add(cust);
                    }
                }
            }
            return lst;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<List<AzureCustomer>> GetCustomersByEmailOrPhoneMobile(string emailOrPhone, IDbConnection connection, IDbTransaction transaction = null)
        {

            //emailOrPhone = emailOrPhone.Replace("+", "");

            //List<string> PhoneNumbers = new List<string>();

            //if (System.Text.RegularExpressions.Regex.IsMatch(emailOrPhone, @"^\d+$"))
            //{
            //    emailOrPhone.TrimStart('1');
            //}

            //Dictionary<string, object> request = new Dictionary<string, object>() {
            //    { "C.EmailAddress=@EmailOrPhone", emailOrPhone },
            //    { "C.MobilePhoneNumber like '%' + @EmailOrPhone", emailOrPhone } };

            //// return await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");

            ///*
            // * For backward compatibility., changed it to a list , although the GetCustomerByDictionary returns only one object
            // * 
            // * */

            //var cust = await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");
            //List<AzureCustomer> lst = new List<AzureCustomer>();
            //if (cust != null)
            //{
            //    lst.Add(cust);
            //}
            //return lst;



            Dictionary<string, object> request = null;
            List<AzureCustomer> lst = new List<AzureCustomer>();

            if (!string.IsNullOrEmpty(emailOrPhone))
            {
                emailOrPhone = emailOrPhone.Trim();

                emailOrPhone = emailOrPhone.Replace("+", "");
                //if (!emailOrPhone.Contains("@"))
                //{
                //    emailOrPhone.TrimStart('1');
                //}


                List<string> PhoneNumbers = new List<string>();

                if (System.Text.RegularExpressions.Regex.IsMatch(emailOrPhone, @"^\d+$") && emailOrPhone.Count() > 10)
                {
                    emailOrPhone = emailOrPhone.TrimStart('1');
                }

                //-------------------------------------------------------------------------------------------------commented out the like operation 
                //Dictionary<string, object> request = new Dictionary<string, object>() {
                //    { "C.EmailAddress=@EmailOrPhone", emailOrPhone },
                //    { "C.MobilePhoneNumber like '%' + @EmailOrPhone", emailOrPhone } };

                if (emailOrPhone.Contains("@"))
                {
                    request = new Dictionary<string, object>()
                {
                    { "C.EmailAddress=@EmailOrPhone", emailOrPhone }
                };
                }
                else
                {
                    string phoneNumberwithplusone = string.Format("+1{0}", emailOrPhone);
                    string phoneNumberwithone = string.Format("1{0}", emailOrPhone);
                    string phoneNumberwithspace = string.Format(" {0}", emailOrPhone);

                    request = new Dictionary<string, object>()
                {

                    { "C.MobilePhoneNumber = @EmailOrPhone", emailOrPhone },
                    {"C.MobilePhoneNumber = @phoneNumberwithplusone" , phoneNumberwithplusone },
                    {"C.MobilePhoneNumber = @phoneNumberwithone" , phoneNumberwithone},
                    {"C.MobilePhoneNumber = @phoneNumberwithspace" , phoneNumberwithspace}
                };

                }

                // return await GetCustomerByDictionary(request, connection, transaction, true, "c.EmailAddress=@EmailOrPhone OR C.MobilePhoneNumber like '%' + @EmailOrPhone");

                /*
                 * For backward compatibility., changed it to a list , although the GetCustomerByDictionary returns only one object
                 * 
                 * */

                if (request != null)
                {
                    lst = await GetCustomerByDictionaryListMobile(request, connection, transaction, true);

                    //if (cust != null)
                    //{
                    //    lst.Add(cust);
                    //}
                }
            }
            return lst;

        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets customer by alias number. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="AliasNumber">  The alias number. </param>
        /// <param name="connection">   The connection. </param>
        /// <param name="transaction">  (Optional) The transaction. </param>
        ///
        /// <returns>   An asynchronous result that yields the customer by alias number. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task<List<AzureCustomer>> GetCustomerByAliasNumber(string AliasNumber, IDbConnection connection, IDbTransaction transaction = null)
        {
            Dictionary<string, object> request = new Dictionary<string, object>() {
                { "C.AliasNumber=@AliasNumber", AliasNumber}
            };

            var customer = await AzureCustomerDAL.GetCustomerByDictionary(request, connection, transaction);
            List<AzureCustomer> lst = new List<AzureCustomer>();
            if (customer != null)
            {
                lst.Add(customer);
            }
            return lst;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the store. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId"> Identifier for the member. </param>
        /// <param name="chainId">  Identifier for the chain. </param>
        /// <param name="storeId">  Identifier for the store. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task UpdateStore(string memberId, string chainId, int storeId)
        {

            AzureCustomerBannerMetadata customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, chainId).ConfigureAwait(false);

            if (customerBanner == null)
            {
                switch (chainId)
                {
                    case "2":

                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "3").ConfigureAwait(false);
                        if (customerBanner != null)
                        {
                            customerBanner = new AzureCustomerBannerMetadata()
                            {
                                ChainId = "2",
                                CouponAlias = customerBanner.CouponAlias,
                                CouponId = customerBanner.CouponId,
                                ShoppingListId = customerBanner.ShoppingListId,
                                MemberId = customerBanner.MemberId,
                                CreateDateTime = DateTime.Now,
                                StoreNumber = storeId,
                                LastUpdateDateTime = DateTime.Now,
                            };

                        }
                        break;
                    case "3":
                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "2").ConfigureAwait(false);

                        if (customerBanner != null)
                        {
                            customerBanner = new AzureCustomerBannerMetadata()
                            {
                                ChainId = "3",
                                CouponAlias = customerBanner.CouponAlias,
                                CouponId = customerBanner.CouponId,
                                ShoppingListId = customerBanner.ShoppingListId,
                                MemberId = customerBanner.MemberId,
                                CreateDateTime = DateTime.Now,
                                StoreNumber = storeId,
                                LastUpdateDateTime = DateTime.Now
                            };

                        }
                        break;
                    default:
                        break;
                }

                //var customer = azureLoyaltyDBContext.AzureCustomers.Where(a => a.MemberId == memberId).FirstOrDefault();


            }

            customerBanner.StoreNumber = storeId;
            customerBanner.LastUpdateDateTime = DateTime.Now;
            await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(customerBanner);



        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the coupon identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">     Identifier for the member. </param>
        /// <param name="chainId">      Identifier for the chain. </param>
        /// <param name="couponId">     Identifier for the coupon. </param>
        /// <param name="couponAlias">  The coupon alias. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task UpdateCouponId(string memberId, string chainId, string couponId, string couponAlias)
        {
            if (string.IsNullOrWhiteSpace(chainId)) return;

            AzureCustomerBannerMetadata customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, chainId).ConfigureAwait(false);
            if (customerBanner == null)
            {
                switch (chainId)
                {
                    case "2":
                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "3").ConfigureAwait(false);
                        break;
                    case "3":
                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "2").ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }



            if (customerBanner != null)
            {
                customerBanner.CouponId = couponId;
                customerBanner.CouponAlias = couponAlias;
            }
            else
            {

                customerBanner = new AzureCustomerBannerMetadata()
                {
                    CouponAlias = couponAlias,
                    MemberId = memberId,
                    ChainId = chainId,
                    CreateDateTime = DateTime.Now,
                    LastUpdateDateTime = DateTime.Now,
                    CouponId = couponId
                };
            }

            await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(customerBanner);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the shopping list identifier by member identifier and chain identifier.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="memberId">         Identifier for the member. </param>
        /// <param name="chainId">          Identifier for the chain. </param>
        /// <param name="shoppingListId">   Identifier for the shopping list. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task UpdateShoppingListIdByMemberIdAndChainId(string memberId, string chainId, string shoppingListId)
        {

            AzureCustomerBannerMetadata customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, chainId).ConfigureAwait(false);
            if (customerBanner == null)
            {
                switch (chainId)
                {
                    case "2":
                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "3").ConfigureAwait(false);
                        break;
                    case "3":
                        customerBanner = await AzureCustomerBannerMetadataDAL.GetByMemberIdAndChainId(memberId, "2").ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }


            if (customerBanner != null)
            {
                customerBanner.ShoppingListId = shoppingListId;
                await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(customerBanner);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the shopping list identifier by email and chain identifier. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="email">            The email. </param>
        /// <param name="chainId">          Identifier for the chain. </param>
        /// <param name="shoppingListId">   Identifier for the shopping list. </param>
        ///
        /// <returns>   An asynchronous result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static async Task UpdateShoppingListIdByEmailAndChainId(string email, string chainId, string shoppingListId)
        {

            AzureCustomer customer = await AzureCustomerDAL.GetCustomerByEmail(email).ConfigureAwait(false);
            if (customer != null)
            {
                if (customer.AzureCustomerBannerMetadata != null)
                {
                    AzureCustomerBannerMetadata data = customer.AzureCustomerBannerMetadata.FirstOrDefault(x => x.ChainId.Trim() == chainId.Trim()
                        && x.MemberId == customer.MemberId);
                    if (data != null)
                    {
                        data.ShoppingListId = shoppingListId;
                        await AzureCustomerBannerMetadataDAL.SaveCustomerBannerMetadata(data);

                    }
                }
            }

        }

        #endregion Public Methods

        #region Private Helper Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerToUpdate"> The customer to update. </param>
        /// <param name="existingCustomer"> The existing customer. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static async Task UpdateCustomer(AzureCustomer customerToUpdate, AzureCustomerDetail existingCustomer)
        {
            //update existing user           
            if (!String.IsNullOrEmpty(existingCustomer.EmailAddress))
            {
                customerToUpdate.EmailAddress = existingCustomer.EmailAddress;
            }
            if (!String.IsNullOrEmpty(existingCustomer.FirstName))
            {
                customerToUpdate.FirstName = existingCustomer.FirstName;
            }
            if (!String.IsNullOrEmpty(existingCustomer.LastName))
            {
                customerToUpdate.LastName = existingCustomer.LastName;
            }
            if (!String.IsNullOrEmpty(existingCustomer.MobilePhoneNumber))
            {
                customerToUpdate.MobilePhoneNumber = existingCustomer.MobilePhoneNumber;
            }
            //update AliasNumber if it's null in azure tabele
            if (!String.IsNullOrEmpty(existingCustomer.AliasNumber))
            {
                customerToUpdate.AliasNumber = existingCustomer.AliasNumber;
            }

            customerToUpdate.LastUpdateDateTime = DateTime.Now;
            customerToUpdate.AzureCustomerBannerMetadata = null;//don't save AzureCustomerBannerMetadata here

            await SaveCustomer(customerToUpdate).ConfigureAwait(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Map customer to customer detail. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customer"> The customer. </param>
        ///
        /// <returns>   An AzureCustomerDetail. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static AzureCustomerDetail MapCustomerToCustomerDetail(AzureCustomer customer)
        {
            AzureCustomerBannerMetadata banner = customer.AzureCustomerBannerMetadata.First();

            AzureCustomerDetail detail = new AzureCustomerDetail()
            {
                AliasNumber = customer.AliasNumber,
                ChainId = banner.ChainId,
                BannerDetailModifiedDate = banner.LastUpdateDateTime,
                CouponAlias = banner.CouponAlias,
                CreatedDate = customer.CreateDateTime,
                CustomerDetailModifiedDate = customer.LastUpdateDateTime,
                EmailAddress = customer.EmailAddress,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                MemberId = customer.MemberId,
                MobilePhoneNumber = customer.MobilePhoneNumber,
                ShoppingListId = banner.ShoppingListId,
                StoreId = banner.StoreNumber,
                CouponId = banner.CouponId

            };

            return detail;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Map customer detail to banner. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerDetail">   The customer detail. </param>
        ///
        /// <returns>   An AzureCustomerBannerMetadata. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static AzureCustomerBannerMetadata MapCustomerDetailToBanner(AzureCustomerDetail customerDetail)
        {
            string couponAlias = customerDetail.CouponAlias;
            if (customerDetail.ChainId == Banner.WD.GetAttribute<ChainIdAttribute>().Value)
            {
                if (!String.IsNullOrEmpty(customerDetail.CouponAlias) && !customerDetail.CouponAlias.StartsWith(WinnDixieCouponAliasPrefix))
                {
                    //for winndixie we need to preappend the 9800 to the alias number
                    couponAlias = string.Format("{0}{1}", WinnDixieCouponAliasPrefix, customerDetail.CouponAlias);
                }
            }

            AzureCustomerBannerMetadata banner = new AzureCustomerBannerMetadata()
            {
                ChainId = customerDetail.ChainId,
                CouponAlias = couponAlias,
                MemberId = customerDetail.MemberId,
                LastUpdateDateTime = customerDetail.BannerDetailModifiedDate ?? DateTime.Now,
                ShoppingListId = customerDetail.ShoppingListId,
                StoreNumber = customerDetail.StoreId,
                CouponId = customerDetail.CouponId,

                //added support for Create date -- Sam 02/14/2018
                CreateDateTime = customerDetail.CreatedDate

            };

            return banner;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Map customer detail to customer. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="customerDetail">   The customer detail. </param>
        ///
        /// <returns>   An AzureCustomer. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static AzureCustomer MapCustomerDetailToCustomer(AzureCustomerDetail customerDetail)
        {
            AzureCustomer customer = new AzureCustomer()
            {
                AliasNumber = customerDetail.AliasNumber ?? customerDetail.CouponAlias,
                LastUpdateDateTime = customerDetail.CustomerDetailModifiedDate ?? DateTime.Now,
                MemberId = customerDetail.MemberId,
                EmailAddress = customerDetail.EmailAddress,
                FirstName = customerDetail.FirstName,
                LastName = customerDetail.LastName,
                MobilePhoneNumber = customerDetail.MobilePhoneNumber,

                //aDDED Created Date Too..
                CreateDateTime = customerDetail.CreatedDate
            };

            return customer;
        }

        #region IDisposable Support
        private bool disposedValue = false; ///< To detect redundant calls

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Releases the unmanaged resources used by the
        /// SEG.AzureLoyaltyDatabase.DataAccess.AzureCustomerDAL and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="disposing">    True to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {


                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AzureCustomerDAL() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   This code added to correctly implement the disposable pattern. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <seealso cref="M:System.IDisposable.Dispose()"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #endregion Private Helper Methods
    }
}