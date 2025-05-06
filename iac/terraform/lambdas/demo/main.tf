locals {
  application      = "demo"
  account          = "Development Equifax"
  environment      = "dev"
  line_of_business = "int"
  role             = "pwrtlz-demo"
  resource_prefix  = "${local.line_of_business}-${local.application}-${local.environment}"
}

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

module "demo-1-prms-lambda" {
  source                     = "./demo-1-prms"
  environment                = local.environment
  line_of_business           = local.line_of_business
  application                = local.application
  security_group_ids         = [data.aws_security_group.default_security_groups.id]
  subnet_ids                 = data.aws_subnets.private_subnets.ids
  secrets_manager_policy_arn = aws_iam_policy.secretsmanager_policy.arn
}

module "demo-2-log-lambda" {
  source                     = "./demo-2-log"
  environment                = local.environment
  line_of_business           = local.line_of_business
  application                = local.application
  security_group_ids         = [data.aws_security_group.default_security_groups.id]
  subnet_ids                 = data.aws_subnets.private_subnets.ids
  secrets_manager_policy_arn = aws_iam_policy.secretsmanager_policy.arn
  depends_on                 = [module.demo-1-prms-lambda]
}

module "demo-3-trc-lambda" {
  source                     = "./demo-3-trc"
  environment                = local.environment
  line_of_business           = local.line_of_business
  application                = local.application
  security_group_ids         = [data.aws_security_group.default_security_groups.id]
  subnet_ids                 = data.aws_subnets.private_subnets.ids
  secrets_manager_policy_arn = aws_iam_policy.secretsmanager_policy.arn
  depends_on = [
    module.demo-1-prms-lambda,
    module.demo-2-log-lambda
  ]
}

module "demo-4-met-lambda" {
  source                     = "./demo-4-met"
  environment                = local.environment
  line_of_business           = local.line_of_business
  application                = local.application
  security_group_ids         = [data.aws_security_group.default_security_groups.id]
  subnet_ids                 = data.aws_subnets.private_subnets.ids
  secrets_manager_policy_arn = aws_iam_policy.secretsmanager_policy.arn
  depends_on = [
    module.demo-1-prms-lambda,
    module.demo-2-log-lambda,
    module.demo-3-trc-lambda
  ]
}

module "demo-5-idem-lambda" {
  source                     = "./demo-5-idem"
  environment                = local.environment
  line_of_business           = local.line_of_business
  application                = local.application
  security_group_ids         = [data.aws_security_group.default_security_groups.id]
  subnet_ids                 = data.aws_subnets.private_subnets.ids
  secrets_manager_policy_arn = aws_iam_policy.secretsmanager_policy.arn
  idempotency_table_name     = aws_dynamodb_table.idempotency_table.name
  dynamodb_policy_arn        = aws_iam_policy.dynamodb_policy.arn
  depends_on = [
    module.demo-1-prms-lambda,
    module.demo-2-log-lambda,
    module.demo-3-trc-lambda,
    module.demo-4-met-lambda
  ]
}
