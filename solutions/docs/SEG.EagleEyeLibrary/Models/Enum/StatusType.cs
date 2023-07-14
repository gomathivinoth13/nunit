using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace SEG.EagleEyeLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum StatusType
    {
        [StatusAttribute("Active")]
        ACTIVE,

        [StatusAttribute("Blocked")]
        BLOCKED,

        [StatusAttribute("Cancelled")]
        CANCELLED,

        [StatusAttribute("Deleted")]
        DELETED,

        [StatusAttribute("Expired")]
        EXPIRED,

        [StatusAttribute("InActive")]
        INACTIVE,

        [StatusAttribute("InValidated")]
        INVALIDATED,

        [StatusAttribute("Locked")]
        LOCKED,

        [StatusAttribute("Stolen")]
        STOLEN,

        [StatusAttribute("Used")]
        USED
    }
}
