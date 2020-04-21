using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFacIOC
{
    public class TodayWrite : IDateWriter
    {
        private IOutPut _outPut;
        public TodayWrite(IOutPut outPut)
        {
            _outPut = outPut;
        }
        public void WriteData()
        {
            this._outPut.Write(DateTime.Today.ToShortDateString());
        }
    }
}
