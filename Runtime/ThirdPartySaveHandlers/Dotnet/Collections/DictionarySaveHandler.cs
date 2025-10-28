using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System.Collections.Generic;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Dotnet.Collections
{
    [SaveHandler(id:3242349838493284, "Dictionary`2", typeof(Dictionary<,>))]
    public class DictionarySaveHandler<TKey, TValue> : UnmanagedSaveHandler<Dictionary<TKey, TValue>, DictionarySaveData<TKey,TValue>>
    {

        public override void WriteSaveData()
        {
            base.WriteSaveData();


            __saveData.pairs.Clear();
            foreach((var key, var value) in __instance)
            {
                var keyDatum = new Data<TKey>
                {
                    ReferencedBy = HandledObjectId,
                    Value = key
                };

                var valueDatum = new Data<TValue>
                {
                    ReferencedBy = HandledObjectId,
                    Value = value
                };

                __saveData.pairs.Add((keyDatum,valueDatum));
            }
        }


        public override void LoadReferences()
        {
            base.LoadReferences();


            foreach(var pair in __saveData.pairs)
            {
                __instance.Add(pair.key.Value, pair.value.Value);
            }
        }
    }


    public class DictionarySaveData<TKey, TValue> : SaveDataBase
    {
        public List<(Data<TKey> key, Data<TValue> value)> pairs = new();
    }
}
