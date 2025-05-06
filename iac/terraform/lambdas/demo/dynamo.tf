
resource "aws_dynamodb_table" "idempotency_table" {
  name         = "${local.resource_prefix}-idempotency-table"
  billing_mode = "PAY_PER_REQUEST"

  attribute {
    name = "id"
    type = "S"
  }

  hash_key = "id"

  ttl {
    attribute_name = "expiration"
    enabled        = true
  }
}
