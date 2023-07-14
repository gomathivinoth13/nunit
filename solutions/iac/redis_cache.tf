data "azurerm_redis_cache" "omnichannel" {
  name                = "redis-omnichannel-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  resource_group_name = "rg-omnichannel-cache-${local.environment_sanitized}"
}

data "azurerm_redis_cache" "sfmc" {
  name                = var.sfmc_redis_cache_name
  resource_group_name = "rg-loyaltyapi-${local.environment_sanitized}"
}

resource "azurerm_key_vault_secret" "redis_omnichannel_connection_string" {
  name         = "redis-omnichannel-connection-string-primary"
  value        = data.azurerm_redis_cache.omnichannel.primary_connection_string
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}

resource "azurerm_key_vault_secret" "redis_sfmc_connection_string" {
  name         = "redis-sfmc-connection-string-primary"
  value        = data.azurerm_redis_cache.sfmc.primary_connection_string
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
