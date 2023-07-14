using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushNotificationSFMCFunctionApp.Middleware
{
    public static class ApplicationInsightExtensions
    {
        public static IApplicationBuilder UseRequestBodyLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestBodyLoggingMiddleware>();
        }
    }
}
