resource "azurerm_application_insights" "airealtimepoints" {
  name                 =  "appi-realtimepoints-apis-${local.environment_sanitized}"
  location             =  azurerm_resource_group.realtimepoints_apis.location
  resource_group_name  =  azurerm_resource_group.realtimepoints_apis.name
  application_type     =  "web"
  retention_in_days    =  90
}
