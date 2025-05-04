
resource "aws_iam_policy" "secretsmanager_policy" {
  name        = "${local.resource_prefix}-secretsmanager-policy"
  description = "Policy to allow the lambda to access Secrets Manager"
  policy      = data.aws_iam_policy_document.secretsmanager_policy.json
}

resource "aws_iam_role_policy_attachment" "secretsmanager_policy_attachment" {
  role       = module.lambda.lambda_iam_role_name
  policy_arn = aws_iam_policy.secretsmanager_policy.arn
}