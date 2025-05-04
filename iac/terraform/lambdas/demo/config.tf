provider "aws" {
  region = "us-east-1"
  default_tags {
    tags = module.base_tags.tags
  }
}

terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
    }
  }

  backend "s3" {
    region       = "us-east-1"
    use_lockfile = true
    bucket       = "mi-tfstate-int-equifax-dev"
    key          = "demo7/lambdas/pwrtlz-demo"
  }
}
