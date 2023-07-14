#region Assembly SalesForceLibrary, Version=2022.5.8.1, Culture=neutral, PublicKeyToken=null
// C:\Users\gomathi.thangavel\.nuget\packages\salesforcelibrary\2022.5.8.1\lib\netstandard2.0\SalesForceLibrary.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using SalesForceLibrary.Models;
using SEG.ApiService.Models.Pii;
using SEG.ApiService.Models.SalesForce;
using SEG.ApiService.Models.Surveys;
using SEG.SalesForce.Controllers;
using SEG.SalesForce.Models;
using SEG.Shared;

namespace SalesForceLibrary.SalesForceAPIM
{
    public class SalesForceAPIMService
    {
        //
        // Summary:
        //     The logging.
        private ILog Logging = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private ManageAccessTokenV2 manageAccessToken;

        private SalesForceRestAPIMDAL serviceDAL;

        //
        // Parameters:
        //   baseRestUrlAuth:
        //
        //   baseRestUrl:
        //
        //   clientID:
        //
        //   clientSecret:
        //
        //   cacheConnectionString:
        //
        //   ocpApimSubscriptionKey:
        public SalesForceAPIMService(string baseRestUrlAuth, string baseRestUrl, string clientID, string clientSecret, string cacheConnectionString, string ocpApimSubscriptionKey)
        {
            serviceDAL = new SalesForceRestAPIMDAL(baseRestUrl, clientID, clientSecret, ocpApimSubscriptionKey);
            manageAccessToken = new ManageAccessTokenV2(baseRestUrlAuth, clientID, clientSecret, cacheConnectionString, ocpApimSubscriptionKey);
        }

        public async Task<ContactResponse> DeletePiiAsync(string memberId)
        {
            ContactResponse response = null;
            try
            {
                ContactRequest request = new ContactRequest
                {
                    Values = new List<string> { memberId }
                };
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    response = await serviceDAL.DeletePiiAsync(text, request).ConfigureAwait(continueOnCapturedContext: false);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> UpsertProductSurveyAsync(ProductSurvey survey, string keyBU)
        {
            DataExtentionsResponse response = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    ProductSurveyItem item = new ProductSurveyItem
                    {
                        Banner = survey.Banner,
                        BuyItAgain = survey.BuyItAgain,
                        MemberId = survey.MemberId,
                        CrcId = survey.CrcId,
                        LocId = survey.LocId,
                        UpcCode = survey.UpcCode,
                        TransactionDateTime = survey.TransactionDateTime,
                        Satisfaction = survey.Satisfaction,
                        Size = survey.Size,
                        Taste = survey.Taste,
                        Packaging = survey.Packaging,
                        VisualAppeal = survey.VisualAppeal,
                        Comments = survey.Comments
                    };
                    DataExtensionsProductSurveyRequest dataExtensionsProductSurveyRequest = new DataExtensionsProductSurveyRequest();
                    dataExtensionsProductSurveyRequest.items.Add(item);
                    return await serviceDAL.UpsertAsyncProductSurvey(text, dataExtensionsProductSurveyRequest, keyBU);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Summary:
        //     Upsert the EE ProductData To SFMC
        public async Task<DataExtentionsResponse> UpsertAsyncEEProductData(DataExtensionsEEGroupRequest dataExtensionsEEGroupRequest, DataExtensionsEETagRequest dataExtensionsEETagRequest, DataExtensionsEEUPCRequest dataExtensionsEEUPCRequest, string keyGroup, string keyTag, string keyUpc)
        {
            DataExtentionsResponse response = null;
            try
            {
                string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    if (dataExtensionsEEUPCRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEEUpcData(accessToken, dataExtensionsEEUPCRequest, keyUpc);
                    }

                    if (dataExtensionsEETagRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEETagData(accessToken, dataExtensionsEETagRequest, keyTag);
                    }

                    if (dataExtensionsEEGroupRequest.items.Count > 0)
                    {
                        response = await serviceDAL.UpsertAsyncEEGroupData(accessToken, dataExtensionsEEGroupRequest, keyGroup);
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Summary:
        //     Upsert the EE ProductData To SFMC
        public async Task<DataExtentionsResponse> UpsertAsyncEECampaign(DataExtensionsEECampaignRequest eeCampaign, string keyBU)
        {
            DataExtentionsResponse response = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.UpsertAsyncEECampaign(text, eeCampaign, keyBU);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<pushNotificationResponse>> SendPushNotification(string messageID, SendPushNotificationRequest request)
        {
            Response<pushNotificationResponse> response = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.SendPushNotification(text, messageID, request).ConfigureAwait(continueOnCapturedContext: false);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WelcomeJourneyResponse> SendJourney(MBOIssuanceJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse response = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.WelcomeJourneyMBO(text, welcomeJourneyRequest).ConfigureAwait(continueOnCapturedContext: false);
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> UpsertAsyncBabyClub(DataExtentionsBabyClubChildRequest dataExtentionsBabyClubChildRequest, bool babyClubFlag, string memberID, string keyBU, string babyKeyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(memberID))
                {
                    string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        DataExtentionsRequest dataExtentionsRequest = new DataExtentionsRequest();
                        Item item = new Item();
                        item.MEMBER_ID = memberID;
                        if (babyClubFlag)
                        {
                            item.Baby_Club_Flag = "Y";
                        }
                        else
                        {
                            item.Baby_Club_Flag = "N";
                        }

                        List<Item> list = new List<Item>();
                        list.Add(item);
                        dataExtentionsRequest.items = list;
                        await serviceDAL.UpsertAsync(accessToken, dataExtentionsRequest, keyBU).ConfigureAwait(continueOnCapturedContext: false);
                        if (dataExtentionsBabyClubChildRequest != null && dataExtentionsBabyClubChildRequest.items != null && dataExtentionsBabyClubChildRequest.items.Count > 0)
                        {
                            dataExtentionsResponse = await serviceDAL.UpsertAsyncBabyClub(accessToken, dataExtentionsBabyClubChildRequest, babyKeyBU).ConfigureAwait(continueOnCapturedContext: false);
                        }
                    }

                    return dataExtentionsResponse;
                }

                return dataExtentionsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> UpsertAsyncPetClub(DataExtentionsPetClubChildRequest dataExtentionsPetClubChildRequest, bool petClubFlag, string memberID, string keyBU, string petKeyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(memberID))
                {
                    string accessToken = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        DataExtentionsRequest dataExtentionsRequest = new DataExtentionsRequest();
                        Item item = new Item();
                        item.MEMBER_ID = memberID;
                        if (petClubFlag)
                        {
                            item.Pet_Club_Flag = "Y";
                        }
                        else
                        {
                            item.Pet_Club_Flag = "N";
                        }

                        List<Item> list = new List<Item>();
                        list.Add(item);
                        dataExtentionsRequest.items = list;
                        await serviceDAL.UpsertAsync(accessToken, dataExtentionsRequest, keyBU).ConfigureAwait(continueOnCapturedContext: false);
                        if (dataExtentionsPetClubChildRequest != null && dataExtentionsPetClubChildRequest.items != null && dataExtentionsPetClubChildRequest.items.Count > 0)
                        {
                            dataExtentionsResponse = await serviceDAL.UpsertAsyncPetClub(accessToken, dataExtentionsPetClubChildRequest, petKeyBU).ConfigureAwait(continueOnCapturedContext: false);
                        }
                    }

                    return dataExtentionsResponse;
                }

                return dataExtentionsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> UpsertAsync(DataExtentionsRequest dataExtentionsRequest, string keyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.UpsertAsync(text, dataExtentionsRequest, keyBU).ConfigureAwait(continueOnCapturedContext: false);
                }

                return dataExtentionsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataExtentionsResponse> UpsertAsyncPreferences(DataExtentionsCustomerPreferenceRequest dataExtentionsCustomerPreferenceRequest, string keyBU)
        {
            DataExtentionsResponse dataExtentionsResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.UpsertAsyncPreferences(text, dataExtentionsCustomerPreferenceRequest, keyBU).ConfigureAwait(continueOnCapturedContext: false);
                }

                return dataExtentionsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Parameters:
        //   messageKey:
        //
        //   messaging:
        public async Task<MessagingResponse> Send(string messageKey, Messaging messaging)
        {
            MessagingResponse messagingResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.Send(text, messageKey, messaging).ConfigureAwait(continueOnCapturedContext: false);
                }

                return messagingResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Parameters:
        //   messageKey:
        //
        //   messaging:
        public async Task<SmsMessagingResponse> SendSMS(string messageKey, SmsMessaging messaging)
        {
            SmsMessagingResponse smsMessagingResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.SendSMS(text, messageKey, messaging).ConfigureAwait(continueOnCapturedContext: false);
                }

                return smsMessagingResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Parameters:
        //   welcomeJourneyRequest:
        public async Task<WelcomeJourneyResponse> WelcomeJourney(WelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    return await serviceDAL.WelcomeJourney(text, welcomeJourneyRequest).ConfigureAwait(continueOnCapturedContext: false);
                }

                return welcomeJourneyResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // Parameters:
        //   welcomeJourneyRequest:
        public async Task<WelcomeJourneyResponse> PosWelcomeJourney(POSWelcomeJourneyRequest welcomeJourneyRequest)
        {
            WelcomeJourneyResponse welcomeJourneyResponse = null;
            try
            {
                string text = await manageAccessToken.GetAccessToken().ConfigureAwait(continueOnCapturedContext: false);
                if (!string.IsNullOrEmpty(text))
                {
                    welcomeJourneyResponse = await serviceDAL.PosWelcomeJourney(text, welcomeJourneyRequest).ConfigureAwait(continueOnCapturedContext: false);
                    return welcomeJourneyResponse;
                }

                return welcomeJourneyResponse;
            }
            catch (Exception ex)
            {
                Logging.Error($"An error occured while trying to run SalesForce_WelcomeJourney.  Error {ex.Message}", ex);
                return welcomeJourneyResponse;
            }
        }
    }
}
#if false // Decompilation log
'365' items in cache
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '2.1.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\netstandard.dll'
------------------
Resolve: 'log4net, Version=2.0.8.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
Found single assembly: 'log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
WARN: Version mismatch. Expected: '2.0.8.0', Got: '2.0.15.0'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\log4net\2.0.15\lib\netstandard2.0\log4net.dll'
------------------
Resolve: 'SEG.ApiService.Models, Version=2022.4.22.1, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'SEG.ApiService.Models, Version=2023.1.24.3, Culture=neutral, PublicKeyToken=null'
WARN: Version mismatch. Expected: '2022.4.22.1', Got: '2023.1.24.3'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\seg.apiservice.models\2023.1.24.3\lib\netstandard2.0\SEG.ApiService.Models.dll'
------------------
Resolve: 'SEG.Shared, Version=2021.2.24.2, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'SEG.Shared, Version=2022.7.26.2, Culture=neutral, PublicKeyToken=null'
WARN: Version mismatch. Expected: '2021.2.24.2', Got: '2022.7.26.2'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\seg.shared\2022.7.26.2\lib\netstandard2.0\SEG.Shared.dll'
------------------
Resolve: 'Microsoft.WindowsAzure.Storage, Version=8.7.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'Microsoft.WindowsAzure.Storage, Version=9.3.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '8.7.0.0', Got: '9.3.2.0'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\windowsazure.storage\9.3.3\lib\netstandard1.3\Microsoft.WindowsAzure.Storage.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
WARN: Version mismatch. Expected: '11.0.0.0', Got: '13.0.0.0'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\newtonsoft.json\13.0.1\lib\netstandard2.0\Newtonsoft.Json.dll'
------------------
Resolve: 'StackExchange.Redis, Version=2.0.0.0, Culture=neutral, PublicKeyToken=c219ff1ca8c2ce46'
Found single assembly: 'StackExchange.Redis, Version=2.0.0.0, Culture=neutral, PublicKeyToken=c219ff1ca8c2ce46'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\stackexchange.redis\2.5.43\lib\net5.0\StackExchange.Redis.dll'
------------------
Resolve: 'Flurl.Http, Version=2.3.1.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Flurl.Http, Version=2.3.1.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\gomathi.thangavel\.nuget\packages\flurl.http\2.3.1\lib\netstandard2.0\Flurl.Http.dll'
------------------
Resolve: 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.dll'
------------------
Resolve: 'System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.MemoryMappedFiles.dll'
------------------
Resolve: 'System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.Pipes.dll'
------------------
Resolve: 'System.Diagnostics.Process, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Process, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.Process.dll'
------------------
Resolve: 'System.Security.Cryptography.X509Certificates, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.X509Certificates, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Cryptography.X509Certificates.dll'
------------------
Resolve: 'System.Memory, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Memory, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Memory.dll'
------------------
Resolve: 'System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Collections.dll'
------------------
Resolve: 'System.Collections.NonGeneric, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.NonGeneric, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Collections.NonGeneric.dll'
------------------
Resolve: 'System.Collections.Concurrent, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Concurrent, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Collections.Concurrent.dll'
------------------
Resolve: 'System.ObjectModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.ObjectModel.dll'
------------------
Resolve: 'System.Collections.Specialized, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Specialized, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Collections.Specialized.dll'
------------------
Resolve: 'System.ComponentModel.TypeConverter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.TypeConverter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.ComponentModel.TypeConverter.dll'
------------------
Resolve: 'System.ComponentModel.EventBasedAsync, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.EventBasedAsync, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.ComponentModel.EventBasedAsync.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'System.ComponentModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.ComponentModel.dll'
------------------
Resolve: 'Microsoft.Win32.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'Microsoft.Win32.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\Microsoft.Win32.Primitives.dll'
------------------
Resolve: 'System.Console, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Console.dll'
------------------
Resolve: 'System.Data.Common, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Data.Common, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Data.Common.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Diagnostics.TraceSource, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.TraceSource, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.TraceSource.dll'
------------------
Resolve: 'System.Diagnostics.Contracts, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Contracts, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.Contracts.dll'
------------------
Resolve: 'System.Diagnostics.TextWriterTraceListener, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.TextWriterTraceListener, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.TextWriterTraceListener.dll'
------------------
Resolve: 'System.Diagnostics.FileVersionInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.FileVersionInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.FileVersionInfo.dll'
------------------
Resolve: 'System.Diagnostics.StackTrace, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.StackTrace, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.StackTrace.dll'
------------------
Resolve: 'System.Diagnostics.Tracing, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Tracing, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Diagnostics.Tracing.dll'
------------------
Resolve: 'System.Drawing.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Drawing.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Drawing.Primitives.dll'
------------------
Resolve: 'System.Linq.Expressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Expressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Linq.Expressions.dll'
------------------
Resolve: 'System.IO.Compression.Brotli, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.IO.Compression.Brotli, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.Compression.Brotli.dll'
------------------
Resolve: 'System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.Compression.dll'
------------------
Resolve: 'System.IO.Compression.ZipFile, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.IO.Compression.ZipFile, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.Compression.ZipFile.dll'
------------------
Resolve: 'System.IO.FileSystem.DriveInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem.DriveInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.FileSystem.DriveInfo.dll'
------------------
Resolve: 'System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.FileSystem.Watcher, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.FileSystem.Watcher.dll'
------------------
Resolve: 'System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.IO.IsolatedStorage.dll'
------------------
Resolve: 'System.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Linq.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Linq.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Linq.Parallel.dll'
------------------
Resolve: 'System.Threading.Thread, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Thread, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Threading.Thread.dll'
------------------
Resolve: 'System.Net.Requests, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Requests, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Requests.dll'
------------------
Resolve: 'System.Net.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Primitives.dll'
------------------
Resolve: 'System.Net.HttpListener, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.HttpListener, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.HttpListener.dll'
------------------
Resolve: 'System.Net.ServicePoint, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.ServicePoint, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.ServicePoint.dll'
------------------
Resolve: 'System.Net.NameResolution, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.NameResolution, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.NameResolution.dll'
------------------
Resolve: 'System.Net.WebClient, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.WebClient, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.WebClient.dll'
------------------
Resolve: 'System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Http.dll'
------------------
Resolve: 'System.Net.WebHeaderCollection, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.WebHeaderCollection, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.WebHeaderCollection.dll'
------------------
Resolve: 'System.Net.WebProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.WebProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.WebProxy.dll'
------------------
Resolve: 'System.Net.Mail, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.Mail, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Mail.dll'
------------------
Resolve: 'System.Net.NetworkInformation, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.NetworkInformation, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.NetworkInformation.dll'
------------------
Resolve: 'System.Net.Ping, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Ping, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Ping.dll'
------------------
Resolve: 'System.Net.Security, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Security, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Security.dll'
------------------
Resolve: 'System.Net.Sockets, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Sockets, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.Sockets.dll'
------------------
Resolve: 'System.Net.WebSockets.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.WebSockets.Client, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.WebSockets.Client.dll'
------------------
Resolve: 'System.Net.WebSockets, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.WebSockets, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Net.WebSockets.dll'
------------------
Resolve: 'System.Runtime.Numerics, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Numerics, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.Numerics.dll'
------------------
Resolve: 'System.Numerics.Vectors, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Numerics.Vectors, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Numerics.Vectors.dll'
------------------
Resolve: 'System.Reflection.DispatchProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.DispatchProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Reflection.DispatchProxy.dll'
------------------
Resolve: 'System.Reflection.Emit, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Reflection.Emit.dll'
------------------
Resolve: 'System.Reflection.Emit.ILGeneration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.ILGeneration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Reflection.Emit.ILGeneration.dll'
------------------
Resolve: 'System.Reflection.Emit.Lightweight, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Emit.Lightweight, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Reflection.Emit.Lightweight.dll'
------------------
Resolve: 'System.Reflection.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Reflection.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Reflection.Primitives.dll'
------------------
Resolve: 'System.Resources.Writer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Resources.Writer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Resources.Writer.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.VisualC, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.CompilerServices.VisualC, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.CompilerServices.VisualC.dll'
------------------
Resolve: 'System.Runtime.InteropServices.RuntimeInformation, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices.RuntimeInformation, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.InteropServices.RuntimeInformation.dll'
------------------
Resolve: 'System.Runtime.Serialization.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.Serialization.Primitives.dll'
------------------
Resolve: 'System.Runtime.Serialization.Xml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Xml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.Serialization.Xml.dll'
------------------
Resolve: 'System.Runtime.Serialization.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.Serialization.Json.dll'
------------------
Resolve: 'System.Runtime.Serialization.Formatters, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.Serialization.Formatters, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Runtime.Serialization.Formatters.dll'
------------------
Resolve: 'System.Security.Claims, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Claims, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Claims.dll'
------------------
Resolve: 'System.Security.Cryptography.Algorithms, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Algorithms, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Cryptography.Algorithms.dll'
------------------
Resolve: 'System.Security.Cryptography.Csp, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Csp, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Cryptography.Csp.dll'
------------------
Resolve: 'System.Security.Cryptography.Encoding, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Encoding, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Cryptography.Encoding.dll'
------------------
Resolve: 'System.Security.Cryptography.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.Cryptography.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Security.Cryptography.Primitives.dll'
------------------
Resolve: 'System.Text.Encoding.Extensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.Encoding.Extensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Text.Encoding.Extensions.dll'
------------------
Resolve: 'System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Text.RegularExpressions.dll'
------------------
Resolve: 'System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Threading.dll'
------------------
Resolve: 'System.Threading.Overlapped, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Overlapped, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Threading.Overlapped.dll'
------------------
Resolve: 'System.Threading.ThreadPool, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.ThreadPool, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Threading.ThreadPool.dll'
------------------
Resolve: 'System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Threading.Tasks.Parallel.dll'
------------------
Resolve: 'System.Transactions.Local, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Transactions.Local, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Transactions.Local.dll'
------------------
Resolve: 'System.Web.HttpUtility, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Web.HttpUtility, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Web.HttpUtility.dll'
------------------
Resolve: 'System.Xml.ReaderWriter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.ReaderWriter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Xml.ReaderWriter.dll'
------------------
Resolve: 'System.Xml.XDocument, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XDocument, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Xml.XDocument.dll'
------------------
Resolve: 'System.Xml.XmlSerializer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XmlSerializer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Xml.XmlSerializer.dll'
------------------
Resolve: 'System.Xml.XPath.XDocument, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XPath.XDocument, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Xml.XPath.XDocument.dll'
------------------
Resolve: 'System.Xml.XPath, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XPath, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.16\ref\net6.0\System.Xml.XPath.dll'
#endif
