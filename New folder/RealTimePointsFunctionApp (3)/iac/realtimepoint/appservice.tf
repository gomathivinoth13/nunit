﻿resource "azurerm_app_service_plan" "main" {
  count = local.use_local_plan ? 1 : 0
  name                = "asp-realtimepointsfunction-app-${local.environment_sanitized}"
  location            = azurerm_resource_group.realtimepoints_apis.location
  resource_group_name = azurerm_resource_group.realtimepoints_apis.name
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