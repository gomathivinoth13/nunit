resource "azurerm_storage_account" "shared_sa" {
  name                     = join("", [substr("st${local.environment_sanitized}erapp${replace(local.org_suffix_sanitized, "-", "")}", 0, 23)])
  resource_group_name      = "rg-ereceipts-apis-${local.environment_sanitized}"
  location                 = local.region_sanitized
  account_tier             = "Standard"
  account_replication_type = "LRS"

  account_kind = "StorageV2"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
