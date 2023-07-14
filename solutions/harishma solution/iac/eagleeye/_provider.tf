terraform {
  required_providers {
    azurerm = {
    source  = "hashicorp/azurerm"
    version = "~>3.29.0"
    }
    azapi = {
      source = "Azure/azapi"
    }

    external = {
      source = "hashicorp/external"
      version = "2.3.1"
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