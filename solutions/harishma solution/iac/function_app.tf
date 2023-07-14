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
    FUNCTIONS_WORKER_RUNTIME                 = "dotnet-isolated"
    DT_TENANT                                = !local.use_local_plan ? var.dynatrace_tenant : null
    DT_SSL_MODE                              = !local.use_local_plan ? var.dynatrace_ssl_mode : null
    DT_API_TOKEN                             = !local.use_local_plan ? "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["DynatraceAgentToken"].id})" : null
    SalesForceAPIMAuthEndPoint               = var.salesforceapimauthendpoint
    SalesForceAPIMBaseEndPoint               = var.salesforceapimbaseendpoint 
    SEG_ClientID                             = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["SalesForce-SEG-ClientID"].id})"
    SEG_ClientSecret                         = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["SalesForce-SEG-ClientSecret"].id})"
    redisConnectionString                    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_connection_string.id})"
    SEG_Key                                  = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["WalletAccountIDSegKey"].id})"
    OcpApimSubscriptionKey                   = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["ocp-apim-subscription-key"].id})"
    DataBaseConnectionString                 = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.digitalmktg_secrets["Loyalty-Azure-DB-Conn-String"].id})"


  }

  depends_on = [
    azurerm_resource_group.apis,
    azurerm_storage_account.main,
    azurerm_application_insights.main,
    azurerm_key_vault_secret.digitalmktg_secrets,
    azurerm_key_vault_secret.redis_connection_string

  ]
}

resource "azapi_resource" "dynatrace_agent" {
  count = !local.use_local_plan ? 1 : 0

  type      = "Microsoft.Web/sites/siteextensions@2021-01-15"
  name      = "Dynatrace"
  parent_id = module.api_app.function_app_id

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
