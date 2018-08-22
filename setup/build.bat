REM SET APPVEYOR_BUILD_VERSION=1.0.0

del src\AppGet\bin\x86\Release\*.xml
setup\inno\ISCC.exe setup\appget.iss

