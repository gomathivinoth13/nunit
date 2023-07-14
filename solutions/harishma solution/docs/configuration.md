# Configuration

This document is intend as a high-level overview of the `Wallet Account Data Processor` application configuration. Application settings are created by Terraform during provisioning and exposed as environment variables for access by the application at runtime.

## Application Settings DEV

The application settings required by the application in the DEV environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet-isolated                                                                |Plain Text |

## Application Settings QA

The application settings required by the application in the QA environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet-isolated                                                                |Plain Text |
|DT_TENANT                      |qds48093                                                                       |Plain Text |
|DT_SSL_MODE                    |default                                                                        |Plain Text |
|DT_API_TOKEN                   |Key Vault reference to `DynatraceAgentToken`                                   |Key Vault  |

## Application Settings PROD

The application settings required by the application in the PROD environment are listed below.

|Name                           |Value                                                                          |Type       |
|:------------------------------|:------------------------------------------------------------------------------|:----------|
|FUNCTIONS_WORKER_RUNTIME       |dotnet-isolated                                                                |Plain Text |
|DT_TENANT                      |qds48093                                                                       |Plain Text |
|DT_SSL_MODE                    |default                                                                        |Plain Text |
|DT_API_TOKEN                   |Key Vault reference to `DynatraceAgentToken`                                   |Key Vault  |

## Depreciated Application Settings

Application settings for the `Wallet Account Data Processor` application that are no longer used and can be scheduled for removal.

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
