using Hiywin.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.Models.Base
{
    public class SqlConnModel : ISqlConnModel
    {
        public string MysqlConn { get; set; }
        public string MssqlConn { get; set; }
    }
}
