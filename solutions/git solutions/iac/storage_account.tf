resource "azurerm_storage_account" "main" {
  name                     = join("", [substr("st${local.environment_sanitized}${local.app_abbr_sanitized}api${replace(local.org_suffix_sanitized, "-", "")}", 0, 23)])
  resource_group_name      = azurerm_resource_group.apis.name
  location                 = azurerm_resource_group.apis.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "StorageV2"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
