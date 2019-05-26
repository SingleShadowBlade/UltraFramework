@echo off
set tool=ProtoGen
 
SET BASE_DIR=%CD%
SET PROTO_PER_COMPILE_BIN=\Precompile\precompile.exe 
SET DLL_NAME=ProtoCoreData.dll
SET DOT_NET_VER=v2.0.50727
SET CSC_MAKE=C:\Windows\Microsoft.NET\Framework\%DOT_NET_VER%\csc.exe
 
set proto=login.proto
%tool%\protogen.exe -i:%proto% -o:%proto%.cs -ns:MyProto
 
%CSC_MAKE% /r:%BASE_DIR%\unity\protobuf-net.dll /out:%DLL_NAME% /target:library *.cs
%BASE_DIR%%PROTO_PER_COMPILE_BIN% %DLL_NAME% -o:Serialzer_%DLL_NAME% -p:%BASE_DIR%\unity\ -t:ProtobufSerializer
 
pause