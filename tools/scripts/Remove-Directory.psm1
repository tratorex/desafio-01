function Remove-Directory {
    Param([Parameter(Mandatory=$true)][string]$dir)
    $verbose = $PSBoundParameters['Verbose']
    Write-Host "Removing '$dir'" -ForegroundColor Blue
    if ((Test-Path $dir) -ne $true) {
        Write-Host "$dir does not exist, exiting" -ForegroundColor Red
        return
    }
    if ($verbose) {
        Write-Host "$dir exists" -ForegroundColor Blue
    }
    rm "$dir" -Recurse -Force -ErrorAction SilentlyContinue -ErrorVariable errs | out-null
    if ($errs -ne $null) {
        if ($verbose) {
            Write-Host -ForegroundColor Red "Got errors: $errs"
        }
        Foreach ($err in $errs){
            if ($($err.CategoryInfo.Reason) -eq "PathTooLongException") {
                $drive = ls function:[f-z]: -n | ?{ !(test-path $_) } | random
                if ($verbose) {
                    Write-Host -ForegroundColor Red "Running 'subst $drive $dir'"
                }
                subst $drive $dir
                $children = ls $dir
                foreach($child in $children) {
                    $dirName = $child.Name
                    $verboseParam = ""
                    if ($verbose) {
                        Write-Host -ForegroundColor Green "force removing ${drive}\$($child.Name)"
                        $verboseParam = "-verbose:`$true"
                    }
                    Start-Process powershell "-NoProfile -ExecutionPolicy unrestricted -Command `"Import-Module $PSScriptRoot\Remove-Directory.psm1;Remove-Directory `"${drive}\$($child.Name)`" $verboseParam`" " -Wait -NoNewWindow
                }
                subst /d $drive
                break
            }
        }
    }
    rm "$dir" -Recurse -Force -ErrorAction SilentlyContinue -ErrorVariable errs | out-null
}

Export-ModuleMember -Function Remove-Directory