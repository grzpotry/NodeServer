protoc --plugin="/protoc-gen-ts=protoc-gen-ts" --js_out="import_style=commonjs,binary:generated,error_on_name_conflict" --ts_out=generated CommunicationProtocol.proto