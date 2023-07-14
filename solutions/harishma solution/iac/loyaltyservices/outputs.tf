output "app_service_name" {
  value = data.azurerm_app_service.loyalty_services_azure.name
}
output "app_service_hostname" {
  value = data.azurerm_app_service.loyalty_services_azure.default_site_hostname
}
output "apim_name" {
  value = data.azurerm_api_management.seg_apim.name
}
output "apim_resource_group_name" {
  value = data.azurerm_api_management.seg_apim.resource_group_name
}
output "api_products" {
  value = "${azurerm_api_management_product.loyalty_services_azure_internal.product_id}\n"
}