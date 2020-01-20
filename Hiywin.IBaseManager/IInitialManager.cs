using Hiywin.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.IBaseManager
{
    public interface IInitialManager
    {
        bool InitData(ISqlConnModel model);
    }
}
