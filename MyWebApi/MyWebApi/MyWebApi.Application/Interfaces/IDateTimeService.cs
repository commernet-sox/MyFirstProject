using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
