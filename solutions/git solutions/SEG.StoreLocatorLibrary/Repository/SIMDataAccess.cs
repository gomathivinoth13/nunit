using System;
using System.Collections.Generic;
using System.Collections;
using SEG.StoreLocatorLibrary.Shared;
using SEG.StoreLocatorLibrary.Shared.Interfaces;
using SEG.StoreLocatorLibrary.Shared.Types;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEG.StoreLocatorLibrary.Repository
{
    public class SIMDataAccess : ISimDataAccess
    {
        private string _simStores;

        //*****************************************************
        //* simStores is a JSON string with SIM store
        //* use if getting data from HTTP Post call
        //*****************************************************
        public SIMDataAccess(string simStores)
        {
            _simStores = simStores;
        }

        //*****************************************************
        //* Make HTTP call to SIM URL to get stores
        //*****************************************************
        public SIMDataAccess(IDictionary config)
        {
            var simUrl = config["SIM_Url"].ToString();
            var apimKey = config["Ocp-Apim-Subscription-Key"].ToString();
            var restClient = new RestClient();
            var request = new RestRequest(simUrl, Method.GET);
            request.AddHeader("Ocp-Apim-Subscription-Key", apimKey);
            _simStores = restClient.Execute(request).Content;
        }

        public Option<IList<SimStore>> SimStores()
        {
            if(string.IsNullOrEmpty(_simStores))
                return Option<IList<SimStore>>.CreateEmpty(500, $"SIM file is Empty");

            try
            {
                var payload = JObject.Parse(_simStores)["Store"].ToString();
                var stores = JsonConvert.DeserializeObject<List<SimStore>>(payload);
                return Option<IList<SimStore>>.Create(stores);
            }
            catch (NullReferenceException nr)
            {
                return Option<IList<SimStore>>
                    .CreateEmpty(400, $"Element 'Store' was not found in SIM stores result: {nr.Message}");
            }
            catch (Exception ex)
            {
                return Option<IList<SimStore>>.CreateEmpty(500, $"Error message: {ex.Message}");
            }
        }
    }
}
