$config.buildFileName="deploy.ps1"
$config.framework = "4.0"
$config.taskNameFormat="Executing {0}"
$config.verboseError=$false
$config.coloredOutput = $true
$config.modules=(
  ".\_powerUp\deploy\modules\PowerUp*\*.psm1",
  ".\_powerUp\deploy\combos\PrereqCombos\*.psm1",
  ".\_powerUp\deploy\combos\WebsiteCombos\*.psm1",
  ".\_powerUp\deploy\combos\WindowsServiceCombos\*.psm1"
)
$config.moduleScope="global"