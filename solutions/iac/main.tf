terraform {
  backend "azurerm" {
    resource_group_name = "rg-terraform-${local.environment_sanitized}"
  }
}

locals {
  environment_sanitized    = lower(var.environment)
  region_sanitized         = lower(var.location)
  org_suffix_sanitized     = lower(var.organization_suffix)
  app_name_sanitized       = lower(var.app_name)
  app_abbr_sanitized       = lower(var.app_abbr)
  plan_name                = "${var.plan_name}-${local.environment_sanitized}"
  plan_resource_group_name = "${var.plan_resource_group_name}-${local.environment_sanitized}"
  use_local_plan           = local.environment_sanitized == "dev" ? true : false

  digitalmktg_secrets = [
    "DynatraceAgentToken",
    "SalesForce-SEG-ClientID",
    "SalesForce-SEG-ClientSecret"
  ]
}

data "azurerm_client_config" "current" {
}

data "azurerm_redis_cache""sfmc_cache"{
   name                 = "Redis-SFMC-Cache-${local.environment_sanitized}"
   resource_group_name ="rg-loyaltyapi-${local.environment_sanitized}"
}
