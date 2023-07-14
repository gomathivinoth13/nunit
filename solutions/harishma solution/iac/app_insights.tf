resource "azurerm_application_insights" "main" {
  name                = "appi-${local.app_name_sanitized}-apis-${local.environment_sanitized}"
  location            = azurerm_resource_group.apis.location
  resource_group_name = azurerm_resource_group.apis.name
  application_type    = "web"
  retention_in_days   = 90
}
