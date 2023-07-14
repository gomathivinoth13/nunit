# Application

This document is intend as a high-level overview of the `MBO Notifications SFMC` application.

|Application Information  |                    |
|:------------------------|:-------------------|
|Application name         |mbonotificationsfmc |
|Application abbreviation |mbosfmc             |
|Application type         |                    |

## Subscriptions

An Azure subscription is a logical container used to provision resources in Azure. The subscriptions used by the `MBO Notifications SFMC` application are listed below.

|Subscription Id |Subscription Name |
|:---------------|:-----------------|
|490b96bd-0560-4953-872d-6ed5e1147222 |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|00966c0f-e5c0-4ae2-954f-a96b6a9af123 |SEGDEVOPS Bi-Lo Holdings DM API PROD |

## Environments

The environments used by the `MBO Notifications SFMC` application are listed below:

|Environment |Subscription Name |
|:-----------|:-----------------|
|DEV  |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|QA   |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|PROD |SEGDEVOPS Bi-Lo Holdings DM API PROD |

## Dependencies

Resources that the `MBO Notifications SFMC` application is dependant on during provisioning.

### Dependencies DEV

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Cache for Redis |redis-omnichannel-seg-dev |rg-omnichannel-cache-dev ||
|Azure Cache for Redis |Redis-SFMC-Cache-Dev      |rg-loyaltyapi-dev        ||
|Azure Key Vault       |DigitalMktg-Vault-DEV     |ActiveDirectory-DEV      ||

### Dependencies QA

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Cache for Redis |redis-omnichannel-seg-qa |rg-omnichannel-cache-qa ||
|Azure Cache for Redis |Redis-SFMC-Cache-QA      |rg-loyaltyapi-qa        ||
|Azure Key Vault       |DigitalMktg-Vault-QA     |ActiveDirectory-DEV     ||

### Dependencies PROD

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Cache for Redis |redis-omnichannel-seg-prod |rg-omnichannel-cache-prod ||
|Azure Cache for Redis |Redis-SFMC-Cache-Prod      |rg-loyaltyapi-prod        ||
|Azure Key Vault       |DigitalMktg-Vault-Prod     |Security                  ||

## Dependants

The services that are currently consuming this application are listed below.

### Internal

|Name |Subscription Key |
|:----|:----------------|
|N/A  |N/A              |

### External

|Name |Subscription Key |
|:----|:----------------|
|N/A  |N/A              |
