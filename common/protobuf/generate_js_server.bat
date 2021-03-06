::generates .js (code) and .d.ts (typescript type definitions) from source .proto file 
::doc https://developers.google.com/protocol-buffers/docs/reference/javascript-generated
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

IF NOT EXIST %SOURCE_PROTO_FILE_NAME%.proto (
echo File %SOURCE_PROTO_FILE_NAME%.proto not exist. Pass valid .proto file
exit /b 1
)

IF NOT EXIST %PROJECT_DIR% (
echo Directory %PROJECT_DIR% does not exist. Pass valid javascript project root directory.
exit /b 1
)

set TARGET_DIR_NAME=generated
set TARGET_PATH=%PROJECT_DIR%/src/%TARGET_DIR_NAME%

::create target dir if not exists
mkdir "%TARGET_PATH%"

set PROTOC_GEN_TS_PATH="../../server/node_modules/.bin/protoc-gen-ts"

::export .js and .d.ts with generated classes and type definitions from .proto file
protoc --plugin="/protoc-gen-ts=${PROTOC_GEN_TS_PATH}" --js_out="import_style=commonjs,binary:%TARGET_PATH%" --ts_out=%TARGET_PATH% %SOURCE_PROTO_FILE_NAME%.proto

cd %PROJECT_DIR%
::copy generated files to dist so modules can be loaded
robocopy src/%TARGET_DIR_NAME% dist/src/%TARGET_DIR_NAME% /E 

::exit success
exit /b 0

