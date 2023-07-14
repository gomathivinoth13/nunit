terraform {
  backend "azurerm" {
    resource_group_name = "rg-terraform-${local.environment_sanitized}"
  }
}
locals {
  environment_sanitized = lower(var.environment)
  region_sanitized      = lower(var.location)
  org_suffix_sanitized  = lower(var.organization_suffix)
  key_vault_name        = "kv-egapis-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  vaultName             = "kv-omnichannel-seg-${local.environment_sanitized}"
  mktg_vault_name       = "DigitalMktg-Vault-${local.environment_sanitized}"
  app_name_sanitized    = "eagleeye"
}

data "azurerm_client_config" "current" {}

####################### DATA ########################

data "azurerm_key_vault" "digitalmktg_vault" {
  name                = var.digitalmktg_vault_name
  resource_group_name = var.digitalmktg_vault_resource_group_name
}

data "azurerm_key_vault_secret" "dt_api_token_secret" {
  name         = "DynatraceAgentToken"
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id
}

data "azurerm_cosmosdb_account" "storelocator_database" {
  name                = "cosmos-omnichannel-${local.environment_sanitized}"
  resource_group_name = var.cosmos_db_rg
}

data "azurerm_key_vault" "omnichannel_kv" {
  name                = local.vaultName 
  resource_group_name = var.omnichannel_kv_rg
}
data "azurerm_cosmosdb_account" "Eagleeye-database" {
  name                = "cosmos-omnichannel-${local.environment_sanitized}"
  resource_group_name = "rg-omnichannel-core-${local.environment_sanitized}"
}


# Used by StoreLocator.Library Nuget Package
data "azurerm_cosmosdb_account" "storelocator_address_database" {
  name                = "seg${local.environment_sanitized}"
  resource_group_name = var.store_locator_address_db_rg
}

data "azurerm_redis_cache" "storelocator_address_cache" {
  name                = "redis-storelocator-cache-${local.environment_sanitized}"
  resource_group_name = "rg-storelocator-apis-${local.environment_sanitized}"
}






