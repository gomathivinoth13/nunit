resource "azurerm_app_service_plan" "main" {
  name                = "${var.plan_name}-${var.environment}"
  location            = azurerm_resource_group.eagleeye_apis.location
  resource_group_name = azurerm_resource_group.eagleeye_apis.name
  kind                = var.app_service_kind
   
  sku {
    tier = var.app_service_tier
    size = var.app_service_sku
  }

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}