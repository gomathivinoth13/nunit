# Key Vault

This document is intend as a high-level overview of the `MBO Notifications SFMC` application secrets.

## Secrets DEV

The secrets required by the application in the DEV environment are stored in the `kv-mbosfmcapi-seg-dev` key vault and listed below.

|Name |Source |
|:----|:------|
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-clientid              |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-secret                |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-eventdefinitionkey    |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-clientid             |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-secret               |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-eventdefinitionkey   |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-clientid           |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-secret             |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-eventdefinitionkey |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|Loyalty-Azure-DB-Conn-String                          |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|ocp-apim-subscription-key                             |Copied from `DigitalMktg-Vault-DEV` by Terraform during provisioning |
|redis-sfmc-connection-string-primary                  |Created by Terraform during provisioning |
|redis-omnichannel-connection-string-primary           |Created by Terraform during provisioning |

## Secrets QA

The secrets required by the application in the QA environment are stored in the `kv-mbosfmcapi-seg-qa` key vault and listed below.

|Name |Source |
|:----|:------|
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-clientid              |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-secret                |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-eventdefinitionkey    |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-clientid             |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-secret               |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-eventdefinitionkey   |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-clientid           |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-secret             |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-eventdefinitionkey |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|Loyalty-Azure-DB-Conn-String                          |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|ocp-apim-subscription-key                             |Copied from `DigitalMktg-Vault-QA` by Terraform during provisioning |
|redis-sfmc-connection-string-primary                  |Created by Terraform during provisioning |
|redis-omnichannel-connection-string-primary           |Created by Terraform during provisioning |

## Secrets PROD

The secrets required by the application in the PROD environment are stored in the `kv-mbosfmcapi-seg-prod` key vault and listed below.

|Name |Source |
|:----|:------|
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-eagleeye-clientid            |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-clientid              |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-secret                |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-fresco-eventdefinitionkey    |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-clientid             |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-secret               |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-harveys-eventdefinitionkey   |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-clientid           |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-secret             |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|azurembonotificationsfmc-winndixie-eventdefinitionkey |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|Loyalty-Azure-DB-Conn-String                          |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|ocp-apim-subscription-key                             |Copied from `DigitalMktg-Vault-PROD` by Terraform during provisioning |
|redis-sfmc-connection-string-primary                  |Created by Terraform during provisioning |
|redis-omnichannel-connection-string-primary           |Created by Terraform during provisioning |

## Depreciated Key Vault Resources

Key vault resources like keys, secrets, certificates and access policies for the `MBO Notifications SFMC` application that are no longer used and can be scheduled for removal.

### Depreciated Key Vault Resources DEV

|Type |Name |Source |Note |
|:----|:----|:------|:----|
|Secret |eagle-eye-client-id |kv-omnichannel-seg-dev ||
|Secret |eagle-eye-secret |kv-omnichannel-seg-dev ||
|Secret |fresco-client-id |kv-omnichannel-seg-dev ||
|Secret |fresco-client-secret |kv-omnichannel-seg-dev ||
|Secret |fresco-event-definition-key |kv-omnichannel-seg-dev ||
|Secret |harveys-client-id |kv-omnichannel-seg-dev ||
|Secret |harveys-client-secret |kv-omnichannel-seg-dev ||
|Secret |harveys-event-definition-key |kv-omnichannel-seg-dev ||
|Secret |winndixie-client-id |kv-omnichannel-seg-dev ||
|Secret |winn-dixie-client-secret |kv-omnichannel-seg-dev ||
|Secret |wd-event-definition-key |kv-omnichannel-seg-dev ||

### Depreciated Key Vault Resources QA

|Type |Name |Source |Note |
|:----|:----|:------|:----|
|Secret |eagle-eye-client-id |kv-omnichannel-seg-qa ||
|Secret |eagle-eye-secret |kv-omnichannel-seg-qa ||
|Secret |fresco-client-id |kv-omnichannel-seg-qa ||
|Secret |fresco-client-secret |kv-omnichannel-seg-qa ||
|Secret |fresco-event-definition-key |kv-omnichannel-seg-qa ||
|Secret |harveys-client-id |kv-omnichannel-seg-qa ||
|Secret |harveys-client-secret |kv-omnichannel-seg-qa ||
|Secret |harveys-event-definition-key |kv-omnichannel-seg-qa ||
|Secret |winndixie-client-id |kv-omnichannel-seg-qa ||
|Secret |winn-dixie-client-secret |kv-omnichannel-seg-qa ||
|Secret |wd-event-definition-key |kv-omnichannel-seg-qa ||

### Depreciated Key Vault Resources PROD

|Type |Name |Source |Note |
|:----|:----|:------|:----|
|Secret |eagle-eye-client-id |kv-omnichannel-seg-prod ||
|Secret |eagle-eye-secret |kv-omnichannel-seg-prod ||
|Secret |fresco-client-id |kv-omnichannel-seg-prod ||
|Secret |fresco-client-secret |kv-omnichannel-seg-prod ||
|Secret |fresco-event-definition-key |kv-omnichannel-seg-prod ||
|Secret |harveys-client-id |kv-omnichannel-seg-prod ||
|Secret |harveys-client-secret |kv-omnichannel-seg-prod ||
|Secret |harveys-event-definition-key |kv-omnichannel-seg-prod ||
|Secret |winndixie-client-id |kv-omnichannel-seg-prod ||
|Secret |winn-dixie-client-secret |kv-omnichannel-seg-prod ||
|Secret |wd-event-definition-key |kv-omnichannel-seg-prod ||
