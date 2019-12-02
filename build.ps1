Import-Module $PSScriptRoot\tools\psake\psake.psm1 -force
if ($MyInvocation.UnboundArguments.Count -ne 0) {
    Invoke-Expression("Invoke-psake -framework '4.5.1' default.ps1 " + $MyInvocation.UnboundArguments -join " ")
}
else {
    . $PSScriptRoot\build.ps1 -taskList Full-Build
}