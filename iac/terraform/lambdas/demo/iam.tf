
resource "aws_iam_policy" "rds_policy" {
  name        = "${local.resource_prefix}-rds-policy"
  description = "Policy to allow the lambda to generate RDS authentication tokens"
  policy      = data.aws_iam_policy_document.rds_auth_token_policy.json
}

resource "aws_iam_role_policy_attachment" "rds_policy_attachment" {
  role       = module.lambda.lambda_iam_role_name
  policy_arn = aws_iam_policy.rds_policy.arn
}

resource "aws_iam_policy" "route53_policy" {
  name        = "${local.resource_prefix}-route53-policy"
  description = "Policy to allow the lambda to list Route53 hosted zones and resource record sets"
  policy      = data.aws_iam_policy_document.route53_policy.json
}

resource "aws_iam_role_policy_attachment" "route53_policy_attachment" {
  role       = module.lambda.lambda_iam_role_name
  policy_arn = aws_iam_policy.route53_policy.arn
}
