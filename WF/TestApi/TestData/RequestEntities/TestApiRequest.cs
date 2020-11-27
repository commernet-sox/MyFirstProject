using System;
using System.Collections.Generic;
using System.Text;

namespace TestData.RequestEntities
{
    public class TestApiRequest
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
    }
}
