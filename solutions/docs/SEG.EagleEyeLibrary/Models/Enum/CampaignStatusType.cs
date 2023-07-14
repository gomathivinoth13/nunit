using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SEG.EagleEyeLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum CampaignStatusType
    {
        [Description("Active")]
        ACTIVE,
        [Description("InActive")]
        INACTIVE,
        [Description("Pending")]
        PENDING,
        [Description("Expired")]
        EXPIRED,
        [Description("Deleted")]
        DELETED,
        [Description("Draft")]
        DRAFT,
        [Description("Rejected")]
        REJECTED,
        [Description("Ready")]
        READY
    }
}
