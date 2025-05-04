

module "base_tags" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/base-tags?ref=v1"

  application      = local.application
  account          = local.account
  environment      = local.environment
  line_of_business = local.line_of_business
  lifespan         = "temporary"
  owner_email      = "test.user@milliman.com"
  map_migrated     = "exclude"
}

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
  timeout               = 30
  build_output_path     = "../../../../src/lambdas/PwrTlzDemo/PwrTlzDemo/bin/Release/net8.0"
  security_group_ids    = [data.aws_security_group.default_security_groups.id]
  subnet_ids            = data.aws_subnets.private_subnets.ids
  enable_tracing        = true
  log_format            = "JSON"
  application_log_level = "INFO"
  event_log_level       = "INFO"
  environment_variables = [
    {
      name  = "POWERTOOLS_SERVICE_NAME"
      value = local.role
    },
    {
      name  = "POWERTOOLS_TRACER_CAPTURE_RESPONSE",
      value = "false"
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
