data "azurerm_key_vault" "digitalmktg_vault" {
  name                = var.digitalmktg_vault_name
  resource_group_name = var.digitalmktg_vault_resource_group_name
}

data "azurerm_key_vault_secret" "digitalmktg_secrets" {
  for_each     = toset(local.digitalmktg_secrets)
  name         = each.value
  key_vault_id = data.azurerm_key_vault.digitalmktg_vault.id
}

resource "azurerm_key_vault" "main" {
  name                        = "kv-${local.app_abbr_sanitized}api-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  location                    = azurerm_resource_group.security.location
  resource_group_name         = azurerm_resource_group.security.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  purge_protection_enabled    = false
  sku_name                    = "standard"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_key_vault_access_policy" "azure_devops_kv_access_policy" {
  key_vault_id = azurerm_key_vault.main.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

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
    "GetIssuers",
    "Import",
    "List",
    "ListIssuers",
    "ManageContacts",
    "ManageIssuers",
    "Purge",
    "Recover",
    "Restore",
    "SetIssuers",
    "Update"
  ]

  storage_permissions = [
    "Backup",
    "Delete",
    "DeleteSAS",
    "Get",
    "GetSAS",
    "List",
    "ListSAS",
    "Purge",
    "Recover",
    "RegenerateKey",
    "Restore",
    "Set",
    "SetSAS",
    "Update"
  ]
}


resource "azurerm_key_vault_secret" "digitalmktg_secrets" {
  for_each     = toset(local.digitalmktg_secrets)
  name         = each.value
  value        = data.azurerm_key_vault_secret.digitalmktg_secrets[each.value].value
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}
