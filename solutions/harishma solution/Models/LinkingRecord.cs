﻿////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Models\LinkingRecord.cs
//
// summary:	Implements the linking record class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.LoyaltyService.Process.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Information about the linking. </summary>
    ///
    /// <remarks>   Mcdand, 2/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class LinkingRecord
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the CRC. </summary>
        ///
        /// <value> The identifier of the CRC. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string CRC_ID { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the chain. </summary>
        ///
        /// <value> The identifier of the chain. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string CHAIN_ID { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the member. </summary>
        ///
        /// <value> The identifier of the member. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string MEMBER_ID { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the enrollment location. </summary>
        ///
        /// <value> The identifier of the enrollment location. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int ENROLLMENT_LOC_ID { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the enrollment date. </summary>
        ///
        /// <value> The enrollment date. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public DateTime ENROLLMENT_DATE { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the mobile phone. </summary>
        ///
        /// <value> The mobile phone. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string MOBILE_PHONE { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the alias number. </summary>
        ///
        /// <value> The alias number. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ALIAS_NUMBER { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the enrollment status. </summary>
        ///
        /// <value> The enrollment status. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ENROLLMENT_STATUS { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether this object is updated. </summary>
        ///
        /// <value> True if updated, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool Updated { get; set; }
    }
   
}