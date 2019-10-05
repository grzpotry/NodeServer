// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: CommunicationProtocol.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Networking.Protobuf.CommunicationProtocol {

  /// <summary>Holder for reflection information generated from CommunicationProtocol.proto</summary>
  public static partial class CommunicationProtocolReflection {

    #region Descriptor
    /// <summary>File descriptor for CommunicationProtocol.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommunicationProtocolReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChtDb21tdW5pY2F0aW9uUHJvdG9jb2wucHJvdG8SFUNvbW11bmljYXRpb25Q",
            "cm90b2NvbCIdCgZIZWFkZXISEwoLUGF5bG9hZFNpemUYASABKA0i1AEKCFJl",
            "c3BvbnNlEi0KBkhlYWRlchgBIAEoCzIdLkNvbW11bmljYXRpb25Qcm90b2Nv",
            "bC5IZWFkZXISNwoLUmVxdWVzdENvZGUYAiABKA4yIi5Db21tdW5pY2F0aW9u",
            "UHJvdG9jb2wuUmVxdWVzdENvZGUSOQoMUmVzcG9uc2VDb2RlGAMgASgOMiMu",
            "Q29tbXVuaWNhdGlvblByb3RvY29sLlJlc3BvbnNlQ29kZRIUCgxFcnJvck1l",
            "c3NhZ2UYBCABKAkSDwoHUGF5bG9hZBgFIAEoDCKCAQoHUmVxdWVzdBItCgZI",
            "ZWFkZXIYASABKAsyHS5Db21tdW5pY2F0aW9uUHJvdG9jb2wuSGVhZGVyEjcK",
            "C1JlcXVlc3RDb2RlGAIgASgOMiIuQ29tbXVuaWNhdGlvblByb3RvY29sLlJl",
            "cXVlc3RDb2RlEg8KB1BheWxvYWQYAyABKAwqNQoLUmVxdWVzdENvZGUSDQoJ",
            "SGFuZHNoYWtlEAASCQoFTG9naW4QARIMCghSZWdpc3RlchACKlcKDFJlc3Bv",
            "bnNlQ29kZRILCgdTdWNjZXNzEAASEwoPSW52YWxpZFByb3RvY29sEAESEAoM",
            "SW52YWxpZExvZ2luEAISEwoPSW52YWxpZFBhc3N3b3JkEANCLKoCKU5ldHdv",
            "cmtpbmcuUHJvdG9idWYuQ29tbXVuaWNhdGlvblByb3RvY29sYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Networking.Protobuf.CommunicationProtocol.RequestCode), typeof(global::Networking.Protobuf.CommunicationProtocol.ResponseCode), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Networking.Protobuf.CommunicationProtocol.Header), global::Networking.Protobuf.CommunicationProtocol.Header.Parser, new[]{ "PayloadSize" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Networking.Protobuf.CommunicationProtocol.Response), global::Networking.Protobuf.CommunicationProtocol.Response.Parser, new[]{ "Header", "RequestCode", "ResponseCode", "ErrorMessage", "Payload" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Networking.Protobuf.CommunicationProtocol.Request), global::Networking.Protobuf.CommunicationProtocol.Request.Parser, new[]{ "Header", "RequestCode", "Payload" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum RequestCode {
    [pbr::OriginalName("Handshake")] Handshake = 0,
    [pbr::OriginalName("Login")] Login = 1,
    [pbr::OriginalName("Register")] Register = 2,
  }

  public enum ResponseCode {
    [pbr::OriginalName("Success")] Success = 0,
    [pbr::OriginalName("InvalidProtocol")] InvalidProtocol = 1,
    [pbr::OriginalName("InvalidLogin")] InvalidLogin = 2,
    [pbr::OriginalName("InvalidPassword")] InvalidPassword = 3,
  }

  #endregion

  #region Messages
  public sealed partial class Header : pb::IMessage<Header> {
    private static readonly pb::MessageParser<Header> _parser = new pb::MessageParser<Header>(() => new Header());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Header> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Networking.Protobuf.CommunicationProtocol.CommunicationProtocolReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header(Header other) : this() {
      payloadSize_ = other.payloadSize_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Header Clone() {
      return new Header(this);
    }

    /// <summary>Field number for the "PayloadSize" field.</summary>
    public const int PayloadSizeFieldNumber = 1;
    private uint payloadSize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint PayloadSize {
      get { return payloadSize_; }
      set {
        payloadSize_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Header);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Header other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PayloadSize != other.PayloadSize) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PayloadSize != 0) hash ^= PayloadSize.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (PayloadSize != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(PayloadSize);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (PayloadSize != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(PayloadSize);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Header other) {
      if (other == null) {
        return;
      }
      if (other.PayloadSize != 0) {
        PayloadSize = other.PayloadSize;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            PayloadSize = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Response : pb::IMessage<Response> {
    private static readonly pb::MessageParser<Response> _parser = new pb::MessageParser<Response>(() => new Response());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Networking.Protobuf.CommunicationProtocol.CommunicationProtocolReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response(Response other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      requestCode_ = other.requestCode_;
      responseCode_ = other.responseCode_;
      errorMessage_ = other.errorMessage_;
      payload_ = other.payload_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Response Clone() {
      return new Response(this);
    }

    /// <summary>Field number for the "Header" field.</summary>
    public const int HeaderFieldNumber = 1;
    private global::Networking.Protobuf.CommunicationProtocol.Header header_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Networking.Protobuf.CommunicationProtocol.Header Header {
      get { return header_; }
      set {
        header_ = value;
      }
    }

    /// <summary>Field number for the "RequestCode" field.</summary>
    public const int RequestCodeFieldNumber = 2;
    private global::Networking.Protobuf.CommunicationProtocol.RequestCode requestCode_ = global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Networking.Protobuf.CommunicationProtocol.RequestCode RequestCode {
      get { return requestCode_; }
      set {
        requestCode_ = value;
      }
    }

    /// <summary>Field number for the "ResponseCode" field.</summary>
    public const int ResponseCodeFieldNumber = 3;
    private global::Networking.Protobuf.CommunicationProtocol.ResponseCode responseCode_ = global::Networking.Protobuf.CommunicationProtocol.ResponseCode.Success;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Networking.Protobuf.CommunicationProtocol.ResponseCode ResponseCode {
      get { return responseCode_; }
      set {
        responseCode_ = value;
      }
    }

    /// <summary>Field number for the "ErrorMessage" field.</summary>
    public const int ErrorMessageFieldNumber = 4;
    private string errorMessage_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrorMessage {
      get { return errorMessage_; }
      set {
        errorMessage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Payload" field.</summary>
    public const int PayloadFieldNumber = 5;
    private pb::ByteString payload_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Payload {
      get { return payload_; }
      set {
        payload_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Header, other.Header)) return false;
      if (RequestCode != other.RequestCode) return false;
      if (ResponseCode != other.ResponseCode) return false;
      if (ErrorMessage != other.ErrorMessage) return false;
      if (Payload != other.Payload) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (header_ != null) hash ^= Header.GetHashCode();
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) hash ^= RequestCode.GetHashCode();
      if (ResponseCode != global::Networking.Protobuf.CommunicationProtocol.ResponseCode.Success) hash ^= ResponseCode.GetHashCode();
      if (ErrorMessage.Length != 0) hash ^= ErrorMessage.GetHashCode();
      if (Payload.Length != 0) hash ^= Payload.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (header_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Header);
      }
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        output.WriteRawTag(16);
        output.WriteEnum((int) RequestCode);
      }
      if (ResponseCode != global::Networking.Protobuf.CommunicationProtocol.ResponseCode.Success) {
        output.WriteRawTag(24);
        output.WriteEnum((int) ResponseCode);
      }
      if (ErrorMessage.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ErrorMessage);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(42);
        output.WriteBytes(Payload);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (header_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Header);
      }
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) RequestCode);
      }
      if (ResponseCode != global::Networking.Protobuf.CommunicationProtocol.ResponseCode.Success) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ResponseCode);
      }
      if (ErrorMessage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrorMessage);
      }
      if (Payload.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Payload);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Response other) {
      if (other == null) {
        return;
      }
      if (other.header_ != null) {
        if (header_ == null) {
          Header = new global::Networking.Protobuf.CommunicationProtocol.Header();
        }
        Header.MergeFrom(other.Header);
      }
      if (other.RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        RequestCode = other.RequestCode;
      }
      if (other.ResponseCode != global::Networking.Protobuf.CommunicationProtocol.ResponseCode.Success) {
        ResponseCode = other.ResponseCode;
      }
      if (other.ErrorMessage.Length != 0) {
        ErrorMessage = other.ErrorMessage;
      }
      if (other.Payload.Length != 0) {
        Payload = other.Payload;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (header_ == null) {
              Header = new global::Networking.Protobuf.CommunicationProtocol.Header();
            }
            input.ReadMessage(Header);
            break;
          }
          case 16: {
            RequestCode = (global::Networking.Protobuf.CommunicationProtocol.RequestCode) input.ReadEnum();
            break;
          }
          case 24: {
            ResponseCode = (global::Networking.Protobuf.CommunicationProtocol.ResponseCode) input.ReadEnum();
            break;
          }
          case 34: {
            ErrorMessage = input.ReadString();
            break;
          }
          case 42: {
            Payload = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Request : pb::IMessage<Request> {
    private static readonly pb::MessageParser<Request> _parser = new pb::MessageParser<Request>(() => new Request());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Request> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Networking.Protobuf.CommunicationProtocol.CommunicationProtocolReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request(Request other) : this() {
      header_ = other.header_ != null ? other.header_.Clone() : null;
      requestCode_ = other.requestCode_;
      payload_ = other.payload_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Request Clone() {
      return new Request(this);
    }

    /// <summary>Field number for the "Header" field.</summary>
    public const int HeaderFieldNumber = 1;
    private global::Networking.Protobuf.CommunicationProtocol.Header header_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Networking.Protobuf.CommunicationProtocol.Header Header {
      get { return header_; }
      set {
        header_ = value;
      }
    }

    /// <summary>Field number for the "RequestCode" field.</summary>
    public const int RequestCodeFieldNumber = 2;
    private global::Networking.Protobuf.CommunicationProtocol.RequestCode requestCode_ = global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Networking.Protobuf.CommunicationProtocol.RequestCode RequestCode {
      get { return requestCode_; }
      set {
        requestCode_ = value;
      }
    }

    /// <summary>Field number for the "Payload" field.</summary>
    public const int PayloadFieldNumber = 3;
    private pb::ByteString payload_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Payload {
      get { return payload_; }
      set {
        payload_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Request);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Request other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Header, other.Header)) return false;
      if (RequestCode != other.RequestCode) return false;
      if (Payload != other.Payload) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (header_ != null) hash ^= Header.GetHashCode();
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) hash ^= RequestCode.GetHashCode();
      if (Payload.Length != 0) hash ^= Payload.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (header_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Header);
      }
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        output.WriteRawTag(16);
        output.WriteEnum((int) RequestCode);
      }
      if (Payload.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(Payload);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (header_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Header);
      }
      if (RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) RequestCode);
      }
      if (Payload.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Payload);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Request other) {
      if (other == null) {
        return;
      }
      if (other.header_ != null) {
        if (header_ == null) {
          Header = new global::Networking.Protobuf.CommunicationProtocol.Header();
        }
        Header.MergeFrom(other.Header);
      }
      if (other.RequestCode != global::Networking.Protobuf.CommunicationProtocol.RequestCode.Handshake) {
        RequestCode = other.RequestCode;
      }
      if (other.Payload.Length != 0) {
        Payload = other.Payload;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (header_ == null) {
              Header = new global::Networking.Protobuf.CommunicationProtocol.Header();
            }
            input.ReadMessage(Header);
            break;
          }
          case 16: {
            RequestCode = (global::Networking.Protobuf.CommunicationProtocol.RequestCode) input.ReadEnum();
            break;
          }
          case 26: {
            Payload = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code