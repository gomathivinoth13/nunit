# Key Vault

This document is intend as a high-level overview of the `Wallet Account Data Processor` application secrets.

## Secrets DEV

The secrets required by the application in the DEV environment are stored in the `kv-wadprocapi-seg-dev` key vault and listed below.

|Name |Source |
|:----|:------|
|DynatraceAgentToken |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |

## Secrets QA

The secrets required by the application in the QA environment are stored in the `kv-wadprocapi-seg-qa` key vault and listed below.

|Name |Source |
|:----|:------|
|DynatraceAgentToken |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |

## Secrets PROD

The secrets required by the application in the PROD environment are stored in the `kv-wadprocapi-seg-prod` key vault and listed below.

|Name |Source |
|:----|:------|
|DynatraceAgentToken |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
