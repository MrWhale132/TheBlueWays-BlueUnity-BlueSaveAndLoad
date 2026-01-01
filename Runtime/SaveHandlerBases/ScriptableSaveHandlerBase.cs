
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

            HandledObjectId = __saveData._ObjectId_;

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

            _AssignInstance();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;



            Infra.Singleton.RegisterReference(__instance, __saveData._ObjectId_, rootObject: __saveData._isRootObject_);
        }

        public override void _AssignInstance()
        {
            base._AssignInstance();

            __instance = ScriptableObject.CreateInstance<TScriptable>();
        }
    }
}
