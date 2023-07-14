resource "azurerm_application_insights" "aiereceipts" {
  name                = "appi-EcreboProcess-apis-${local.environment_sanitized}"
  location             = data.azurerm_resource_group.ereceipts_apis.location
  resource_group_name = data.azurerm_resource_group.ereceipts_apis.name
  application_type    = "web"
   retention_in_days   = 90
}
