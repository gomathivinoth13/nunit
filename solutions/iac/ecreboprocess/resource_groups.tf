data "azurerm_resource_group" "example" {
  name = "rg-ereceipts-${local.environment_sanitized}"
  # location = local.region_sanitized
}

data "azurerm_resource_group" "ereceipts_apis" {
  name     = "rg-ereceipts-apis-${local.environment_sanitized}"
  # location = local.region_sanitized
}

data "azurerm_resource_group" "ereceipts_security" {
  name     = "rg-ereceipts-security-${local.environment_sanitized}"
  # location = local.region_sanitized
}