terraform {
  backend "azurerm" {
    resource_group_name = "rg-terraform-dev"
  }
}

locals {
  environment_sanitized            = lower(var.environment)
  region_sanitized                 = lower(var.location)
  org_suffix_sanitized             = lower(var.organization_suffix)
  app_insights_name                = "Func-EcreboProcessor-${local.environment_sanitized}"
  app_insights_resource_group_name = "rg-ereceipts-${local.environment_sanitized}"
  key_vault_name                   = "kv-ecproapi-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  vaultName                        = "kv-omnichannel-seg-${local.environment_sanitized}"
  use_local_plan                   = local.environment_sanitized == "dev" ? true : false
  plan_name                        = "${var.plan_name}-${local.environment_sanitized}"
  plan_resource_group_name         = "${var.plan_resource_group_name}-${local.environment_sanitized}"
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


data "azurerm_cosmosdb_account" "ereceipts_database" {
  name                = "e-receipts${local.environment_sanitized}"
  resource_group_name = "rg-ereceipts-${local.environment_sanitized}"
}

data "azurerm_storage_account" "ereceipts_blob" {
  name                = "str${local.org_suffix_sanitized}ereceipts${local.environment_sanitized}"
  resource_group_name = "rg-ereceipts-${local.environment_sanitized}"
}


data "azurerm_storage_account" "ingester_blob" {
  name                = "stereceipts${local.org_suffix_sanitized}${local.environment_sanitized}"
  resource_group_name = "rg-ereceipts-ingester-apps-${local.environment_sanitized}"
}


data "azurerm_redis_cache" "ereceipts_cache" {
  name                = "Redis-EReceipts-Cache-${local.environment_sanitized}"
  resource_group_name = "rg-ereceipts-${local.environment_sanitized}"
  }

