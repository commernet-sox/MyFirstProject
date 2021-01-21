0.首先下载一个Consul文件，由于github上传不了大于100M的文件。其他配置文件都有。
1.启动Consul，可以通过bat文件双击执行。
2.打开浏览器http://127.0.0.1:8500/ui/dc1/services
3.打开网关，运行网关exe或者vs运行。
4.打开goodapi项目，到项目文件下，按Shift+右键，在power shell中打开，
5.输入 dotnet run 执行.
6.此时consul就会出来Good服务。
7.现在就可以通过网关暴露的地址访问good项目的接口了。
8.输入http://localhost:5000/a/api/good，返回good接口返回值。
9.good项目也可以访问。http://localhost:9001/api/good
10.其他服务像这样配置即可。