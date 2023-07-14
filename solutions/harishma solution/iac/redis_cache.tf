
data "azurerm_redis_cache""sfmc_cache"{
   name                 = "Redis-SFMC-Cache-${local.environment_sanitized}"
   resource_group_name  ="rg-loyaltyapi-${local.environment_sanitized}"
}


resource "azurerm_key_vault_secret" "redis_connection_string" {
  name         = "redisConnectionString"
  value        = data.azurerm_redis_cache.sfmc_cache.primary_connection_string
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
