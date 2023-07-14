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
  description = "(required) primary azure region where resources will be deployed"
}
variable "app_service_resource_group_name" {
  type        = string
  description = "existing app service resource group name for SEG loyalty (loyalty services) API"
}
variable "app_service_name" {
  type        = string
  description = "existing app service name for SEG loyalty (loyalty services) API"
}