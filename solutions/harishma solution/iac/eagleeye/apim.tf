data "azurerm_api_management" "apim_instance" {
  name                = "apim-api-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  resource_group_name = "rg-api-${local.environment_sanitized}"
}

resource "azurerm_api_management_product" "eagleeye" {
  product_id            = "eagleeye"
  api_management_name   = data.azurerm_api_management.apim_instance.name
  resource_group_name   = data.azurerm_api_management.apim_instance.resource_group_name
  display_name          = "eagleeye"
  subscription_required = true
  approval_required     = true
  published             = true
  subscriptions_limit   = 1
}


 data "external" "get_apim_base_url" {
  program = ["powershell","$key='baseurl'; $value=az apim show -n apim-api-seg-${local.environment_sanitized} -g rg-api-${local.environment_sanitized} --query id -o tsv; Write-Output \"{\"\"$${key}\"\":\"\"$${value}\"\"}\""]
 }

 data "external" "get_ocp_apim_sub_key" {
  program = ["powershell","$key='primaryKey'; $value=az rest --method post --uri ${data.external.get_apim_base_url.result.baseurl}/subscriptions/MarketingIntegrationTeam-Internal/listSecrets?api-version=2021-08-01 --query primaryKey -o tsv; Write-Output \"{\"\"$${key}\"\":\"\"$${value}\"\"}\""]
 }

resource "azurerm_key_vault_secret" "ocp_apim_subscription_key" {
  name         = "ocp-apim-sub-key-${local.app_name_sanitized}"
  value        = data.external.get_ocp_apim_sub_key.result.primaryKey
  key_vault_id = azurerm_key_vault.main.id

  depends_on = [
    azurerm_key_vault_access_policy.azure_devops_kv_access_policy
  ]
}



