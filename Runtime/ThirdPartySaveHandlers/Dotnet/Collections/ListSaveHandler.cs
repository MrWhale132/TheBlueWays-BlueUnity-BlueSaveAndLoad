using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Dotnet.Collections
{

    [SaveHandler(43292483249582532, "List`1", typeof(List<>))]
    public class ListSaveHandler<T> : UnmanagedSaveHandler<IList<T>, ListSaveData<T>>
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

            foreach(var datum in __saveData.data)
            {
                __instance.Add(datum.Value);
            }
        }
    }


    public class ListSaveData<T> : SaveDataBase
    {
        public List<Data<T>> data = new();
    }
}
