
using Assets._Project.Scripts.Infrastructure;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class ScriptableSaveHandlerBase<TScriptable, TSaveData> : SaveHandlerGenericBase<TScriptable, TSaveData>
        where TScriptable : ScriptableObject
        where TSaveData : SaveDataBase, new()
    {
        public override void CreateObject()
        {
            base.CreateObject();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

            __instance = ScriptableObject.CreateInstance<TScriptable>();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;


            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, __saveData._ObjectId_,rootObject:true);
        }
    }
}
