protoc --plugin="/protoc-gen-ts=protoc-gen-ts" --js_out="import_style=commonjs,binary:generated" --ts_out=generated CommunicationProtocol.proto
::documentation -> https://developers.google.com/protocol-buffers/docs/reference/javascript-generated