output "eagleeye_api_function_app_name" {
  value = module.eagleeye_api_app.function_app_name
}
output "apim_resource_group_name" {
  value = data.azurerm_api_management.apim_instance.resource_group_name
}
output "apim_name" {
  value = data.azurerm_api_management.apim_instance.name
}
output "apim_eagleeye_product_id" {
  value = azurerm_api_management_product.eagleeye.product_id
}
output "apim_eagleeye_backend_id" {
  value = module.eagleeye_api_app_apim.apim_api_backend_name
}
