[CmdletBinding()]
param()
<#
.SYNOPSIS
    RUN Get-PEHeader.ps1

#>

write-host "source (?) Get-PEHeader"
. $PSScriptRoot\Get-PEHeader.ps1

$TargetFile = "$env:SystemRoot\system32\ntdll.dll"
$PEHeader = Get-PEHaeder -FilePath $TargetFile
$PEHeader
