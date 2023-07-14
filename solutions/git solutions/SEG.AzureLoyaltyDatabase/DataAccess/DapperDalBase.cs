////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	DataAccess\DapperDalBase.cs
//
// summary:	Implements the dapper dal base class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.AzureLoyaltyDatabase.DataAccess
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A dapper dal base. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract class DapperDalBase
    {
        static string _connectionString;    ///< The connection string
        static string _connectionStringMBO;
        static string _connectionStringSurvey;
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the connection string. </summary>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <value> The connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        static public string ConnectionString { get {
                if (string.IsNullOrWhiteSpace(_connectionString)) {
                    throw new ArgumentNullException("ConnectionString", "Connection String must be defined");
                }
                return _connectionString; } set { _connectionString = value; } }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Mystery Bonus Offers connection string. </summary>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <value> The connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        static public string ConnectionStringMBO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionStringMBO))
                {
                    throw new ArgumentNullException("ConnectionString", "Connection String must be defined");
                }
                return _connectionStringMBO;
            }
            set { _connectionStringMBO = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Mystery Bonus Offers connection string. </summary>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <value> The connection string. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        static public string ConnectionStringSurvey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionStringSurvey))
                {
                    throw new ArgumentNullException("ConnectionStringSurvey", "Connection String must be defined");
                }
                return _connectionStringSurvey;
            }
            set { _connectionStringSurvey = value; }
        }

#if NET45
        static DapperDalBase()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["LoyaltyConnection"].ConnectionString;
        }
 
#endif

    }
}
