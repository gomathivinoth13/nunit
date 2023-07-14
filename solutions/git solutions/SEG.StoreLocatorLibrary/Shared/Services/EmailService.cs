using RestSharp;
using System.Collections;
using System;
using static Microsoft.Graph.CoreConstants;

namespace SEG.StoreLocatorLibrary.Shared.Services
{
    public class EmailService
    {
        public bool SendEmail(EmailErrorLog message, IDictionary config)
        {
            var subject = $" StoreLocator - {DateTime.UtcNow}(UTC) - Processing Store Locator updates";
            var method = string.IsNullOrEmpty(message.Process)
                ? "ERROR in StoreLocator APP: "
                : message.Process;

            var error = string.IsNullOrEmpty(message.ErrorMessage)
                ? "ERROR in StoreLocator APP: "
                : message.ErrorMessage;

            var fromEmail = config["MailAddressFrom"].ToString();
            var toEmail = config["MailAddressTo"].ToString();
            var ccEmail = config["MailAddressCC"]?.ToString() ?? toEmail;

            if (string.IsNullOrEmpty(ccEmail)) ccEmail = toEmail;

            var email = new EmailModel 
            {
                subject = subject,
                toEmail = toEmail,
                fromEmail = fromEmail,
                ccEmail = ccEmail,
                errorResponse = error,
                methodRequest = method
            };

            var restClient = new RestClient();
            var request = new RestRequest(config["InternalMailUrl"].ToString(), Method.POST);
            request.AddHeader("Ocp-Apim-Subscription-Key", config["InternalMailApiKey"].ToString());
            request.AddJsonBody(email);
            var response = restClient.Execute(request);

            return true;
        }


        public bool SendEmail(EmailErrorLog message, IDictionary config, string accessToken)
        {
            var subject = $" StoreLocator - {DateTime.UtcNow}(UTC) - Processing Store Locator updates";
            var method = string.IsNullOrEmpty(message.Process)
                ? "ERROR in StoreLocator APP: "
                : message.Process;

            var error = string.IsNullOrEmpty(message.ErrorMessage)
                ? "ERROR in StoreLocator APP: "
                : message.ErrorMessage;

            var fromEmail = config["MailAddressFrom"].ToString();
            var toEmail = config["MailAddressTo"].ToString();
            var ccEmail = config["MailAddressCC"]?.ToString() ?? toEmail;

            if (string.IsNullOrEmpty(ccEmail)) ccEmail = toEmail;

            var email = new EmailModel
            {
                subject = subject,
                toEmail = toEmail,
                fromEmail = fromEmail,
                ccEmail = ccEmail,
                errorResponse = error,
                methodRequest = method
            };

            var restClient = new RestClient();
            var request = new RestRequest(config["InternalMailUrl"].ToString(), Method.POST);
            request.AddHeader("Ocp-Apim-Subscription-Key", config["InternalMailApiKey"].ToString());
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(email);
            var response = restClient.Execute(request);

            return true;
        }
    }
}
