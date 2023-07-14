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
variable "realtime_api_deployment_number" {
  description = "real time  API Function App deploy number"
  type        = string
  default     = "001"
}
variable "salesforceapimbaseendpoint"{
     type = string
}
variable "salesforceapimauthendpoint"{
     type = string
}
variable "ocpapimsubscriptionkey"{
     type = string
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
variable "cache_server" {
  description = "cache serve url"
  type        = string
  default     = ""
}
variable "base_url_campaigns_ee" {
  description = "eagle eye campaigns base url"
  type        = string
  default     = ""
}
variable "redis_connection_timeout" {
  description = "redis_connection_timeout"
  type        = string
  default     = ""
}
variable "seg_key" {
  description = "seg key"
  type        = string
  default     = ""
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

variable "digitalmktg_vault_name" {
  type        = string
  description = "(required) DigitalMktg Key Vault name"
}

variable "digitalmktg_vault_resource_group_name" {
  type        = string
  description = "(required) name for Resource Group containing DigitalMktg Key Vault"
}
variable "segclientid" {
  type        = string
  description = "(required) client id"
}
variable "segclientsecret" {
  type        = string
  description = "(required) client secret"
}