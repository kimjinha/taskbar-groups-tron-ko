@echo off
cd /d "%~dp0"
title TaskbarGroups Auto Builder

echo ============================================================
echo   TaskbarGroups Auto Builder - v2026.11.06 (kjh)
echo ============================================================
echo.

echo [1/4] Unblocking files (removing Mark of the Web)...
powershell -ExecutionPolicy Bypass -NoProfile -Command "Get-ChildItem -Path '%~dp0' -Recurse | Unblock-File" > nul 2>&1
echo Done.

set "MSBUILD="

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
    for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
        set "MSBUILD=%%i"
    )
)

if not defined MSBUILD (
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
    if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" set "MSBUILD=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
)

if not defined MSBUILD (
    echo ERROR: MSBuild was not found.
    echo Please open TaskbarGroups.sln in Visual Studio.
    goto END
)

echo [2/4] Checking NuGet packages...
if not exist "packages" (
    if not exist "nuget.exe" (
        powershell -ExecutionPolicy Bypass -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'nuget.exe' -UseBasicParsing"
    )
    if exist "nuget.exe" (
        nuget.exe restore TaskbarGroups.sln
    )
)
echo Done.

echo [3/4] Compiling Release build...
rem Remove the old exe so a failed build is not reported as success
if exist "main\bin\Release\TaskbarGroups.exe" del /q "main\bin\Release\TaskbarGroups.exe"
echo Building TaskbarGroups.sln with Platform="Any CPU"...
"%MSBUILD%" TaskbarGroups.sln /p:Configuration=Release /p:Platform="Any CPU"

if not exist "main\bin\Release\TaskbarGroups.exe" (
    echo Building project directly...
    "%MSBUILD%" main\client.csproj /p:Configuration=Release
)

echo [4/4] Building TaskbarX (taskbar style) and bundling it...
"%MSBUILD%" "TaskbarX\TaskbarX\TaskbarX.sln" /restore /p:Configuration=Release /v:minimal
"%MSBUILD%" "TaskbarX\TaskbarX Configurator\TaskbarX Configurator.sln" /restore /p:Configuration=Release /v:minimal
if exist "TaskbarX\APPX\TaskbarX\Release\TaskbarX Configurator.exe" if exist "main\bin\Release" (
    xcopy /e /i /y /q "TaskbarX\APPX\TaskbarX\Release" "main\bin\Release\TaskbarX" > nul
    echo TaskbarX copied to main\bin\Release\TaskbarX
)

if exist "main\bin\Release\TaskbarGroups.exe" (
    echo.
    echo ============================================================
    echo   SUCCESS! TaskbarGroups.exe has been created!
    echo   Location: main\bin\Release\TaskbarGroups.exe
    echo ============================================================
    explorer "main\bin\Release"
) else (
    echo.
    echo ============================================================
    echo   Build failed. Please check the error messages above.
    echo ============================================================
)

:END
echo.
echo Press any key to close this window...
pause
