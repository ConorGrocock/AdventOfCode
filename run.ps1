[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]   
    [string]$year,
    [Parameter(Mandatory = $true)]
    [string]$day,
    [Parameter(Mandatory = $true)]
    [string]$part,
    [Parameter(Mandatory = $true)]
    [string]$mode
)

Push-Location

Set-Location $year/Day$day

dotnet run --configuration Release -- $part $mode

Pop-Location

