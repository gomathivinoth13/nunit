resource "azurerm_resource_group" "realtimepoints_apis" {
  name     = "rg-realtimepoints-apis-${local.environment_sanitized}"
  location = local.region_sanitized
}
resource "azurerm_resource_group" "realtimepoints_security" {
  name     = "rg-realtimepoints-security-${local.environment_sanitized}"
  location = local.region_sanitized
}


