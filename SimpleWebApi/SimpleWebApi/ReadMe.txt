1.创建一个asp.net core web 空的应用程序。
2.Nuget引用CPC.Service包
3.在program.cs类中使用UseEngine()；方法
4.在startup.cs类中继承BaseStartup类。
5.添加startup构造函数
6.在ConfigureServices方法中添加AddNoLockDb（）方法。
7.在appsettings.json中添加数据库连接字符串。
8.创建一个用来增加字段基础设施类库，Infrastructure。
9.引用CPC.DBCore，EFCore.sqlserver,EFCore.Tool包。
10.添加DbContext数据库操作上下文类.
11.添加一个测试的类TestApi
12.添加一个用来放Entity实体的类库Core。
13.Nuget添加CPC，EFCore 3.1.10.
14.创建Entities文件夹存放实体类。
15.创建一个测试实体,创建一个基础实体类BaseEntity。
16.添加项目引用。
17.添加存放实体DTO的类库。Data。
18.Nuget引用CPC，System.ComponentModel.Annotations，System.Runtime。
19.创建一个BaseDto类，添加测试DTO，
20.创建一个Application文件夹，存放一些底层的操作和服务。
21.在Core中添加BaseController,Operate,DbHost,MapperConf操作类。
22.在Service文件夹添加BaseService,TestApiService操作类.
23.添加Controllers文件夹，存放控制器。
24.添加SwaggerController用来显示接口文档。
25.添加测试控制器。
26.添加xml接口文档。
27.添加接口文档服务。
28.添加服务注册。
29.添加中间件。
30.添加请求地址映射.
31.设置启动方式，不用IIS。
32.接口文档地址。http://127.0.0.1:5000/doc/index.html

33.update-database   更新数据库字段，再不启动的情况下
34.add-migration 版本号   添加实体类版本