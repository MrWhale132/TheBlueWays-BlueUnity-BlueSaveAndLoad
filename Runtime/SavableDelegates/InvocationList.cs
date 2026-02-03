
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.SavableDelegates
{
    public class InvocationList
    {
        public List<DelegateSaveInfo> Delegates { get; set; } = new List<DelegateSaveInfo>();

        public static InvocationList From(params (RandomId target, long methodId)[] delegates)
        {
            List<DelegateSaveInfo> saveInfos = new();

            foreach ((var target, var method) in delegates)
            {
                var info = new DelegateSaveInfo(target, method);

                saveInfos.Add(info);
            }

            var list = new InvocationList { Delegates = saveInfos };
            return list;
        }
    }
}
