resource "azurerm_key_vault" "main" {
  name                        = local.key_vault_name
  location                    = data.azurerm_resource_group.ereceipts_security.location
  resource_group_name         = data.azurerm_resource_group.ereceipts_security.name #Separates security concerns for key vault security from teams development of functionality
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
    "backup",
    "create",
    "decrypt",
    "delete",
    "encrypt",
    "get",
    "import",
    "list",
    "purge",
    "recover",
    "restore",
    "sign",
    "unwrapKey",
    "update",
    "verify",
    "wrapKey"
  ]
  secret_permissions = [
    "backup",
    "delete",
    "get",
    "list",
    "purge",
    "recover",
    "restore",
    "set"
  ]
  certificate_permissions = [
    "get",
    "getissuers",
    "import",
    "list",
    "listissuers",
    "managecontacts",
    "manageissuers",
    "purge",
    "recover",
    "restore",
    "setissuers",
    "update"
  ]
  storage_permissions = [
    "backup",
    "delete",
    "deletesas",
    "get",
    "getsas",
    "list",
    "listsas",
    "purge",
    "recover",
    "regeneratekey",
    "restore",
    "set",
    "setsas",
    "update"
  ]
}

resource "azurerm_key_vault_secret" "cosmos_connection_string" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "CosmosEndpoint"
  value        = "${data.azurerm_cosmosdb_account.ereceipts_database.endpoint}"

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}

resource "azurerm_key_vault_secret" "cosmos_primary_key" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "CosmosPrimary"
  value        = "${data.azurerm_cosmosdb_account.ereceipts_database.primary_key}"

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}

resource "azurerm_key_vault_secret" "blobstorage_ingester_connection_string" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "BlobstorageIngesterConnection"
  value        = data.azurerm_storage_account.ingester_blob.primary_blob_connection_string

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}

resource "azurerm_key_vault_secret" "blobstorage_ereceipts_connection_string" {
  key_vault_id = azurerm_key_vault.main.id
  name         = "BlobstorageEreceiptsConnection"
  value        = data.azurerm_storage_account.ereceipts_blob.primary_blob_connection_string

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}

  
  resource "azurerm_key_vault_secret" "redis_connection_string" {
  name         = "RedisConnectionString"
  value        = data.azurerm_redis_cache.ereceipts_cache.primary_connection_string
  key_vault_id = azurerm_key_vault.main.id

   depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}