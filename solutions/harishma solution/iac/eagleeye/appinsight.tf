resource "azurerm_application_insights" "aieagleeye" {
  name                = "appi-eagleeye-apis-${local.environment_sanitized}"
  location            = azurerm_resource_group.eagleeye_apis.location
  resource_group_name = azurerm_resource_group.eagleeye_apis.name
  application_type    = "web"
  retention_in_days   = 90
}
