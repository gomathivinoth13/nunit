module "ecreboprocessor_api_app" {
  source = "git::https://dev.azure.com/SEGDEVOPS/SE%20Grocers/_git/Common.Terraform//modules/shared-plan-function-app-v3-function-v4"

  plan_name                        = local.use_local_plan ? azurerm_app_service_plan.main[0].name : local.plan_name
  plan_resource_group_name         = local.use_local_plan ? azurerm_app_service_plan.main[0].resource_group_name : local.plan_resource_group_name
  environment                      = local.environment_sanitized
  organization_suffix              = local.org_suffix_sanitized
  app_name                         = "ecreboprocessor-api"
  app_resource_group_name          = data.azurerm_resource_group.ereceipts_apis.name
  deployment_number                = var.ecreboprocessor_api_deployment_number
  use_shared_storage_account       = false
  pre_warmed_instance_count        = var.function_apps_warm_instance_count
  runtime_scale_monitoring_enabled = false

  function_app_storage_account_name                = azurerm_storage_account.shared_sa.name
  function_app_storage_account_resource_group_name = azurerm_storage_account.shared_sa.resource_group_name

 app_insights_name                = azurerm_application_insights.aiereceipts.name
 app_insights_resource_group_name = azurerm_application_insights.aiereceipts.resource_group_name


  app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet-isolated"
    CosmosEndpoint           = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_connection_string.id})"
    CosmosPrimary            = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.cosmos_primary_key.id})"
    BlobContainerName        = "ereceipts-html"
    CosmosDataBase           = "E-Receipts"
    CosmosContainer          = "Receipts"
    RedisConnectionString    = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.redis_connection_string.id})"
    AzureDataBaseConnectionString = "@Microsoft.KeyVault(VaultName=${local.vaultName};SecretName=ConnectionStrings--AzureSqlDB)"
    EReceiptsHtmlConfiguration = "EReceiptsHtmlConfiguration"
    IngesterContainerNameHtml  ="eventshtml"
    IngesterContainerNameJson  ="eventsjson"
    BlobStorageEreceiptsConnection = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.blobstorage_ereceipts_connection_string.id})"
    BlobIngesterStorageConnection = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.blobstorage_ingester_connection_string.id})"
    
    DT_TENANT                = "qds48093"
    DT_SSL_MODE              = "default"
    DT_API_TOKEN             = local.environment_sanitized != "dev" ? "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault_secret.dt_api_token_secret.id})" : ""
  }

  depends_on = [
   data.azurerm_resource_group.ereceipts_apis,
   azurerm_app_service_plan.main,
   azurerm_storage_account.shared_sa,
   azurerm_application_insights.aiereceipts
  ]
}

resource "azapi_resource" "dynatrace_agent" {
  count = local.environment_sanitized != "dev" ? 1 : 0
  type = "Microsoft.Web/sites/siteextensions@2021-01-15"
  name = "Dynatrace"
  parent_id = module.ecreboprocessor_api_app.function_app_id  #Comes from outputs.tf in shared-plan-function-app-v3 in Common.Terraform in git source above

  depends_on = [
    module.ecreboprocessor_api_app
  ]
}

# Azure Keyvault access policy
resource "azurerm_key_vault_access_policy" "ecreboprocessor_api_app_kv_access_policy" {
  key_vault_id = azurerm_key_vault.main.id

  tenant_id = module.ecreboprocessor_api_app.function_msi_tenant_id
  object_id = module.ecreboprocessor_api_app.function_msi_principal_id

  key_permissions = [
    "get",
  ]

  secret_permissions = [
    "get",
  ]

  depends_on = [
    module.ecreboprocessor_api_app
  ]
}

resource "azurerm_key_vault_access_policy" "ecreboprocessor_digitalmktg_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id

  tenant_id = module.ecreboprocessor_api_app.function_msi_tenant_id
  object_id = module.ecreboprocessor_api_app.function_msi_principal_id

  key_permissions = [
    "get",
  ]

  secret_permissions = [
    "get",
  ]

  depends_on = [
    module.ecreboprocessor_api_app
  ]
}

resource "azurerm_key_vault_access_policy" "ecreboprocessor_omnichannel_kv_access_policy" {
  key_vault_id = data.azurerm_key_vault.omnichannel_kv.id
 
  tenant_id = module.ecreboprocessor_api_app.function_msi_tenant_id
  object_id = module.ecreboprocessor_api_app.function_msi_principal_id

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]

    depends_on = [
    module.ecreboprocessor_api_app
  ]

}
