param([string]$packageId)

$version = get-childitem .\..\packages `
| Select-Object @{n="Name"; e={$_.Name}}, @{n="Sort"; e={ $_.Name | select-string "$packageId.(\d.*)" | % { $_.Matches } | % { $_.Groups[1] } | % { $_.Value -replace "\.", "" } | % { $_ -as [int32] } } } `
| Where-Object { $_.Sort } `
| Sort-Object Sort -Descending `
| select -First 1 `
| select Name

$script = ".\..\packages\" + $version.Name + "\Tools\bootstrap.ps1"
$packagePath = Resolve-Path $(".\..\packages\" + $version.Name)
& $script -packagePath $packagePath