<#
.SYNOPSIS
testing writing dos path to json file
 #>

 [cmdletbinding()]
 param()

 $obj = @{thePath=$PSScriptRoot}

 $obj | Convertto-json | Out-File -FilePath ".\test-json.json"

#  $testFile = 