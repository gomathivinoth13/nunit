variable "environment" {
  type        = string
  description = "(required) environment name for deployment"
}
variable "organization_suffix" {
  type        = string
  description = "(required) organizational suffix to seed into globaly unique names for resources"
}
variable "location" {
  type        = string
  description = "(required) azure region where resources will be deployed"
}
variable "app_service_kind" {
  type        = string
  description = "(required) kind setting for App Service Plan. Ex: elastic, Windows, Linux, FunctionApp"
  default     = "FunctionApp"
}

variable "app_service_tier" {
  description = "(optional) sku tier for App Service Plan. Ex: ElasticPremium, Dynamic, or Standard, Premium, PremiumV2"
  type        = string
  default     = "Dynamic"
}

variable "app_service_sku" {
  description = "(optional) sku size for App Service Plan. Ex: EP1, Y1, S1"
  type        = string
  default     = "Y1"
}

variable "plan_name" {
  description = "(required) base name for App Service Plan."
  type        = string
  default     = "asp-digitalmktg-apps"
}
variable "plan_resource_group_name" {
  description = "(required) base name for Resource Group containing shared App Service Plan."
  type        = string
  default     = "rg-asp-apps"
}
variable "function_apps_warm_instance_count" {
  type        = number
  description = "(required) number of warm instances to keep for the function apps"
  default = 0
}
variable "runtime_scale_monitoring_enabled" {
  type        = bool
  description = "(required) enable runtime scale monitoring, only available on Premium plans"
  default = false
}
variable "ecreboprocessor_api_deployment_number" {
  description = "Ecrebo Processor API Function App deploy number"
  type        = string
  default     = "001"
}

variable "digitalmktg_vault_name" {
  type        = string
  description = "(required) DigitalMktg Key Vault name"
}

variable "digitalmktg_vault_resource_group_name" {
  type        = string
  description = "(required) name for Resource Group containing DigitalMktg Key Vault"
}
