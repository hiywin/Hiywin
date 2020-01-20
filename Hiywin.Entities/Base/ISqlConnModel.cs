using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.Entities.Base
{
    public interface ISqlConnModel
    {
        string MysqlConn { get; set; }
        string MssqlConn { get; set; }
    }
}
