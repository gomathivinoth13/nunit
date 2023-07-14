using Newtonsoft.Json;
using SEG.ApiService.Models.AuditCustomerServicePoints;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SEG.EagleEyeLibrary.Models.CustomerCareCenter
{
    public class PointsProcessCustomerServiceResponse
    {
        [JsonProperty(PropertyName = "auditProcessPoints", NullValueHandling = NullValueHandling.Ignore)]
        public virtual AuditProcessPoints AuditProcessPoints { get; set; }

        [JsonProperty(PropertyName = "auditCustomerServiceTicket", NullValueHandling = NullValueHandling.Ignore)]
        public virtual AuditCustomerServiceTicket AuditCustomerServiceTicket { get; set; }

        [JsonProperty(PropertyName = "auditCustomerServiceRep", NullValueHandling = NullValueHandling.Ignore)]
        public virtual AuditCustomerServiceRep AuditCustomerServiceRep { get; set; }

        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(PropertyName = "errorMessages", NullValueHandling = NullValueHandling.Ignore)]
        public string[] ErrorMessages { get; set; }

        [JsonProperty(PropertyName = "statusCode", NullValueHandling = NullValueHandling.Ignore)]
        public HttpStatusCode StatusCode { get; set; }
    }
}
