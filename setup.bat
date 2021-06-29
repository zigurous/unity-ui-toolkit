powershell -Command "(gc  %~dp0\Editor\Zigurous.Template.Editor.asmdef) -replace 'Zigurous.Template.Editor', 'Zigurous.%1.Editor' | Out-File -encoding ASCII  %~dp0\Editor\Zigurous.Template.Editor.asmdef"
powershell -Command "(gc  %~dp0\Runtime\Zigurous.Template.asmdef) -replace 'Zigurous.Template', 'Zigurous.%1' | Out-File -encoding ASCII  %~dp0\Runtime\Zigurous.Template.asmdef"
powershell -Command "(gc  %~dp0\Tests\Editor\Zigurous.Template.Editor.Tests.asmdef) -replace 'Zigurous.Template.Editor.Tests', 'Zigurous.%1.Editor.Tests' | Out-File -encoding ASCII  %~dp0\Tests\Editor\Zigurous.Template.Editor.Tests.asmdef"
powershell -Command "(gc  %~dp0\Tests\Runtime\Zigurous.Template.Tests.asmdef) -replace 'Zigurous.Template.Tests', 'Zigurous.%1.Tests' | Out-File -encoding ASCII  %~dp0\Tests\Runtime\Zigurous.Template.Tests.asmdef"

@REM powershell -Command "(gc  %~dp0\package.json) -replace 'com.zigurous.template', 'com.zigurous.%2' | Out-File -encoding ASCII  %~dp0\package.json"
@REM powershell -Command "(gc  %~dp0\package.json) -replace 'Zigurous Template', 'Zigurous %3' | Out-File -encoding ASCII  %~dp0\package.json"
@REM powershell -Command "(gc  %~dp0\README.md) -replace 'unity-package-template', '%4' | Out-File -encoding ASCII  %~dp0\README.md"

ren %~dp0\Editor\Zigurous.Template.Editor.asmdef Zigurous.%1.Editor.asmdef
ren %~dp0\Runtime\Zigurous.Template.asmdef Zigurous.%1.asmdef
ren %~dp0\Tests\Editor\Zigurous.Template.Editor.Tests.asmdef Zigurous.%1.Editor.Tests.asmdef
ren %~dp0\Tests\Runtime\Zigurous.Template.Tests.asmdef Zigurous.%1.Tests.asmdef

