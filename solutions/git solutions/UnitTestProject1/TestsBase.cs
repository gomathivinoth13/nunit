﻿////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	testsbase.cs
//
// summary:	Implements the testsbase class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.AzureLoyaltyDatabase.Tests
{
   ////////////////////////////////////////////////////////////////////////////////////////////////////
   /// <summary>    The tests base. </summary>
   ///
   /// <remarks>    Mcdand, 2/20/2018. </remarks>
   ////////////////////////////////////////////////////////////////////////////////////////////////////

   public abstract class TestsBase
    {
        static string LoyaltyDBConnectionString = "Server=tcp:tablestorageexport.database.windows.net,1433;Database=Loyalty_QA;User ID = tablestorageadmin@tablestorageexport;Password=Admin123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";   ///< .

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Static constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        static TestsBase()
        {
            SEG.AzureLoyaltyDatabase.DataAccess.DapperDalBase.ConnectionString = LoyaltyDBConnectionString;
        }
    }
}
