module "realtimepoints_api_app" {

  source                           = "git::https://dev.azure.com/SEGDEVOPS/SE%20Grocers/_git/Common.Terraform//modules/shared-plan-function-app-v4-windows-v1"
  plan_name                        = local.use_local_plan ? azurerm_app_service_plan.main[0].name : local.plan_name
  plan_resource_group_name         = local.use_local_plan ? azurerm_app_service_plan.main[0].resource_group_name : local.plan_resource_group_name
  environment                      = local.environment_sanitized
  organization_suffix              = local.org_suffix_sanitized
  app_name                         = "realtimepoints-api"
  app_resource_group_name          = azurerm_resource_group.realtimepoints_apis.name
  deployment_number                = var.realtime_api_deployment_number
  use_shared_storage_account       = false
  pre_warmed_instance_count        = var.function_apps_warm_instance_count
  runtime_scale_monitoring_enabled = false

  application_stack = {
    dotnet_version              = "6"
    use_dotnet_isolated_runtime = true
    java_version                = null
    node_version                = null
    powershell_core_version     = null
    use_custom_runtime          = null
  }

  function_app_storage_account_name                = azurerm_storage_account.shared_sa.name
  function_app_storage_account_resource_group_name = azurerm_storage_account.shared_sa.resource_group_name

 app_insights_name                = azurerm_application_insights.airealtimepoints.name
 app_insights_resource_group_name = azurerm_application_insights.airealtimepoints.resource_group_name


  app_settings = {
    FUNCTIONS_WORKER_RUNTIME                            = "dotnet-isolated"
    FUNCTIONS_EXTENSION_VERSION                         = "~4"
    OpenApi__Version                                    = "V3"
    OpenApi__DocVersion                                 = "1.0.0"
    SalesForceAPIMAuthEndPoint                          = var.salesforceapimauthendpoint
    SalesForceAPIMBaseEndPoint                          = var.salesforceapimbaseendpoint 
    SEG_ClientID                                        = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["SalesForce-SEG-ClientID"].id})"
    SEG_ClientSecret                                    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["SalesForce-SEG-ClientSecret"].id})"
    redisConnectionString                               = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_connection_string.id})"
    SEG_Key                                             = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["RealTimePointSegKey"].id})" 
    ClientIDEE                                          = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["ClientIdEagleEye"].id})"
    SecretEE                                            = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["SecretEagleEye"].id})"
    BaseUrlEE                                           = var.base_url_ee
    CosmosPrimaryKey                                    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_primary_key.id})"
    CosmosDataBaseId                                    = "seg-stream"
    BaseUrlCampaignsEE                                  = var.base_url_campaigns_ee
    CosmosEndpointUri                                   = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_connection_string.id})"
    CacheConnectionString                               = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_connection.id})"
    OcpApimSubscriptionKey                              = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["ocp-apim-subscription-key"].id})"
    loyaltyAzureConnection                              = "@Microsoft.KeyVault(VaultName=${local.vaultName};SecretName=ConnectionStrings--AzureSqlDB)"
    CacheServer                                         = var.cache_server
    CosmosContainerId                                   = "coupon"  
  
    DT_TENANT                = "qds48093"
    DT_SSL_MODE              = "default"
    DT_API_TOKEN             = local.environment_sanitized != "dev" ? "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault_secret.dt_api_token_secret.id})" : ""
  }

  depends_on = [
   azurerm_resource_group.realtimepoints_apis,
   azurerm_app_service_plan.main,
   azurerm_storage_account.shared_sa,
   azurerm_application_insights.airealtimepoints
  ]
}

resource "azapi_resource" "dynatrace_agent" {
  count = local.environment_sanitized != "dev" ? 1 : 0
  type = "Microsoft.Web/sites/siteextensions@2021-01-15"
  name = "Dynatrace"
  parent_id = module.realtimepoints_api_app.function_app_id  #Comes from outputs.tf in shared-plan-function-app-v3 in Common.Terraform in git source above

  depends_on = [
    module.realtimepoints_api_app
  ]
}

# Azure Keyvault access policy
resource "azurerm_key_vault_access_policy" "realtimepoints_api_app_kv_access_policy" {
  key_vault_id = azurerm_key_vault.main.id

  tenant_id = module.realtimepoints_api_app.function_msi_tenant_id
  object_id = module.realtimepoints_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

  depends_on = [
    module.realtimepoints_api_app
  ]
}

resource "azurerm_key_vault_access_policy" "realtimepoints_omnichannel_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.omnichannel_kv.id
 
  tenant_id = module.realtimepoints_api_app.function_msi_tenant_id
  object_id = module.realtimepoints_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

    depends_on = [
    module.realtimepoints_api_app
  ]

}
resource "azurerm_key_vault_access_policy" "realtimepoints_digitalmktg_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id

  tenant_id = module.realtimepoints_api_app.function_msi_tenant_id
  object_id = module.realtimepoints_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

  depends_on = [
    module.realtimepoints_api_app
  ]
}
