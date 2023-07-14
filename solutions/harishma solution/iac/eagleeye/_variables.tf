variable "environment" {
  type        = string
  description = "(required) environment name for deployment"
  default = "dev"
}
variable "organization_suffix" {
  type        = string
  description = "(required) organizational suffix to seed into globaly unique names for resources"
  default = "seg"
}
variable "location" {
  type        = string
  description = "(required) azure region where resources will be deployed"
  default = "East US"
}
variable "app_service_kind" {
  type        = string
  description = "(required) kind setting for App Service Plan. Ex: elastic, Windows, Linux, FunctionApp"
  default = "FunctionApp"
}
variable "app_service_tier" {
  description = "(optional) sku tier for App Service Plan. Ex: ElasticPremium, Dynamic, or Standard, Premium, PremiumV2"
  type        = string
  default     = "ElasticPremium"
}
variable "app_service_sku" {
  description = "(optional) sku size for App Service Plan. Ex: EP1, Y1, S1"
  type        = string
  default     = "EP1"
}

variable "function_apps_warm_instance_count" {
  type        = number
  description = "(required) number of warm instances to keep for the function apps"
}

variable "runtime_scale_monitoring_enabled" {
  type        = bool
  description = "(required) enable runtime scale monitoring, only available on Premium plans"
  default = false
}
variable "eagleeye_api_deployment_number" {
  description = "Eagle Eye API Function App deploy number"
  type        = string
  default     = "001"
}
variable "omnichannel_kv_rg" {
  description = "Resource grooup for DigitalMktg Key Vault"
  type        = string
  default     = ""
}
variable "cosmos_db_rg" {
  description = "Existing Eagle eye Cosmos DB Resource Group"
  type        = string
  default     = ""
}
variable "store_locator_address_db_rg" {
  description = "Existing Store Locator Cosmos DB Resource Group"
  type        = string
  default     = ""
}

variable "client_id_ee" {
  description = "eagle eye client ID "
  type        = string
  default     = ""
}

variable "secret_ee" {
  description = "eagle eye secret"
  type        = string
  default     = ""
}

variable "base_url_ee" {
  description = "eagle eye base url"
  type        = string
  default     = ""
}

variable "base_url_campaigns_ee" {
  description = "eagle eye campaigns base url"
  type        = string
  default     = ""
}

variable "bilo_partner_code" {
  description = "bilo banner code"
  type        = string
  default     = ""
}

variable "fresco_partner_code" {
  description = "fresco banner code"
  type        = string
  default     = ""
}

variable "harveys_partner_code" {
  description = "harveys banner code"
  type        = string
  default     = ""
}

variable "wd_partner_code" {
  description = "wd banner code"
  type        = string
  default     = ""
}

variable "cache_server" {
  description = "cache serve url"
  type        = string
  default     = ""
}

variable "redis_connection_timeout" {
  description = "redis_connection_timeout"
  type        = string
  default     = ""
}

variable "plan_name" {
  description = "plan Name"
  type        = string
  default     = ""
}

variable "ee_healthcheck_memberid" {
  description = "EE Healthcheck Member id"
  type        = string
  default     = ""
}

variable "digitalmktg_vault_name" {
  type        = string
  description = "(required) DigitalMktg Key Vault name"
}

variable "digitalmktg_vault_resource_group_name" {
  type        = string
  description = "(required) name for Resource Group containing DigitalMktg Key Vault"
}
