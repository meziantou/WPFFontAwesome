mkdir package\lib
mkdir package\lib
mkdir package\lib\net40

copy "..\Meziantou.WpfFontAwesome\bin\Release" "package\lib\net40"
copy "Meziantou.WpfFontAwesome.csproj.nuspec" "package\"
nuget pack "package\Meziantou.WpfFontAwesome.csproj.nuspec"
rmdir /S /Q package