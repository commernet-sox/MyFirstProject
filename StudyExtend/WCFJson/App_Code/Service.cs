using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、服务和配置文件中的类名“Service”。
public class Service : IService
{
	public string GetData(string value)
	{
		return string.Format("You entered: {0}", value);
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}

    public object GetJson()
    {
		return Newtonsoft.Json.JsonConvert.SerializeObject(new { name = "wangfeng" });
    }

    public object GetSet()
    {
		DataSet dataSet = new DataSet();
		DataTable table1 = new DataTable();
		table1.Columns.Add("name");
		table1.Rows.Add("wangfeng");
		dataSet.Tables.Add(table1);
		DataTable table2 = new DataTable();
		table2.Columns.Add("city");
		table2.Rows.Add("china");
		dataSet.Tables.Add(table2);
		return JsonConvert.SerializeObject( dataSet);
	}

    public stu GetStu(stu stu)
    {
		if (stu == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (stu.name!="")
		{
			stu.name += "Suffix";
		}
		return stu;
	}
}
