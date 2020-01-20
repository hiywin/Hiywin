using Hiywin.Common.Helpers;
using Hiywin.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.Utility.Base
{
    public class InitOperators
    {
        public static bool Init(ISqlConnModel sqlConn)
        {
            bool flag;
            MysqlHelper.MysqlConn = sqlConn.MysqlConn;
            MssqlHelper.ConnCommon = sqlConn.MssqlConn;

            flag = FrameOperaters.Init();

            return flag;
        }

    }
}
