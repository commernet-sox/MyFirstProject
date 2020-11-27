using System;
using System.Collections.Generic;
using System.Text;

namespace TestData.DTO
{
    public class TestApiDTO:BaseDTO
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}
