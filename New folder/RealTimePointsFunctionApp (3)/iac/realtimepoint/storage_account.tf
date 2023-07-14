resource "azurerm_storage_account" "shared_sa" {
  name = "sarealtimeapis${local.environment_sanitized}"
  resource_group_name = azurerm_resource_group.realtimepoints_apis.name
  location =  azurerm_resource_group.realtimepoints_apis.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}