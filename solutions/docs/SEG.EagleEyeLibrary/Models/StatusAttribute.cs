﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SEG.EagleEyeLibrary.Models
{
    public class StatusAttribute : Attribute
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/19/2018. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public StatusAttribute(string value)
        {
            Value = value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the value. </summary>
        ///
        /// <value> The value. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string Value { get; set; }
    }
}
