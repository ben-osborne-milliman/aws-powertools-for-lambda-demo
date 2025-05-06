############
## Locals ##
############

locals {
  role         = "pwrtlz-demo-5-idem"
  project_path = "../../../../src/lambdas/PwrTlzDemo/PwrTlzDemo.5.Idem"
}

###############
## Inputs ####
###############

variable "environment" {
  type = string
}

variable "line_of_business" {
  type = string
}

variable "application" {
  type = string
}

variable "security_group_ids" {
  type = list(string)
}

variable "subnet_ids" {
  type = list(string)
}

variable "secrets_manager_policy_arn" {
  type = string
}

variable "idempotency_table_name" {
  type = string
}

variable "dynamodb_policy_arn" {
  type = string
}

###############
## Lambda #####
###############

module "lambda" {
  source = "git::https://miazuredomke01.mi.local/IntelliScriptV2/terraform-modules/_git/lambda//net?ref=v6"

  application           = var.application
  environment           = var.environment
  line_of_business      = var.line_of_business
  role                  = local.role
  architecture          = "arm64"
  handler_function      = "PwrTlzDemo.5.Idem::PwrTlzDemo.Function::FunctionHandler"
  runtime               = "dotnet8"
  description           = "PowerTools Demo Function - Idempotency"
  memory_size           = 128
  timeout               = 60
  build_output_path     = "${local.project_path}/bin/Release/net8.0"
  security_group_ids    = var.security_group_ids
  subnet_ids            = var.subnet_ids
  enable_tracing        = true
  log_format            = "JSON"
  application_log_level = "INFO"
  event_log_level       = "INFO"
  environment_variables = [
    {
      name  = "IDEMPOTENCY_TABLE",
      value = var.idempotency_table_name
    },
    {
      name  = "POWERTOOLS_LOGGER_LOG_LEVEL",
      value = "INFO"
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

resource "null_resource" "build" {

  triggers = {
    timestamp = timestamp()
  }

  provisioner "local-exec" {
    command = "dotnet build ${local.project_path}/ -c Release"
  }
}

###############
## Resources ##
###############

resource "aws_iam_role_policy_attachment" "secretsmanager_policy_attachment" {
  role       = module.lambda.lambda_iam_role_name
  policy_arn = var.secrets_manager_policy_arn
}

resource "aws_iam_role_policy_attachment" "dynamodb_policy_attachment" {
  role       = module.lambda.lambda_iam_role_name
  policy_arn = var.dynamodb_policy_arn
}