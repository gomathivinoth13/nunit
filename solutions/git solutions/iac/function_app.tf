module "api_app" {
  source = "git::https://dev.azure.com/SEGDEVOPS/SE%20Grocers/_git/Common.Terraform//modules/shared-plan-function-app-v4-windows-v1"

  plan_name                        = local.use_local_plan ? azurerm_app_service_plan.main[0].name : local.plan_name
  plan_resource_group_name         = local.use_local_plan ? azurerm_app_service_plan.main[0].resource_group_name : local.plan_resource_group_name
  environment                      = local.environment_sanitized
  organization_suffix              = local.org_suffix_sanitized
  app_name                         = "${local.app_name_sanitized}-api"
  app_resource_group_name          = azurerm_resource_group.apis.name
  deployment_number                = var.api_deployment_number
  use_shared_storage_account       = true
  pre_warmed_instance_count        = var.function_apps_warm_instance_count
  runtime_scale_monitoring_enabled = var.runtime_scale_monitoring_enabled

  function_app_storage_account_name                = azurerm_storage_account.main.name
  function_app_storage_account_resource_group_name = azurerm_storage_account.main.resource_group_name

  app_insights_name                = azurerm_application_insights.main.name
  app_insights_resource_group_name = azurerm_application_insights.main.resource_group_name

  application_stack = {
    dotnet_version              = "6"
    use_dotnet_isolated_runtime = true
    java_version                = null
    node_version                = null
    powershell_core_version     = null
    use_custom_runtime          = null
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME      = "dotnet-isolated"
    baseUrlEE                     = var.eagleeye_base_url
    baseUrlCampaignsEE            = var.eagleeye_campaigns_base_url
    SalesForceAuthEndPoint        = var.salesforce_auth_base_url
    SalesForceRestEndPoint        = var.salesforce_rest_base_url
    clientIDEE                    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-eagleeye-clientid"].id})"
    secretEE                      = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-eagleeye-secret"].id})"
    ocpApimSubscriptionKey        = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["ocp-apim-subscription-key"].id})"
    DataBaseConnectionString      = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["Loyalty-Azure-DB-Conn-String"].id})"
    redisCampaignConnectionString = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_omnichannel_connection_string.id})"
    RedisConnectionString         = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_sfmc_connection_string.id})"
    Fresco_ClientID               = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-fresco-clientid"].id})"
    Fresco_ClientSecret           = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-fresco-secret"].id})"
    FrescoEventDefinitionKey      = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-fresco-eventdefinitionkey"].id})"
    Harveys_ClientID              = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-harveys-clientid"].id})"
    Harveys_ClientSecret          = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-harveys-secret"].id})"
    HarveysEventDefinitionKey     = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-harveys-eventdefinitionkey"].id})"
    WinnDixie_ClientID            = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-winndixie-clientid"].id})"
    WinnDixie_ClientSecret        = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-winndixie-secret"].id})"
    WDEventDefinitionKey          = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["azurembonotificationsfmc-winndixie-eventdefinitionkey"].id})"
    
    DT_TENANT                = "qds48093"
    DT_SSL_MODE              = "default"
    DT_API_TOKEN             = local.environment_sanitized != "dev" ? "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault_secret.dt_api_token_secret.id})" : ""
  }

  depends_on = [
    azurerm_resource_group.apis,
    azurerm_storage_account.main,
    azurerm_application_insights.main,
    azurerm_key_vault_secret.redis_omnichannel_connection_string,
    azurerm_key_vault_secret.redis_sfmc_connection_string,
    azurerm_key_vault_secret.digitalmktg_secrets
  ]
}

resource "azapi_resource" "dynatrace_agent" {
  count = local.environment_sanitized != "dev" ? 1 : 0
  type = "Microsoft.Web/sites/siteextensions@2021-01-15"
  name = "Dynatrace"
  parent_id = module.api_app.function_app_id  #Comes from outputs.tf in shared-plan-function-app-v3 in Common.Terraform in git source above

  depends_on = [
    module.api_app
  ]
}

resource "azurerm_key_vault_access_policy" "api_app_kv_access_policy" {
  key_vault_id = azurerm_key_vault.main.id
  tenant_id    = module.api_app.function_msi_tenant_id
  object_id    = module.api_app.function_msi_principal_id

  key_permissions = [
    "Get",
    "List"
  ]

  secret_permissions = [
    "Get",
    "List"
  ]

  depends_on = [
    module.api_app
  ]
}


resource "azurerm_key_vault_access_policy" "api_app_digitalmktg_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id

  tenant_id = module.api_app.function_msi_tenant_id
  object_id = module.api_app.function_msi_principal_id

   key_permissions = [
    "Get",
    "List"
  ]

  secret_permissions = [
    "Get",
    "List"
  ]

  depends_on = [
    module.api_app
  ]
}
