﻿
namespace WalletAccountDataProcessorFunctionApp.Models
{
    internal class ResponseMessage
    {
        internal  static readonly string SuccessMessage               = "Data Inserted Successfully to SFMC";
        internal static readonly string ErrorMessage                  = "Error Occured While inserting Value";
        internal static readonly string NullErrorMessage              = "Input was null, Can't process the request";
        internal static readonly string RequestEntityTooLargeMessage  = "RequestEntity is too large ,Can't process the request";
        internal static readonly string DataBaseErrorMessage          = "Error while inserting data to database";
        internal static readonly string DataBaseSuccessMessage        = "Event log data  successfully inserted to Database ";
        internal static readonly string AccountIdNullErrorMessage     = "DataList is null, Can't insert Value";
        internal static readonly string ConfigValueNullErrorMessage   = "Configuration value SEG_Key is missing.";
        internal static readonly string Source_Create                 = "received event.";
        internal static readonly string Source_Process                = "processed event.";
    }
}
