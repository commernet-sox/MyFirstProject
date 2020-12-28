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

% ���ߵ�ַ��packages\grpc.tools\2.31.0\tools\windows_x64\protoc.exe							
csharpģ���ļ��洢Ŀ¼��--csharp_out [grpc���ļ���Ŀ¼]  [GrpcЭ���ļ�·��] 
 csharp�������ļ��洢Ŀ¼��--grpc_out [grpc�����ļ���Ŀ¼] 
 csharp���·����--plugin=protoc-gen-grpc=packages\Grpc.Tools.1.0.0\tools\windows_x86\grpc_csharp_plugin.exe %



setlocal

@rem enter this directory
cd /d %~dp0

set TOOLS_PATH=C:\Users\jsnan\.nuget\packages\grpc.tools\2.31.0\tools\windows_x64

%TOOLS_PATH%\protoc.exe --csharp_out "Ksd.Service.Grpc/Services" "Ksd.Service.Grpc/FlowerX.proto" --grpc_out "Kce.Test/Grpc" --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

pause
endlocal
