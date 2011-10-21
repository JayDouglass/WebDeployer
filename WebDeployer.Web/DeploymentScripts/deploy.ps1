param(
  [string]$source,
  [string]$target,
  [string]$excludefiles,
  [string]$excludedirectories
)

$ErrorActionPreference = "stop"

Write-Host
Write-Host "Copying application to IIS ..."
Write-Host

$robocopyCommand = "robocopy '$source' '$target' /E"

if($excludefiles)
{
	$robocopyCommand += " /xf $excludefiles"
}

if($excludedirectories)
{
	$robocopyCommand += " /xd $excludedirectories"
}

Write-Host "$robocopyCommand"

Invoke-Expression $robocopyCommand 

exit $LASTEXITCODE