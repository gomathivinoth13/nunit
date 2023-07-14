# Application

This document is intend as a high-level overview of the `Wallet Account Data Processor` application.

|Application Information  |                    |
|:------------------------|:-------------------|
|Application name         |walletaccountdataprocessor |
|Application abbreviation |wadproc             |
|Application type         |                    |

## Subscriptions

An Azure subscription is a logical container used to provision resources in Azure. The subscriptions used by the `Wallet Account Data Processor` application are listed below.

|Subscription Id |Subscription Name |
|:---------------|:-----------------|
|490b96bd-0560-4953-872d-6ed5e1147222 |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|00966c0f-e5c0-4ae2-954f-a96b6a9af123 |SEGDEVOPS Bi-Lo Holdings DM API PROD |

## Environments

The environments used by the `Wallet Account Data Processor` application are listed below:

|Environment |Subscription Name |
|:-----------|:-----------------|
|DEV  |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|QA   |SEGDEVOPS Bi-Lo Holdings DM API Dev  |
|PROD |SEGDEVOPS Bi-Lo Holdings DM API PROD |

## Dependencies

Resources that the `Wallet Account Data Processor` application is dependant on during provisioning.

### Dependencies DEV

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Key Vault       |DigitalMktg-Vault-DEV |ActiveDirectory-DEV      ||

### Dependencies QA

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Key Vault       |DigitalMktg-Vault-QA |ActiveDirectory-DEV     ||

### Dependencies PROD

|Type |Name |Resource Group |Note |
|:----|:----|:--------------|:----|
|Azure Key Vault       |DigitalMktg-Vault-Prod |Security                  ||

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
