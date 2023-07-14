# Infrastructure

This document is intend as a high-level overview of the resources used by the `Wallet Account Data Processor` application in each environment.

## Infrastructure DEV

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-walletaccountdataprocessor-apis-dev           |East US||
|Resource Group        |rg-walletaccountdataprocessor-security-dev       |East US||
|App Service Plan      |asp-walletaccountdataprocessor-apps-dev          |East US|Y1: 0 |
|Key Vault             |kv-wadprocapi-seg-dev                     |East US|Standard |
|Function App          |func-walletaccountdataprocessor-api-seg-dev-001  |East US||
|Storage Account       |stdevwadprocapiseg                        |East US|Standard (general purpose v2) LRS |
|Application Insights  |appi-walletaccountdataprocessor-apis-dev         |East US|Web |

## Infrastructure QA

For this environment, the `Wallet Account Data Processor` application uses the shared App Service Plan `asp-digitalmktg-apps-qa` in resource group `rg-asp-apps-qa`. The App Service Plan was created and is controlled by the Cloud team.

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-walletaccountdataprocessor-apis-qa            |East US||
|Resource Group        |rg-walletaccountdataprocessor-security-qa        |East US||
|Key Vault             |kv-wadprocapi-seg-qa                      |East US|Standard |
|Function App          |func-walletaccountdataprocessor-api-seg-qa-001   |East US||
|Storage Account       |stqawadprocapiseg                         |East US|Standard (general purpose v2) LRS |
|Application Insights  |appi-walletaccountdataprocessor-apis-qa          |East US|Web |

## Infrastructure PROD

For this environment, the `Wallet Account Data Processor` application uses the shared App Service Plan `asp-digitalmktg-apps-prod` in resource group `rg-asp-apps-prod`. The App Service Plan was created and is controlled by the Cloud team.

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-walletaccountdataprocessor-apis-prod          |East US||
|Resource Group        |rg-walletaccountdataprocessor-security-prod      |East US||
|Key Vault             |kv-wadprocapi-seg-prod                    |East US|Standard |
|Function App          |func-walletaccountdataprocessor-api-seg-prod-001 |East US||
|Storage Account       |stprodwadprocapiseg                       |East US|Standard (general purpose v2) LRS |
|Application Insights  |appi-walletaccountdataprocessor-apis-prod        |East US|Web |
