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
  depends_on                 = [module.demo-2-log-lambda]
}




/*
module "lambda" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/lambda//net?ref=v6"

  application           = local.application
  environment           = local.environment
  line_of_business      = local.line_of_business
  role                  = local.role
  architecture          = "arm64"
  handler_function      = "PwrTlzDemo::PwrTlzDemo.Function::FunctionHandler"
  runtime               = "dotnet8"
  description           = "PowerTools Demo Function"
  memory_size           = 128
  timeout               = 60
  build_output_path     = "../../../../src/lambdas/PwrTlzDemo/PwrTlzDemo/bin/Release/net8.0"
  security_group_ids    = [data.aws_security_group.default_security_groups.id]
  subnet_ids            = data.aws_subnets.private_subnets.ids
  enable_tracing        = true
  log_format            = "JSON"
  application_log_level = "INFO"
  event_log_level       = "INFO"
  environment_variables = [
    {
      name = "IDEMPOTENCY_TABLE",
      value = aws_dynamodb_table.idempotency_table.name
    },
    {
      name  = "POWERTOOLS_TRACER_CAPTURE_RESPONSE",
      value = "false"
    },
    {
      name = "POWERTOOLS_LOGGER_LOG_LEVEL",
      value = "INFO"
    },
    {
      name  = "DB_CREDENTIALS_SECRET_NAME",
      value = "int-demo-dev-secret"
    },
    {
      name  = "DB_HOST",
      value = "int-demo-dev-db.dev-equifax.acs.millimanintelliscript.com"
    },
    {
      name  = "DB_PORT",
      value = "5432"
    },
    {
      name  = "DB_NAME",
      value = "demo"
    },
    {
      name  = "DB_REQUIRE_SSL",
      value = "true"
    }
  ]
  depends_on = [null_resource.build]
}
*/
