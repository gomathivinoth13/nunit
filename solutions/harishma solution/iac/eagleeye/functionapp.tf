module "eagleeye_api_app" {
 source = "git::https://dev.azure.com/SEGDEVOPS/SE%20Grocers/_git/Common.Terraform//modules/shared-plan-function-app-v2-function-v4"
  plan_name                        = azurerm_app_service_plan.main.name
  plan_resource_group_name         = azurerm_app_service_plan.main.resource_group_name
  environment                      = local.environment_sanitized
  organization_suffix              = local.org_suffix_sanitized
  app_name                         = "eagleeye-api"
  deployment_number                = var.eagleeye_api_deployment_number
  use_shared_storage_account       = true
  pre_warmed_instance_count        = var.function_apps_warm_instance_count
  runtime_scale_monitoring_enabled = false


  function_app_storage_account_name                =  azurerm_storage_account.storage_account.name
  function_app_storage_account_resource_group_name = azurerm_storage_account.storage_account.resource_group_name

  app_insights_name                = azurerm_application_insights.aieagleeye.name
  app_insights_resource_group_name = azurerm_application_insights.aieagleeye.resource_group_name

   app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet-isolated"
    OpenApi__Version         = "V3"
    OpenApi__DocVersion      = "1.0.0"
    OpenApi__DocTitle        = "Eagle Eye API"
    CosmosEndpointUri        = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_connection_string.id})"
    CacheConnectionString    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_connection_string.id})"
    ClientIDEE               = var.client_id_ee
    SecretEE                 = var.secret_ee
    BaseUrlEE                = var.base_url_ee
    CosmosPrimaryKey         = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_primary_key.id})"
    CosmosDataBaseId         = "seg-stream"
    BaseUrlCampaignsEE       = var.base_url_campaigns_ee
    BiloPartnerCode          = var.bilo_partner_code
    CacheServer              = var.cache_server
    CosmosContainerId        = "coupon"
    FrescoPartnerCode        = var.fresco_partner_code
    HarveysPartnerCode       = var.harveys_partner_code
    OcpApimSubscriptionKey   = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.ocp_apim_subscription_key.id})"
    WDPartnerCode            = var.wd_partner_code
    loyaltyAzureConnection   = "@Microsoft.KeyVault(VaultName=${local.vaultName};SecretName=ConnectionStrings--AzureSqlDB)"
    EEHealthCheckMemberId    = var.ee_healthcheck_memberid
	  EEHealthCheckApis        = "https://portal.us2.eagleeye.com/health/status;https://pos.us2.eagleeye.com/health/status;https://wallet.us2.eagleeye.com/health/status"
    CouponLimitDays          = "7"
    
    DT_TENANT                = "qds48093"
    DT_SSL_MODE              = "default"
    DT_API_TOKEN             = local.environment_sanitized != "dev" ? "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault_secret.dt_api_token_secret.id})" : ""

    # Used by StoreLocator.Library Nuget package
    SLLMongoDbConnection     = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.sll_cosmos_connection_string.id})"
    SLLRedisCacheConnection  = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.sll_redis_connection_string.id})"

  }

   depends_on = [
    azurerm_resource_group.eagleeye_apis,
    azurerm_app_service_plan.main,
    azurerm_storage_account.storage_account,
    azurerm_application_insights.aieagleeye,
    azurerm_key_vault_secret.ocp_apim_subscription_key
  ]
}

#resource "azapi_resource" "dynatrace_agent" {
#  count = local.environment_sanitized != "dev" ? 1 : 0
#  type = "Microsoft.Web/sites/siteextensions@2021-01-15"
#  name = "Dynatrace"
#  parent_id = module.eagleeye_api_app.function_app_id  #Comes from outputs.tf in shared-plan-function-app-v3 in Common.Terraform in git source above

# depends_on = [
#   module.eagleeye_api_app
#  ]
#}

# Azure Keyvault access policy

resource "azurerm_key_vault_access_policy" "eagleeye_apis_app_kv_access_policy" {
  key_vault_id = azurerm_key_vault.main.id

  tenant_id = module.eagleeye_api_app.function_msi_tenant_id
  object_id = module.eagleeye_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

  depends_on = [
    module.eagleeye_api_app
  ]
}

resource "azurerm_key_vault_access_policy" "eagleeye_digitalmktg_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id

  tenant_id = module.eagleeye_api_app.function_msi_tenant_id
  object_id = module.eagleeye_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

  depends_on = [
    module.eagleeye_api_app
  ]
}

resource "azurerm_key_vault_access_policy" "eagleeye_omnichannel_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.omnichannel_kv.id
 
  tenant_id = module.eagleeye_api_app.function_msi_tenant_id
  object_id = module.eagleeye_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

   depends_on = [
    module.eagleeye_api_app
  ]
}
module "eagleeye_api_app_apim" {
  source = "git::https://dev.azure.com/SEGDEVOPS/SE%20Grocers/_git/Common.Terraform//modules/apim-api-function-app"
  apim_name                        = data.azurerm_api_management.apim_instance.name
  apim_resource_group_name         = data.azurerm_api_management.apim_instance.resource_group_name
  function_app_name                = module.eagleeye_api_app.function_app_name
  function_app_resource_group_name = azurerm_app_service_plan.main.resource_group_name
  create_kv_access_policy          = true
  key_vault_id                     = azurerm_key_vault.main.id

  providers = {
    azurerm.function = azurerm
    azurerm.apim     = azurerm
  }
    depends_on = [
    module.eagleeye_api_app,
    azurerm_key_vault.main,
    azurerm_app_service_plan.main,
    data.azurerm_api_management.apim_instance,
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}