using Flurl;
using Flurl.Http;
using SEG.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SEG.EagleEyeLibrary.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clientID"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AddRequestHeaders(string data, string clientID, string secret, string ocpApimSubscriptionKeySecret)
        {
            //string encoded;

            //EncodeURi on special characters for front door APIM implemntation  
           // if (!string.IsNullOrEmpty(ocpApimSubscriptionKeySecret))
            //    encoded = data.Replace("[]", "%5B%5D");
            //string encoded1 = encoded.Replace("[", "%5B");
            //string encoded3 = encoded1.Replace("]", "%5D");

            string oauthsignature = ComputeSha256Hash(data);
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("X-EES-AUTH-CLIENT-ID", clientID);
            headers.Add("X-EES-AUTH-HASH", oauthsignature);
            headers.Add("X-EES-TRANSACTION-ID", Guid.NewGuid().ToString());
            headers.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKeySecret);


            return headers;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
