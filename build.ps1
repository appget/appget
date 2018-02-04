$msBuild = 'C:\Program Files\MSBuild\14.0\Bin\v4.0.30319\msbuild.exe'

Function Build()
{
    Write-Host "##teamcity[progressStart 'Build']"

    $clean = $msbuild + " src\AppGet.sln /t:Clean /m"
    $build = $msbuild + " src\AppGet.sln /p:Configuration=Release /t:Build /m"

    Invoke-Expression $clean
    CheckExitCode

    Invoke-Expression $build
    CheckExitCode


    Write-Host "##teamcity[progressFinish 'Build']"
}


Function CheckExitCode()
{
        if ($lastexitcode -ne 0)
        {
            Write-Host $errorMessage
            exit 1
        }
}


Build
