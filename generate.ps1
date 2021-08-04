$ProjectPath = Join-Path $PSScriptRoot "Meziantou.WpfFontAwesome.Generator"
Push-Location $ProjectPath
dotnet run --project "$ProjectPath"
Pop-Location