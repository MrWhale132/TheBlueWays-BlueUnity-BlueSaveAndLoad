
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.SavableDelegates
{
    public class InvocationList
    {
        public List<DelegateSaveInfo> Delegates { get; set; } = new List<DelegateSaveInfo>();
    }
}
