resource "azurerm_key_vault" "main" {
  name                        = local.key_vault_name
  location                    = azurerm_resource_group.realtimepoints_security.location
  resource_group_name         = azurerm_resource_group.realtimepoints_security.name #Separates security concerns for key vault security from teams development of functionality
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  purge_protection_enabled    = false

  sku_name = "standard"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
# Access policy for Azure DevOps 
resource "azurerm_key_vault_access_policy" "azure_devops_kv_access_policy" {
 key_vault_id = azurerm_key_vault.main.id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = data.azurerm_client_config.current.object_id

  key_permissions = [
    "Backup",
    "Create",
    "Decrypt",
    "Delete",
    "Encrypt",
    "Get",
    "Import",
    "List",
    "Purge",
    "Recover",
    "Restore",
    "Sign",
    "UnwrapKey",
    "Update",
    "Verify",
    "WrapKey"
  ]
  secret_permissions = [
    "Backup",
    "Delete",
    "Get",
    "List",
    "Purge",
    "Recover",
    "Restore",
    "Set"
  ]
  certificate_permissions = [
    "Get",    
    "Import",
    "List",    
    "Purge",
    "Recover",
    "Restore",  
    "Update"
  ]
  storage_permissions = [
    "Backup",
    "Delete",  
    "Get",
    "List",   
    "Purge",
    "Recover",   
    "Restore",
    "Set",    
    "Update"
  ]
}
resource "azurerm_key_vault_secret" "redis_connection_string" {
  name         = "redisConnectionString"
  value        = data.azurerm_redis_cache.sfmc_cache.primary_connection_string
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
resource "azurerm_key_vault_secret" "cosmos_primary_key" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "CosmosPrimary"
  value        = data.azurerm_cosmosdb_account.Eagleeye-database.primary_key

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
resource "azurerm_key_vault_secret" "cosmos_connection_string" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "CosmosEndpointUri"
   value        = data.azurerm_cosmosdb_account.Eagleeye-database.endpoint

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
resource "azurerm_key_vault_secret" "redis_connection" {
  name         = "CacheConnectionString"
  value        = "${data.azurerm_redis_cache.eagleeye_cache.primary_connection_string}${var.redis_connection_timeout}"
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
