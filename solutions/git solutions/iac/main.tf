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
    "azurembonotificationsfmc-eagleeye-clientid",
    "azurembonotificationsfmc-eagleeye-secret",
    "azurembonotificationsfmc-fresco-clientid",
    "azurembonotificationsfmc-fresco-secret",
    "azurembonotificationsfmc-fresco-eventdefinitionkey",
    "azurembonotificationsfmc-harveys-clientid",
    "azurembonotificationsfmc-harveys-secret",
    "azurembonotificationsfmc-harveys-eventdefinitionkey",
    "azurembonotificationsfmc-winndixie-clientid",
    "azurembonotificationsfmc-winndixie-secret",
    "azurembonotificationsfmc-winndixie-eventdefinitionkey",
    "Loyalty-Azure-DB-Conn-String",
    "ocp-apim-subscription-key"
  ]
}

data "azurerm_client_config" "current" {
}

data "azurerm_key_vault_secret" "dt_api_token_secret" {
  name         = "DynatraceAgentToken"
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id
}
