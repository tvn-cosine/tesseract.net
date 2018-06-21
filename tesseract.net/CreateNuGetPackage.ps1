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
