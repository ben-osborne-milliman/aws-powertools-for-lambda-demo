
resource "aws_iam_policy" "secretsmanager_policy" {
  name        = "${local.resource_prefix}-secretsmanager-policy"
  description = "Policy to allow the lambda to access Secrets Manager"
  policy      = data.aws_iam_policy_document.secretsmanager_policy.json
}

resource "aws_iam_policy" "dynamodb_policy" {
  name        = "${local.resource_prefix}-dynamodb-policy"
  description = "Policy to allow the lambda to access DynamoDB"
  policy      = data.aws_iam_policy_document.dynamodb_policy.json
}
