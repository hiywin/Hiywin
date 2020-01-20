using Hiywin.FrameService;
using Hiywin.IFrameService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hiywin.Utility
{
    public class FrameOperaters
    {
        private static Dictionary<string, object> dic = new Dictionary<string, object>();

        public static bool Init()
        {
            if (!dic.ContainsKey("IModuleService"))
                dic.Add("IModuleService", null);


            return true;
        }

        public static IModuleService ModuleOperater
        {
            get
            {
                var svr = dic["IModuleService"] as IModuleService;
                if (svr != null) return svr;

                svr = new ModuleService();

                dic["IModuleService"] = svr;
                return svr;
            }
        }
    }
}
