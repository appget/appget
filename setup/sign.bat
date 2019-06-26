ECHO %BUILD_BUILDNUMBER%

setup\AzureSignTool\AzureSignTool.exe sign ^
--description-url "https://appget.net" ^
--file-digest sha512 ^
--azure-key-vault-url %AKV_URL% ^
--azure-key-vault-client-id %AKV_CLIENT_ID% ^
--azure-key-vault-client-secret %AKV_CLIENT_SECRET% ^
--azure-key-vault-certificate appget-authenticode ^
--timestamp-rfc3161 http://timestamp.digicert.com ^
--timestamp-digest sha512 ^
-v ^
setup\Output\appget.%BUILD_BUILDNUMBER%.exe
