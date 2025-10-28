
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class MonoSaveHandler<TSavable, TSaveData> : SaveHandlerGenericBase<TSavable, TSaveData>
        where TSavable : UnityEngine.Component
        where TSaveData : MonoSaveDataBase, new()
    {
        public override void Init(object instance)
        {
            base.Init(instance);

            
            __saveData.GameObjectId = GetObjectId(__instance.gameObject);
        }


        public override void CreateObject()
        {
            base.CreateObject();


            _AssignInstance();

            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, __saveData._ObjectId_);


            if (__instance.GetType().IsAssignableTo(typeof(IGameLoopIntegrator)))
            {
                SaveAndLoadManager.Singleton.RegisterIntegrator(__instance as IGameLoopIntegrator);
            }
        }

        public override void _AssignInstance()
        {
            var goSH = SaveAndLoadManager.Singleton.GetSaveHandlerById<GameObjectSaveHandler>(__saveData.GameObjectId);
            //var go = Infra.Singleton.GetObjectById<GameObject>(__saveData.GameObjectId);

            __instance = goSH.AddComponent<TSavable>();
        }
    }
}
