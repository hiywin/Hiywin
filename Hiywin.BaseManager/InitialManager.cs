using Hiywin.Entities.Base;
using Hiywin.IBaseManager;
using Hiywin.Utility.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.BaseManager
{
    public class InitialManager : IInitialManager
    {
        public bool InitData(ISqlConnModel model)
        {
            return InitOperators.Init(model);
        }
    }
}
