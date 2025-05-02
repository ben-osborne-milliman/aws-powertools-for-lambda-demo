

module "base_tags" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/base-tags?ref=v1"

  application      = local.application
  account          = local.account
  environment      = local.environment
  line_of_business = local.line_of_business
  lifespan         = "temporary"
  owner_email      = "ben.osborne@milliman.com"
  map_migrated     = "exclude"
}

module "ip_address_ranges" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/known-ip-address-ranges?ref=v2"
  environment      = local.environment
}

data "aws_vpc" "vpc" {
  tags = {
    Name = "dev-int-vpc"
  }
}

# Used if a password is not provided in the apply.
resource "random_password" "password" {
  length           = 16
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

module "aurora_cluster" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/aurora-cluster?ref=v1"

  line_of_business                    = local.line_of_business
  application                         = local.application
  environment                         = local.environment
  role                                = "db"
  rds_engine_version                  = "16.3"
  auto_minor_version_upgrade          = true
  allow_major_version_upgrade         = true
  database_name                       = "demo"
  database_user                       = var.db_username
  database_password                   = local.db_password
  database_port                       = 5432
  vpc_id                              = data.aws_vpc.vpc.id
  skip_final_snapshot                 = true
  copy_tags_to_snapshot               = true
  maintenance_window                  = "Thu:05:00-Thu:10:00"
  backup_window                       = "10:15-11:15"
  backup_retention_period             = 1
  kms_key_alias                       = "aws/rds"
  iam_database_authentication_enabled = true
  apply_instance_changes_immediately  = true
  apply_cluster_changes_immediately   = false
  cluster_parameter_group_name        = ""
  instance_parameter_group_name       = ""
  instance_count                      = 1
  availability_zones                  = ["us-east-1a", "us-east-1b", "us-east-1c"]
  storage_encrypted                   = true
  performance_insights_enabled        = false
  serverless_auto_pause               = true
  serverless_seconds_until_auto_pause = 300
  serverless_max_capacity             = 4
  serverless_min_capacity             = 0
  deletion_protection                 = false
  enabled_cloudwatch_logs_exports     = [ ]
  dns_zone_name                       = local.dns_zone_name
  allowed_access_cidrs                = module.ip_address_ranges.intelliscript_data_center_ip_ranges
}