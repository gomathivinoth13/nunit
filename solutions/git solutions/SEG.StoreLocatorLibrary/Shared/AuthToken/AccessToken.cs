using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace SEG.StoreLocatorLibrary.Shared.AuthToken
{
    public class AccessToken
    {


        public static async Task<string> getToken(IDictionary config)
        {
            //a daemon application is a confidential client application
            IConfidentialClientApplication app;


            // Even if this is a console application here, a daemon application is a confidential client application
            app = ConfidentialClientApplicationBuilder.Create(config["Demon_ClientId"].ToString())
                .WithClientSecret(config["Demon_ClientSecret"].ToString())
                .WithB2CAuthority(config["Demon_Instance"].ToString())
                .Build();


            app.AddInMemoryTokenCache();

            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator
            string[] scopes = new string[] { config["Demon_ApiUrl"].ToString() };
          
            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
                throw;
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
                throw;
            }


            return result.AccessToken;

        }
    }
}
