
using Assets._Project.Scripts.UtilScripts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.SavableDelegates
{
    public class DelegateSaveInfo
    {
        public RandomId TargeId { get; init; }
        public long MethodId { get; init; }
        public bool GetByMethodInfo { get; init; }
        [JsonIgnore]
        public bool IsGeneric => GenericTypeArguments != null && GenericTypeArguments.Count > 0;
        public long GenericVairantId { get; set; }
        public List<string> GenericTypeArguments { get; set; }

        public DelegateSaveInfo(RandomId targetInstanceId, long methodId)
        {
            TargeId = targetInstanceId;
            MethodId = methodId;
        }

        public DelegateSaveInfo Copy()
        {
            return new DelegateSaveInfo(TargeId, MethodId)
            {
                GetByMethodInfo = GetByMethodInfo,
                GenericVairantId = GenericVairantId,
                GenericTypeArguments = GenericTypeArguments,
            };
        }
    }
}
