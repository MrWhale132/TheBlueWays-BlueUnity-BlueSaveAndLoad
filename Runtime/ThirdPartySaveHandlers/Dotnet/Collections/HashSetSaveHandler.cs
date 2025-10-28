
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Dotnet.Collections
{
    [SaveHandler(654866735345345, "HashSet`1", typeof(HashSet<>))]
    public class HashSetSaveHandler<T> : UnmanagedSaveHandler<HashSet<T>, HashSetSaveData<T>>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            
            __saveData.data.Clear();
            foreach (var item in __instance)
            {
                var datum = new Data<T>
                {
                    ReferencedBy = HandledObjectId,
                    Value = item,
                };
                __saveData.data.Add(datum);
            }
            return;
        }


        public override void LoadReferences()
        {
            base.LoadReferences();

            foreach (var datum in __saveData.data)
            {
                __instance.Add(datum.Value);
            }
        }
    }


    public class HashSetSaveData<T> : SaveDataBase
    {
        public List<Data<T>> data = new();
    }
}
