::generates c# code for structures defined in source .proto file
::doc https://developers.google.com/protocol-buffers/docs/csharptutorial

@echo off
IF [%1]==[] (
echo Missing argument with source .proto filename
exit /b
) 

set SOURCE_PROTO_FILE_NAME=%1
set SOURCE_PROTO_FULL_FILE_NAME=%SOURCE_PROTO_FILE_NAME%.proto

IF NOT EXIST %SOURCE_PROTO_FULL_FILE_NAME% (
echo File %SOURCE_PROTO_FULL_FILE_NAME% not exist. Pass valid .proto file
exit /b
)

set TARGET_PATH=../../client/Assets/scripts/Networking/generated/%SOURCE_PROTO_FILE_NAME%
protoc --csharp_out=%TARGET_PATH% %SOURCE_PROTO_FILE_NAME%.proto