
resource "azurerm_resource_group" "eagleeye_apis" {
  name     = "rg-eagleeye-apis-${local.environment_sanitized}"
  location = local.region_sanitized
}
resource "azurerm_resource_group" "eagleeye_security" {
  name     = "rg-eagleeyeapi-security-${local.environment_sanitized}"
  location = local.region_sanitized
}

data "azurerm_redis_cache""eagleeye_cache"{
name                 = "redis-omnichannel-seg-${local.environment_sanitized}"
resource_group_name ="rg-omnichannel-cache-${local.environment_sanitized}"
}
