using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFacIOC
{
    public class ConsoleOutput : IOutPut
    {
        public void Write(string content)
        {
            Console.WriteLine(content);
        }
    }
}
