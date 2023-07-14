resource "azurerm_resource_group" "apis" {
  name     = "rg-${local.app_name_sanitized}-apis-${local.environment_sanitized}"
  location = local.region_sanitized
}

resource "azurerm_resource_group" "security" {
  name     = "rg-${local.app_name_sanitized}-security-${local.environment_sanitized}"
  location = local.region_sanitized
}
