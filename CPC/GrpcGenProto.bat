@rem Copyright 2016 gRPC authors.
@rem
@rem Licensed under the Apache License, Version 2.0 (the "License");
@rem you may not use this file except in compliance with the License.
@rem You may obtain a copy of the License at
@rem
@rem     http://www.apache.org/licenses/LICENSE-2.0
@rem
@rem Unless required by applicable law or agreed to in writing, software
@rem distributed under the License is distributed on an "AS IS" BASIS,
@rem WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
@rem See the License for the specific language governing permissions and
@rem limitations under the License.

@rem Generate the C# code for .proto files

% 工具地址：packages\grpc.tools\2.31.0\tools\windows_x64\protoc.exe							
csharp模型文件存储目录：--csharp_out [grpc类文件的目录]  [Grpc协议文件路径] 
 csharp服务类文件存储目录：--grpc_out [grpc服务文件的目录] 
 csharp插件路径：--plugin=protoc-gen-grpc=packages\Grpc.Tools.1.0.0\tools\windows_x86\grpc_csharp_plugin.exe %



setlocal

@rem enter this directory
cd /d %~dp0

set TOOLS_PATH=C:\Users\jsnan\.nuget\packages\grpc.tools\2.31.0\tools\windows_x64

%TOOLS_PATH%\protoc.exe --csharp_out "Ksd.Service.Grpc/Services" "Ksd.Service.Grpc/FlowerX.proto" --grpc_out "Kce.Test/Grpc" --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

pause
endlocal
