terraform {
  backend "azurerm" {
    #resource_group_name = "rg-terraform-dev" #used for local development and testing using dev state loyalty
  }
}

locals {
  environment_sanitized                           = lower(var.environment)
  org_suffix_sanitized                            = lower(var.organization_suffix)
  region_sanitized                                = lower(var.location)
  apim_loyalty_services_azure_product_id_internal = "loyaltyservicesazure"
}

data "azurerm_client_config" "current" {}
data "azurerm_subscription" "current" {}

data "azurerm_api_management" "seg_apim" {
  name                = "apim-api-${local.org_suffix_sanitized}-${local.environment_sanitized}"
  resource_group_name = "rg-api-${local.environment_sanitized}"
}

data "azurerm_app_service" "loyalty_services_azure" {
  name                = var.app_service_name
  resource_group_name = var.app_service_resource_group_name
}