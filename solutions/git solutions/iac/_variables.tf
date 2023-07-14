variable "environment" {
  type        = string
  description = "(required) environment name for deployment"
  default     = "dev"
}

variable "organization_suffix" {
  type        = string
  description = "(required) organizational suffix to seed into globaly unique names for resources"
  default     = "seg"
}

variable "location" {
  type        = string
  description = "(required) azure region where resources will be deployed"
  default     = "East US"
}

variable "app_name" {
  type        = string
  description = "(required) application name"
}

variable "app_abbr" {
  type        = string
  description = "(required) application abbreviation"
}

variable "app_service_kind" {
  type        = string
  description = "(required) kind setting for App Service Plan. Ex: elastic, Windows, Linux, FunctionApp"
  default     = "FunctionApp"
}

variable "app_service_tier" {
  type        = string
  description = "(optional) sku tier for App Service Plan. Ex: ElasticPremium, Dynamic, or Standard, Premium, PremiumV2"
  default     = "Dynamic"
}

variable "app_service_sku" {
  type        = string
  description = "(optional) sku size for App Service Plan. Ex: EP1, Y1, S1"
  default     = "Y1"
}

variable "function_apps_warm_instance_count" {
  type        = number
  description = "(required) number of warm instances to keep for the function apps"
  default     = 0
}

variable "runtime_scale_monitoring_enabled" {
  type        = bool
  description = "(required) enable runtime scale monitoring, only available on Premium plans"
  default     = false
}

variable "api_deployment_number" {
  type        = string
  description = "API Function App deployment number"
  default     = "001"
}

variable "plan_name" {
  type        = string
  description = "(required) base name for App Service Plan"
  default     = "asp-digitalmktg-apps"
}

variable "plan_resource_group_name" {
  type        = string
  description = "(required) base name for Resource Group containing shared App Service Plan"
  default     = "rg-asp-apps"
}

variable "digitalmktg_vault_name" {
  type        = string
  description = "(required) DigitalMktg Key Vault name"
}

variable "digitalmktg_vault_resource_group_name" {
  type        = string
  description = "(required) name for Resource Group containing DigitalMktg Key Vault"
}

# app specific variables

variable "eagleeye_base_url" {
  type        = string
  description = "(required) eagle eye base url"
}

variable "eagleeye_campaigns_base_url" {
  type        = string
  description = "(required) eagle eye campaigns base url"
}

variable "salesforce_auth_base_url" {
  type        = string
  description = "(required) salesforce auth base url"
}

variable "salesforce_rest_base_url" {
  type        = string
  description = "(required) salesforce rest base url"
}

variable "sfmc_redis_cache_name" {
  type        = string
  description = "(required) name for SFMC Redis Cache"
}
