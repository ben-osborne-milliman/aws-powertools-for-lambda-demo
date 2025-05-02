
// NOTE: Only using this for demo purposes. This is not a good practice for production code.
resource "null_resource" "build" {

  triggers = {
    timestamp = timestamp()
  }

  provisioner "local-exec" {
    command = "dotnet build ../../../../src/lambdas/PwrTlzDemo/PwrTlzDemo/ -c Release"
  }
}
