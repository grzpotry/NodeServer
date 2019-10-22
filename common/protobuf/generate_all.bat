::generate code for all shared data structures defined in .proto files
@echo off

call generate_csharp_client.bat CommunicationProtocol ../../client/Assets/scripts/Networking/generated

IF /I "%ERRORLEVEL%" NEQ "0" (
echo Generating csharp files failed
exit /b %ERRORLEVEL%
)

call generate_js_server.bat CommunicationProtocol ../../server

IF /I "%ERRORLEVEL%" NEQ "0" (
echo Generating javascript files failed
exit /b %ERRORLEVEL%
)

exit /b 0