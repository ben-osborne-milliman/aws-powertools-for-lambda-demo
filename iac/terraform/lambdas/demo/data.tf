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

data "aws_secretsmanager_secret" "demo_secret" {
  name = "int-demo-dev-secret"
}

data "aws_iam_policy_document" "secretsmanager_policy" {
  statement {
    actions = [
      "secretsmanager:GetSecretValue",
      "secretsmanager:DescribeSecret"
    ]
    resources = [
      data.aws_secretsmanager_secret.demo_secret.arn
    ]
  }
}

data "aws_iam_policy_document" "dynamodb_policy" {
  statement {
    actions = [
      "dynamodb:PutItem",
      "dynamodb:GetItem",
      "dynamodb:UpdateItem",
      "dynamodb:DeleteItem",
      "dynamodb:Scan",
      "dynamodb:Query"
    ]
    resources = [
      aws_dynamodb_table.idempotency_table.arn,
      "${aws_dynamodb_table.idempotency_table.arn}/*"
    ]
  }
}

data "aws_iam_policy_document" "sqs_policy" {
  statement {
    actions = [
      "sqs:ReceiveMessage",
      "sqs:DeleteMessage",
      "sqs:GetQueueAttributes",
      "sqs:ChangeMessageVisibility"
    ]
    resources = [
      aws_sqs_queue.fifo_queue.arn
    ]
  }
}