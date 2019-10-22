::generates c# code for structures defined in source .proto file
::doc https://developers.google.com/protocol-buffers/docs/csharptutorial

@echo off
IF [%1]==[] (
echo Missing argument with source .proto filename
exit /b 1
) 

IF [%2]==[] (
echo Missing argument with target project dir
exit /b 1
) 

set SOURCE_PROTO_FILE_NAME=%1
set PROJECT_DIR=%2

set SOURCE_PROTO_FULL_FILE_NAME=%SOURCE_PROTO_FILE_NAME%.proto

IF NOT EXIST %SOURCE_PROTO_FULL_FILE_NAME% (
echo File %SOURCE_PROTO_FULL_FILE_NAME% does not exist. Pass valid .proto file
exit /b 1
)

IF NOT EXIST %PROJECT_DIR% (
echo Directory %PROJECT_DIR% does not exist. Pass valid csharp project root directory.
exit /b 1
)

set TARGET_PATH=%PROJECT_DIR%/%SOURCE_PROTO_FILE_NAME%

::create target dir if not exists
mkdir "%TARGET_PATH%"

::generate csharp files
protoc --csharp_out=%TARGET_PATH% %SOURCE_PROTO_FILE_NAME%.proto

::exit success
exit /b 0