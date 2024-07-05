param([string]$configuration = "Release")

dotnet build ./src/skiasharp/LiveChartsCore.SkiaSharp.Wpf/LiveChartsCore.SkiaSharpView.Wpf.csproj -c $configuration

$msbuild = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe

& $msbuild `
    ./src/skiasharp/LiveChartsCore.SkiaSharpView.WinUI/LiveChartsCore.SkiaSharpView.WinUI.csproj `
    /p:configuration=$configuration `
    /restore

