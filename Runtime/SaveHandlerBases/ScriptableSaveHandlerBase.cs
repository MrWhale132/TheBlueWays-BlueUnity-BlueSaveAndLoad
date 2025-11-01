
using Assets._Project.Scripts.Infrastructure;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class ScriptableSaveHandlerBase<TScriptable, TSaveData> : SaveHandlerGenericBase<TScriptable, TSaveData>
        where TScriptable : ScriptableObject
        where TSaveData : SaveDataBase, new()
    {
        public override bool IsValid => __instance != null;


        public override void CreateObject()
        {
            base.CreateObject();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

            _AssignInstance();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;


            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, __saveData._ObjectId_,rootObject:true);
        }

        public override void _AssignInstance()
        {
            base._AssignInstance();

            __instance = ScriptableObject.CreateInstance<TScriptable>();
        }
    }
}
