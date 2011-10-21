param(
  [string]$source,
  [string]$target
)

$ErrorActionPreference = "stop"

Write-Host
Write-Host "Backing up application ..."
Write-Host

robocopy $source $target /E

exit $LASTEXITCODE