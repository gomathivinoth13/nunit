resource "azurerm_storage_account" "storage_account" {
  name = "saeagleeyeapis${local.environment_sanitized}"
  resource_group_name = azurerm_resource_group.eagleeye_apis.name
  location =  azurerm_resource_group.eagleeye_apis.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}