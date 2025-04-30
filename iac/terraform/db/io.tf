#####LOCALS#####
locals {
  application      = "demo"
  account          = "Development Equifax"
  environment      = "dev"
  line_of_business = "int"
  dns_zone_name    = "${local.environment}-equifax.acs.millimanintelliscript.com"
  db_password      = var.db_password == "" ? random_password.password.result : var.db_password
}

#####VARIABLES#####
variable "db_username" {
  type        = string
  description = "The username for the database"
  default     = "master"
}

variable "db_password" {
  type        = string
  description = "The password for the database"
  sensitive   = true
  default     = ""
}

#####OUTPUTS#####
output "database_port" {
  value = module.aurora_cluster.database_port
}

output "rds_cluster_identifier" {
  value = module.aurora_cluster.rds_cluster_identifier
}

output "database_user" {
  value = module.aurora_cluster.database_user
}

output "security_group_id" {
  value = module.aurora_cluster.security_group_id
}

output "rds_cluster_address" {
  value = module.aurora_cluster.rds_cluster_address
}

output "cluster_write_fqdn" {
  value = module.aurora_cluster.cluster_write_fqdn
}

output "cluster_read_fqdn" {
  value = module.aurora_cluster.cluster_read_fqdn
}
