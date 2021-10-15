using System;
using System.Collections.Generic;
using System.Text;

namespace StudyExtend.AutoJson
{
    public class RCSJson
    {
        public static void json(int row,int col)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            for (int i = 0; i < row; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("row",i);
                dic.Add("y", 10* (i + 1) + 10);
                dic.Add("isAverage", true);
                List<Dictionary<string, object>> list1 = new List<Dictionary<string, object>>();
                for (int j = 0; j < col; j++)
                {
                    Dictionary<string, object> dic1 = new Dictionary<string, object>();
                    dic1.Add("type","Step");
                    dic1.Add("text", (i + 1) + "" + (j + 1));
                    //dic1.Add("x","row2.data["+j+"].x");
                    dic1.Add("name","step_"+(i+1)+"_"+(j+1));
                    List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
                    Dictionary<string, object> dic2 = new Dictionary<string, object>();
                    dic2.Add("arrow", "drawBottomToTop");
                    dic2.Add("to", "step_"+(i+2)+"_"+(j+1));
                    list2.Add(dic2);
                    if (j != col - 1 && (i==4||i==15))
                    {
                        Dictionary<string, object> dic3 = new Dictionary<string, object>();
                        dic3.Add("arrow", "drawRightToLeft");
                        dic3.Add("to", "step_" + (i + 1) + "_" + (j + 2));
                        list2.Add(dic3);
                    }
                    dic1.Add("arrowArr", list2);
                    list1.Add(dic1);
                }
                dic.Add("data", list1);
                list.Add(dic);
            }
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
    }
}
