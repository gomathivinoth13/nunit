resource "azurerm_api_management_product" "loyalty_services_azure_internal" {
  product_id            = local.apim_loyalty_services_azure_product_id_internal
  api_management_name   = data.azurerm_api_management.seg_apim.name
  resource_group_name   = data.azurerm_api_management.seg_apim.resource_group_name
  display_name          = "Loyalty Services - Azure"
  subscription_required = true
  subscriptions_limit   = 2
  approval_required     = true
  published             = true
}
resource "azurerm_api_management_product_group" "loyalty_services_azure_internal_newsignature" {
  product_id          = azurerm_api_management_product.loyalty_services_azure_internal.product_id
  group_name          = "newsignature-team"
  api_management_name = data.azurerm_api_management.seg_apim.name
  resource_group_name = data.azurerm_api_management.seg_apim.resource_group_name
}
resource "azurerm_api_management_product_group" "loyalty_services_azure_internal_middleware" {
  product_id          = azurerm_api_management_product.loyalty_services_azure_internal.product_id
  group_name          = "middleware-team"
  api_management_name = data.azurerm_api_management.seg_apim.name
  resource_group_name = data.azurerm_api_management.seg_apim.resource_group_name
}
resource "azurerm_api_management_product_group" "loyalty_services_azure_internal_digitalmarketing" {
  product_id          = azurerm_api_management_product.loyalty_services_azure_internal.product_id
  group_name          = "digitalmarketing-team"
  api_management_name = data.azurerm_api_management.seg_apim.name
  resource_group_name = data.azurerm_api_management.seg_apim.resource_group_name
}