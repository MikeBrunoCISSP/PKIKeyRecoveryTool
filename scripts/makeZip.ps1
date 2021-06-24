Push-Location $PSScriptRoot
$ZipPath = "..\zip\KRTool.zip"

if (Test-Path $ZipPath)
{
	Remove-Item -Path $ZipPath
}

$compress = @{
  Path = "..\bin\*"
  CompressionLevel = "Fastest"
  DestinationPath = $ZipPath
}
Compress-Archive @compress