resource "aws_sqs_queue" "fifo_queue" {
  name                       = "${local.resource_prefix}-sqs-queue.fifo"
  fifo_queue                 = true
  visibility_timeout_seconds = 60
}
