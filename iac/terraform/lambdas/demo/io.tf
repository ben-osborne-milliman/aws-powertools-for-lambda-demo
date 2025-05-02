locals {
  application      = "demo"
  account          = "Development Equifax"
  environment      = "dev"
  line_of_business = "int"
  role             = "pwrtlz-demo"
  resource_prefix  = "${local.line_of_business}-${local.application}-${local.environment}"
}
