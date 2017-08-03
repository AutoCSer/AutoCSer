echo off
cls
bcdedit /set hypervisorlaunchtype auto
echo ...
if %errorlevel% equ 0 (echo 启用 Hyper-V 需要重启操作系统才能生效) else (echo 可能需要使用管理员身份运行)
pause