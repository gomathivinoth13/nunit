terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.76.0"
    }
    azapi = {
      source = "Azure/azapi"
    }
    time = {
      source  = "hashicorp/time"
      version = "0.7.1"
    }
  }
}

provider "azurerm" {
  features {}
}

provider "azapi" {
}