terraform {
  backend "azurerm" {
    resource_group_name = "rg-terraform-${local.environment_sanitized}"
  }
}

locals {
  environment_sanitized            = lower(var.environment)
  region_sanitized                 = lower(var.location)
  org_suffix_sanitized             = lower(var.organization_suffix)
  app_insights_name                = "Func-realtimepoints-${local.environment_sanitized}"
  app_insights_resource_group_name = "rg-realtimepoint-${local.environment_sanitized}"
  key_vault_name                   = "kv-realtimepointapi-${local.environment_sanitized}"
  vaultName                        = "kv-omnichannel-seg-${local.environment_sanitized}"
  mktg_vault_name                  = "DigitalMktg-Vault-${local.environment_sanitized}"
  use_local_plan                   = local.environment_sanitized == "dev" ? true : false
  plan_name                        = "${var.plan_name}-${local.environment_sanitized}"
  plan_resource_group_name         = "${var.plan_resource_group_name}-${local.environment_sanitized}"

   digitalmktg_secrets = [
    "DynatraceAgentToken",
    "SalesForce-SEG-ClientID",
    "SalesForce-SEG-ClientSecret",
    "RealTimePointSegKey",
    "SecretEagleEye",
    "ClientIdEagleEye",
    "ocp-apim-subscription-key"
  ]

   }
data "azurerm_client_config" "current" {}

data "azurerm_key_vault" "omnichannel_kv" {
  name                = "kv-omnichannel-seg-${local.environment_sanitized}"
  resource_group_name = "rg-omnichannel-security-${local.environment_sanitized}"
}

data "azurerm_key_vault" "digitalmktg_vault" {
  name                = var.digitalmktg_vault_name
  resource_group_name = var.digitalmktg_vault_resource_group_name
}

data "azurerm_key_vault_secret" "dt_api_token_secret" {
  name         = "DynatraceAgentToken"
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id
}

data "azurerm_redis_cache""sfmc_cache"{
   name                 = "Redis-SFMC-Cache-${local.environment_sanitized}"
   resource_group_name ="rg-loyaltyapi-${local.environment_sanitized}"
}
data "azurerm_cosmosdb_account" "Eagleeye-database" {
    name                = "cosmos-omnichannel-${local.environment_sanitized}"
    resource_group_name = "rg-omnichannel-core-${local.environment_sanitized}"
}
data "azurerm_redis_cache""eagleeye_cache"{
    name                 = "redis-omnichannel-seg-${local.environment_sanitized}"
    resource_group_name ="rg-omnichannel-cache-${local.environment_sanitized}"
}
data "azurerm_key_vault_secret" "digitalmktg_secrets" {
  for_each     = toset(local.digitalmktg_secrets)
  name         = each.value
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id
}