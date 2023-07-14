app_service_kind            = "elastic"
app_service_tier            = "ElasticPremium"
app_service_sku             = "EP1"
function_apps_warm_instance_count = 1
max_instance_count                = 10
plan_name                   = "asp-ElasticPremium-apps"
runtime_scale_monitoring_enabled  = true
cosmos_db_rg                = "rg-omnichannel-core-qa"
omnichannel_kv_rg           = "rg-omnichannel-security-qa"
store_locator_address_db_rg = "blhtestmydigitalstorewebapi"
client_id_ee                = "loexzymj7g3ncjtvl4rq"
secret_ee                   = "wcxp8l9aof43gq39ro94uitcb6wnkh"
base_url_ee                 = "https://qa-api.segrocers.com/EE"
base_url_campaigns_ee       = "https://qa-api.segrocers.com/EagleEyeCampaignsAPI"
bilo_partner_code           = "43454"
cache_server                = "redis-omnichannel-seg-qa.redis.cache.windows.net:6380"
fresco_partner_code         = "43456"
harveys_partner_code        = "43453"
wd_partner_code             = "43455"
redis_connection_timeout   = ",connectTimeout=15000"
ee_healthcheck_memberid    = "SEG0000000000184" #Random QA member id
digitalmktg_vault_name                = "DigitalMktg-Vault-QA"
digitalmktg_vault_resource_group_name = "ActiveDirectory-DEV"