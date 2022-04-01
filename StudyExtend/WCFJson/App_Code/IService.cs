using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService”。
[ServiceContract]
public interface IService
{

	[OperationContract]
	[WebGet(UriTemplate ="/wcf/{value}",RequestFormat =WebMessageFormat.Json,ResponseFormat =WebMessageFormat.Json)]
	string GetData(string value);

	[OperationContract]
	[WebInvoke(Method = "POST", UriTemplate = "GetDataUsingDataContract", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
	CompositeType GetDataUsingDataContract(CompositeType composite);

	[OperationContract]
	[WebInvoke(Method ="POST",UriTemplate ="GetJson",BodyStyle =WebMessageBodyStyle.Bare,RequestFormat =WebMessageFormat.Json,ResponseFormat =WebMessageFormat.Json)]
	object GetJson();

	[OperationContract]
	[WebInvoke(Method = "POST", UriTemplate = "GetStu", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
	stu GetStu(stu stu);

	[OperationContract]
	[WebInvoke(Method = "POST", UriTemplate = "GetSet", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
	object GetSet();
	// TODO: 在此添加您的服务操作
}

// 使用下面示例中说明的数据约定将复合类型添加到服务操作。
[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

	[DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}


public class stu
{
	public string name { get; set; }
	public string city { get; set; }
}
