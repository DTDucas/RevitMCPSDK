$packageId = "RevitMCPSDK"
$baseVersion = "0.0.5"
$revitVersions = @("2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027")
$outputDir = ".\nupkg"
$nugetPath = "nuget"

try {
    & $nugetPath help | Out-Null
} catch {
    Write-Host "NuGet CLI not found in PATH." -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

function Build-RevitVersion {
    param (
        [string]$revitVersion
    )
    
    $configuration = "Release R$($revitVersion.Substring(2))"
    $configurationParam = """$configuration"""
    
    $targetFramework = if ($revitVersion -eq "2027") { "net10.0-windows10.0.19041.0" }
        elseif ($revitVersion -in @("2025", "2026")) { "net8.0-windows10.0.19041.0" }
        else { "net48" }
    
    dotnet restore --configuration $configurationParam
    dotnet clean --configuration $configurationParam
    dotnet build --configuration $configurationParam
    
    $binPath = "bin\$configuration\$targetFramework"
    $dllPath = "$binPath\RevitMCPSDK.dll"
    
    if (-not (Test-Path $dllPath)) {
        Write-Host "Build failed! DLL not found at: $dllPath" -ForegroundColor Red
        return
    }
    
   $packageVersion = "$revitVersion.$baseVersion"
    
    & $nugetPath pack "RevitMCPSDK.template.nuspec" -Properties "revitversion=$revitVersion;targetframework=$targetFramework;binpath=$binPath;version=$packageVersion" -OutputDirectory $outputDir -BasePath (Get-Location)
}

foreach ($revitVersion in $revitVersions) {
    Build-RevitVersion -revitVersion $revitVersion
}