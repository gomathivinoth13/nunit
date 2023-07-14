using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEG.AzureLoyaltyDatabase.DataAccess;
using SalesForceLibrary.SalesForceAPIM;
using Microsoft.Extensions.Configuration;
using SEG.SalesForce.Models;
using SEG.ApiService.Models.Surveys;
using SEG.AzureLoyaltyDatabase;

namespace SEGLoyaltyServiceWeb.Controllers
{
    /// <summary>
    /// Product Survey Controller
    /// </summary>
    public class ProductSurveyController : Controller
    {
        private readonly SalesForceAPIMService _salesForceService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Product Survey Constructor
        /// </summary>
        public ProductSurveyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _salesForceService = new SalesForceAPIMService(configuration["Settings:SalesForce:SalesForceAPIMAuthEndPoint"], 
                configuration["Settings:SalesForce:SalesForceAPIMBaseEndPoint"], 
                configuration["Settings:SalesForce:SEG_ClientID"], 
                configuration["Settings:SalesForce:SEG_ClientSecret"], 
                configuration["Settings:SalesForce:redisConnectionString"], 
                configuration["Settings:SalesForce:ocpApimSubscriptionKey"]);
        }
        /// <summary>
        /// Bet Product Survey by Member Id
        /// </summary>
        [HttpGet("api/Survey/GetProductSurvey")]
        public async Task<IActionResult> GetProductSurveyAsync(string memberId)
        {
            List<ProductSurvey> surveys = null;
            try
            {
                surveys = await AzureLoyaltyDatabaseManager.GetProductSurveyByMemberIdAsync(memberId);
            }
            catch(Exception e)
            {
                throw new Exception("Exception in GetProductSurveyAsync :", e);
            }

            return Ok(surveys);
        }

        /// <summary>
        /// Save Product Survey
        /// </summary>
        [HttpPost("api/Survey/SaveProductSurvey")]
        public async Task<IActionResult> SaveProductSurveyAsync([FromBody] ProductSurvey survey)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isSaved = await AzureLoyaltyDatabaseManager.SaveProductSurveyAsync(survey);
                    if (!isSaved) return new BadRequestResult();

                    var response = await _salesForceService.UpsertProductSurveyAsync(survey, _configuration["Settings:SalesForce:SEG_Key_Product_Survey"]);
                    if (string.IsNullOrEmpty(response.errorcode)) return Ok();
                }
                catch (Exception e)
                {
                    throw new Exception("Exception in SaveProductSurveyAsync :", e);
                }
            }

            return new BadRequestResult();
        }
    }
}
