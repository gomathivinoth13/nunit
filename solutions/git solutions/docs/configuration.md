# Configuration

This document is intend as a high-level overview of the `MBO Notifications SFMC` application configuration. Application settings are created by Terraform during provisioning and exposed as environment variables for access by the application at runtime.

## Application Settings DEV

The application settings required by the application in the DEV environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet                                                                         |Plain Text |
|baseUrlCampaignsEE             |`https://dev-api.segrocers.com/EE`                                             |Plain Text |
|baseUrlEE                      |`https://dev-api.segrocers.com/EE`                                             |Plain Text |
|SalesForceAuthEndPoint         |`https://dev-api.segrocers.com/sfmcauthservice/`                               |Plain Text |
|SalesForceRestEndPoint         |`https://dev-api.segrocers.com/sfmcservice/`                                   |Plain Text |
|clientIDEE                     |Key Vault reference to `azurembonotificationsfmc-eagleeye-clientid`            |Key Vault  |
|secretEE                       |Key Vault reference to `azurembonotificationsfmc-eagleeye-secret`              |Key Vault  |
|ocpApimSubscriptionKey         |Key Vault reference to `ocp-apim-subscription-key`                             |Key Vault  |
|redisCampaignConnectionString  |Key Vault reference to `redis-omnichannel-connection-string-primary`           |Key Vault  |
|DataBaseConnectionString       |Key Vault reference to `Loyalty-Azure-DB-Conn-String`                          |Key Vault  |
|RedisConnectionString          |Key Vault reference to `redis-sfmc-connection-string-primary`                  |Key Vault  |
|Fresco_ClientID                |Key Vault reference to `azurembonotificationsfmc-fresco-clientid`              |Key Vault  |
|Fresco_ClientSecret            |Key Vault reference to `azurembonotificationsfmc-fresco-secret`                |Key Vault  |
|FrescoEventDefinitionKey       |Key Vault reference to `azurembonotificationsfmc-fresco-eventdefinitionkey`    |Key Vault  |
|Harveys_ClientID               |Key Vault reference to `azurembonotificationsfmc-harveys-clientid`             |Key Vault  |
|Harveys_ClientSecret           |Key Vault reference to `azurembonotificationsfmc-harveys-secret`               |Key Vault  |
|HarveysEventDefinitionKey      |Key Vault reference to `azurembonotificationsfmc-harveys-eventdefinitionkey`   |Key Vault  |
|WinnDixie_ClientID             |Key Vault reference to `azurembonotificationsfmc-winndixie-clientid`           |Key Vault  |
|WinnDixie_ClientSecret         |Key Vault reference to `azurembonotificationsfmc-winndixie-secret`             |Key Vault  |
|WDEventDefinitionKey           |Key Vault reference to `azurembonotificationsfmc-winndixie-eventdefinitionkey` |Key Vault  |

## Application Settings QA

The application settings required by the application in the QA environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet                                                                         |Plain Text |
|baseUrlCampaignsEE             |`https://qa-api.segrocers.com/EE`                                              |Plain Text |
|baseUrlEE                      |`https://qa-api.segrocers.com/EE`                                              |Plain Text |
|SalesForceAuthEndPoint         |`https://qa-api.segrocers.com/sfmcauthservice/`                                |Plain Text |
|SalesForceRestEndPoint         |`https://qa-api.segrocers.com/sfmcservice/`                                    |Plain Text |
|clientIDEE                     |Key Vault reference to `azurembonotificationsfmc-eagleeye-clientid`            |Key Vault  |
|secretEE                       |Key Vault reference to `azurembonotificationsfmc-eagleeye-secret`              |Key Vault  |
|ocpApimSubscriptionKey         |Key Vault reference to `ocp-apim-subscription-key`                             |Key Vault  |
|redisCampaignConnectionString  |Key Vault reference to `redis-omnichannel-connection-string-primary`           |Key Vault  |
|DataBaseConnectionString       |Key Vault reference to `Loyalty-Azure-DB-Conn-String`                          |Key Vault  |
|RedisConnectionString          |Key Vault reference to `redis-sfmc-connection-string-primary`                  |Key Vault  |
|Fresco_ClientID                |Key Vault reference to `azurembonotificationsfmc-fresco-clientid`              |Key Vault  |
|Fresco_ClientSecret            |Key Vault reference to `azurembonotificationsfmc-fresco-secret`                |Key Vault  |
|FrescoEventDefinitionKey       |Key Vault reference to `azurembonotificationsfmc-fresco-eventdefinitionkey`    |Key Vault  |
|Harveys_ClientID               |Key Vault reference to `azurembonotificationsfmc-harveys-clientid`             |Key Vault  |
|Harveys_ClientSecret           |Key Vault reference to `azurembonotificationsfmc-harveys-secret`               |Key Vault  |
|HarveysEventDefinitionKey      |Key Vault reference to `azurembonotificationsfmc-harveys-eventdefinitionkey`   |Key Vault  |
|WinnDixie_ClientID             |Key Vault reference to `azurembonotificationsfmc-winndixie-clientid`           |Key Vault  |
|WinnDixie_ClientSecret         |Key Vault reference to `azurembonotificationsfmc-winndixie-secret`             |Key Vault  |
|WDEventDefinitionKey           |Key Vault reference to `azurembonotificationsfmc-winndixie-eventdefinitionkey` |Key Vault  |

## Application Settings PROD

The application settings required by the application in the PROD environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet                                                                         |Plain Text |
|baseUrlCampaignsEE             |`https://api.segrocers.com/EE`                                                 |Plain Text |
|baseUrlEE                      |`https://api.segrocers.com/EE`                                                 |Plain Text |
|SalesForceAuthEndPoint         |`https://api.segrocers.com/sfmcauthservice/`                                   |Plain Text |
|SalesForceRestEndPoint         |`https://api.segrocers.com/sfmcservice/`                                       |Plain Text |
|clientIDEE                     |Key Vault reference to `azurembonotificationsfmc-eagleeye-clientid`            |Key Vault  |
|secretEE                       |Key Vault reference to `azurembonotificationsfmc-eagleeye-secret`              |Key Vault  |
|ocpApimSubscriptionKey         |Key Vault reference to `ocp-apim-subscription-key`                             |Key Vault  |
|redisCampaignConnectionString  |Key Vault reference to `redis-omnichannel-connection-string-primary`           |Key Vault  |
|DataBaseConnectionString       |Key Vault reference to `Loyalty-Azure-DB-Conn-String`                          |Key Vault  |
|RedisConnectionString          |Key Vault reference to `redis-sfmc-connection-string-primary`                  |Key Vault  |
|Fresco_ClientID                |Key Vault reference to `azurembonotificationsfmc-fresco-clientid`              |Key Vault  |
|Fresco_ClientSecret            |Key Vault reference to `azurembonotificationsfmc-fresco-secret`                |Key Vault  |
|FrescoEventDefinitionKey       |Key Vault reference to `azurembonotificationsfmc-fresco-eventdefinitionkey`    |Key Vault  |
|Harveys_ClientID               |Key Vault reference to `azurembonotificationsfmc-harveys-clientid`             |Key Vault  |
|Harveys_ClientSecret           |Key Vault reference to `azurembonotificationsfmc-harveys-secret`               |Key Vault  |
|HarveysEventDefinitionKey      |Key Vault reference to `azurembonotificationsfmc-harveys-eventdefinitionkey`   |Key Vault  |
|WinnDixie_ClientID             |Key Vault reference to `azurembonotificationsfmc-winndixie-clientid`           |Key Vault  |
|WinnDixie_ClientSecret         |Key Vault reference to `azurembonotificationsfmc-winndixie-secret`             |Key Vault  |
|WDEventDefinitionKey           |Key Vault reference to `azurembonotificationsfmc-winndixie-eventdefinitionkey` |Key Vault  |

## Depreciated Application Settings

Application settings for the `MBO Notifications SFMC` application that are no longer used and can be scheduled for removal.

### Depreciated Application Settings DEV

|Name |Note |
|:----|:----|
|N/A  |     |

### Depreciated Application Settings QA

|Name |Note |
|:----|:----|
|N/A  |     |

### Depreciated Application Settings PROD

|Name |Note |
|:----|:----|
|N/A  |     |
