using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Db
{
    public interface IHWMSDbFactory
    {
        SimpleWebApiContext Create();

        SimpleWebApiContext CreateWithNoLock();
    }
}
