@echo off
REM Docker Manager for Windows - Local Testing
REM Usage: docker-manager.bat

setlocal enabledelayedexpansion

set PROJECT_DIR=%~dp0
set BACKUP_DIR=%PROJECT_DIR%backups

:menu
cls
echo ========================================
echo    Pickleball Club Docker Manager
echo ========================================
echo.
echo 1.  Show Status
echo 2.  Start Services
echo 3.  Stop Services
echo 4.  Restart Services
echo 5.  Restart API Only
echo 6.  Restart Database Only
echo 7.  View API Logs
echo 8.  View Database Logs
echo 9.  View All Logs
echo 10. Run Database Backup
echo 11. Database Shell
echo 12. Run Migrations
echo 13. Update and Rebuild
echo 14. Clean Up System
echo 15. Check Disk Space
echo 16. Health Check
echo 17. View Configuration
echo 0.  Exit
echo.
set /p choice=Select option: 

if "%choice%"=="1" goto status
if "%choice%"=="2" goto start
if "%choice%"=="3" goto stop
if "%choice%"=="4" goto restart
if "%choice%"=="5" goto restart_api
if "%choice%"=="6" goto restart_db
if "%choice%"=="7" goto api_logs
if "%choice%"=="8" goto db_logs
if "%choice%"=="9" goto all_logs
if "%choice%"=="10" goto backup
if "%choice%"=="11" goto db_shell
if "%choice%"=="12" goto migrate
if "%choice%"=="13" goto update
if "%choice%"=="14" goto cleanup
if "%choice%"=="15" goto disk
if "%choice%"=="16" goto health
if "%choice%"=="17" goto config
if "%choice%"=="0" exit /b 0

goto menu

:status
echo.
echo Container Status:
docker-compose ps
timeout /t 3 /nobreak
goto menu

:start
echo.
echo Starting services...
cd /d "%PROJECT_DIR%"
docker-compose up -d
timeout /t 3 /nobreak
goto status

:stop
echo.
echo Stopping services...
cd /d "%PROJECT_DIR%"
docker-compose down
timeout /t 2 /nobreak
echo Services stopped!
timeout /t 2 /nobreak
goto menu

:restart
echo.
echo Restarting services...
cd /d "%PROJECT_DIR%"
docker-compose restart
timeout /t 3 /nobreak
goto status

:restart_api
echo.
echo Restarting API...
cd /d "%PROJECT_DIR%"
docker-compose restart api
timeout /t 2 /nobreak
goto status

:restart_db
echo.
echo Restarting Database...
cd /d "%PROJECT_DIR%"
docker-compose restart sqlserver
timeout /t 3 /nobreak
goto status

:api_logs
echo.
echo API Logs (Ctrl+C to exit):
cd /d "%PROJECT_DIR%"
docker-compose logs -f api
goto menu

:db_logs
echo.
echo Database Logs (Ctrl+C to exit):
cd /d "%PROJECT_DIR%"
docker-compose logs -f sqlserver
goto menu

:all_logs
echo.
echo All Logs (Ctrl+C to exit):
cd /d "%PROJECT_DIR%"
docker-compose logs -f
goto menu

:backup
echo.
echo Starting database backup...
cd /d "%PROJECT_DIR%"

for /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
for /f "tokens=1-2 delims=/:" %%a in ('time /t') do (set mytime=%%a%%b)

set backup_file=%BACKUP_DIR%\db_backup_%mydate%_%mytime%.bak

REM Create backup directory if not exists
if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"

docker-compose exec -T sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Hoang416@" -Q "BACKUP DATABASE PickleBallDB_Hoang248 TO DISK = '/var/opt/mssql/backup/db_backup_%mydate%_%mytime%.bak'"

docker cp pickleball_sqlserver:/var/opt/mssql/backup/db_backup_%mydate%_%mytime%.bak "%backup_file%" 2>nul

if exist "%backup_file%" (
    echo Backup completed: %backup_file%
) else (
    echo Backup failed!
)
timeout /t 3 /nobreak
goto menu

:db_shell
echo.
echo Connecting to database...
cd /d "%PROJECT_DIR%"
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Hoang416@"
goto menu

:migrate
echo.
echo Running Entity Framework migrations...
cd /d "%PROJECT_DIR%"
docker-compose exec api dotnet ef database update
echo Migrations completed!
timeout /t 3 /nobreak
goto menu

:update
echo.
echo Pulling latest code from Git...
cd /d "%PROJECT_DIR%"
git pull origin main

echo Rebuilding containers...
docker-compose up -d --build

timeout /t 3 /nobreak
goto status

:cleanup
echo.
echo Cleaning up Docker system...
docker system prune -a -f

echo Cleanup completed!
timeout /t 3 /nobreak
goto menu

:disk
echo.
echo Disk Usage:
cd /d "%PROJECT_DIR%"
echo.
echo Docker Images and Volumes:
docker system df
echo.
echo Project Directory Size:
for /f %%A in ('dir "%PROJECT_DIR%" /s /b ^| find /c /v ""') do echo %%A files
timeout /t 5 /nobreak
goto menu

:health
echo.
echo Performing health check...
echo.
echo Container Status:
docker-compose ps
echo.
echo API Health Endpoint:
powershell -NoExit -Command "try {$response = Invoke-RestMethod -Uri 'http://localhost:5003/health' -ErrorAction Stop; Write-Host 'API Status: Healthy' -ForegroundColor Green; $response | ConvertTo-Json} catch {Write-Host 'API not responding' -ForegroundColor Red}"
timeout /t 5 /nobreak
goto menu

:config
echo.
echo Current Configuration:
cd /d "%PROJECT_DIR%"
docker-compose config
pause
goto menu
