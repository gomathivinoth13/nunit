# Infrastructure

This document is intend as a high-level overview of the resources used by the `MBO Notifications SFMC` application in each environment.

## Infrastructure DEV

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-mbonotificationsfmc-apis-dev           |East US||
|Resource Group        |rg-mbonotificationsfmc-security-dev       |East US||
|App Service Plan      |asp-mbonotificationsfmc-apps-dev          |East US|Y1: 0 |
|Key Vault             |kv-mbosfmcapi-seg-dev                     |East US|Standard |
|Function App          |func-mbonotificationsfmc-api-seg-dev-001  |East US||
|Storage Account       |stdevmbosfmcapiseg                        |East US|Standard (general purpose v1) LRS |
|Application Insights  |appi-mbonotificationsfmc-apis-dev         |East US|Web |

## Infrastructure QA

For this environment, the `MBO Notifications SFMC` application uses the shared App Service Plan `asp-digitalmktg-apps-qa` in resource group `rg-asp-apps-qa`. The App Service Plan was created and is controlled by the Cloud team.

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-mbonotificationsfmc-apis-qa            |East US||
|Resource Group        |rg-mbonotificationsfmc-security-qa        |East US||
|Key Vault             |kv-mbosfmcapi-seg-qa                      |East US|Standard |
|Function App          |func-mbonotificationsfmc-api-seg-qa-001   |East US||
|Storage Account       |stqambosfmcapiseg                         |East US|Standard (general purpose v1) LRS |
|Application Insights  |appi-mbonotificationsfmc-apis-qa          |East US|Web |

## Infrastructure PROD

For this environment, the `MBO Notifications SFMC` application uses the shared App Service Plan `asp-digitalmktg-apps-prod` in resource group `rg-asp-apps-prod`. The App Service Plan was created and is controlled by the Cloud team.

|Type |Name |Region |Note |
|:----|:----|:------|:----|
|Resource Group        |rg-mbonotificationsfmc-apis-prod          |East US||
|Resource Group        |rg-mbonotificationsfmc-security-prod      |East US||
|Key Vault             |kv-mbosfmcapi-seg-prod                    |East US|Standard |
|Function App          |func-mbonotificationsfmc-api-seg-prod-001 |East US||
|Storage Account       |stprodmbosfmcapiseg                       |East US|Standard (general purpose v1) LRS |
|Application Insights  |appi-mbonotificationsfmc-apis-prod        |East US|Web |

## Depreciated Infrastructure

Resources for the `MBO Notifications SFMC` application that are no longer used and can be scheduled for removal.

### Depreciated Infrastructure DEV

|Type |Name |Note |
|:----|:----|:----|
|App Service Plan      |ASP-rgloyaltyapidev-9922     ||
|Function App          |Func-MboNotificationSFMC-Dev ||
|Storage Account       |storageaccountrgloya657      ||
|Application Insights  |Func-MboNotificationSFMC-Dev ||

### Depreciated Infrastructure QA

|Type |Name |Note |
|:----|:----|:----|
|App Service Plan      |ASP-rgloyaltyapiqa-adc5      ||
|Function App          |Func-MboNotificationSFMC-QA  ||
|Storage Account       |storageaccountrgloya8314     ||
|Application Insights  |Func-MboNotificationSFMC-QA  ||

### Depreciated Infrastructure PROD

|Type |Name |Note |
|:----|:----|:----|
|App Service Plan      |ASP-rgloyaltyapiprod-ac89     ||
|Function App          |Func-MboNotificationSFMC-Prod ||
|Storage Account       |storageaccountrgloya9b9       ||
|Application Insights  |Func-MboNotificationSFMC-Prod ||
