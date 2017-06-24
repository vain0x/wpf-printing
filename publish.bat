.nuget\nuget.exe pack DotNetKit.Wpf.Printing\DotNetKit.Wpf.Printing.csproj.nuspec -Build
.nuget\nuget.exe push *.nupkg -Source https://www.nuget.org/api/v2/package
del /Q *.nupkg
