Function Find-MsBuild([int] $MaxVersion = 2017)
{
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319"
		
    If ((2017 -le $MaxVersion) -And (Test-Path $agentPath)) { return $agentPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $MaxVersion) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $MaxVersion) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
        
    throw "Yikes - Unable to find msbuild"
}

Write-Host "########" 

$projectName = "tesseract.net"
$GitExe = "C:\Program Files\Git\bin\git.exe"
$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" 
$localDirectory = "C:\csource\"
$GitHttpsUrl = "https://github.com/tvn-cosine/tesseract.net"
$MSBuild = Find-MsBuild
 
Write-Host "########"
Write-Host "Creating directory $localDirectory ..."
Write-Host "########"

if (Test-Path "$localDirectory\\$projectName\\")
{
    Remove-Item "$localDirectory\$projectName\" -Force -Recurse
    Write-Host "Done removing directory..."
} 

if (-not (Test-Path "$localDirectory\"))
{
    New-Item "$localDirectory\" -ItemType Directory
    Write-Host "Done creating directory..."
}
Write-Host "########"
Write-Host " "
Write-Host " "
 
Write-Host "########"
Write-Host "Changine to directory $localDirectory..." 
Set-Location -Path $localDirectory
Write-Host "Changed to directory..." 
Write-Host "########"
Write-Host " "
Write-Host " "

 

Write-Host "########"
Write-Host "Cloning from git $localDirectory."
$localDirectory = "C:\csource\$projectName\"

Try
{
    $erroractionpreference = "Stop"
    & $GitExe clone "$GitHttpsUrl" 
} Catch
{
    $ErrorMessage = $_.Exception.Message
    Write-Host "$ErrorMessage..." 
}
    $erroractionpreference = "Stop"

Write-Host "Cloned into $localDirectory ..." 
Write-Host "########"
Write-Host " "
Write-Host " "

Write-Host "########"
Write-Host "Downloading nuget into $localDirectory."
$targetNugetExe = "$localDirectory\nuget.exe"
if (-Not (Test-Path $targetNugetExe))
{
    Invoke-WebRequest $sourceNugetExe -OutFile "C:\csource\nuget.exe" 
}
Write-Host "Downloaded nuget..." 
Write-Host "########"
Write-Host " "
Write-Host " "
 
 
Write-Host "########"
Write-Host "Packing nuget into $localDirectory." 
Write-Host "Packing $localDirectory\$projectName\$projectName.csproj"
& "C:\csource\nuget.exe" restore "$localDirectory$projectName.sln"
& $MSBuild "$localDirectory$projectName\$projectName.csproj" /p:Configuration=net452;
& $MSBuild "$localDirectory$projectName\$projectName.csproj" /p:Configuration=net462;
& $MSBuild "$localDirectory$projectName\$projectName.csproj" /p:Configuration=net471;
& "C:\csource\nuget.exe" pack "$localDirectory\$projectName\$projectName.csproj" -build
Write-Host "Package created..." 
Write-Host "########"
Write-Host " "
Write-Host " " 
Write-Host "Done..."
