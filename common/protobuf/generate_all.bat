::generate code for all shared data structures defined in .proto files
@echo off

call generate_csharp_client.bat communication_protocol ../../client/Assets/scripts/Networking/generated

IF /I "%ERRORLEVEL%" NEQ "0" (
echo Generating csharp files failed
exit /b %ERRORLEVEL%
)

call generate_js_server.bat communication_protocol ../../server

IF /I "%ERRORLEVEL%" NEQ "0" (
echo Generating javascript files failed
exit /b %ERRORLEVEL%
)

exit /b 0