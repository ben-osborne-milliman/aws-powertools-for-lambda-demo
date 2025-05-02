data "aws_vpc" "vpc" {
  filter {
    name   = "tag:Name"
    values = ["${local.environment}-${local.line_of_business}-vpc"]
  }
}

data "aws_subnets" "private_subnets" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.vpc.id]
  }

  tags = {
    Type = "private"
  }
}

data "aws_security_group" "default_security_groups" {
  vpc_id = data.aws_vpc.vpc.id

  filter {
    name   = "group-name"
    values = ["default"]
  }
}

data "aws_rds_cluster" "db_cluster" {
  cluster_identifier = "int-demo-${local.environment}-cluster"
}

data "aws_iam_policy_document" "rds_auth_token_policy" {
  statement {
    actions = [
      "rds-db:connect"
    ]
    resources = [
      "arn:aws:rds-db:*:*:dbuser:${data.aws_rds_cluster.db_cluster.cluster_resource_id}/lambda_user"
    ]
  }
}

data "aws_iam_policy_document" "route53_policy" {
  statement {
    actions = [
      "route53:ListHostedZones",
      "route53:ListResourceRecordSets"
    ]
    resources = ["*"]
  }
}
