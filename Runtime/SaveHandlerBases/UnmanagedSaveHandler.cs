
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{

    public class UnmanagedSaveHandler<TSavable, TSaveData> : SaveHandlerGenericBase<TSavable, TSaveData>
        where TSavable : class
        where TSaveData : SaveDataBase, new()
    {


        public override void CreateObject()
        {
            base.CreateObject();


            HandledObjectId = __saveData._ObjectId_;


            Type instanceType = Type.GetType(__saveData._AssemblyQualifiedName_);

            if (instanceType == null)
            {
                //debug error
                Debug.LogError($"Couldn't load the type that was in the save file. Type name in save file: {__saveData._AssemblyQualifiedName_}. " +
                    $"Cant do anything so let it go on.");
            }


            bool notArrayType = !typeof(Array).IsAssignableFrom(instanceType);

            if (notArrayType)
            {
                SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

                //todo: replace this with something else
                //update: take into account il2cpp
                try
                {
                    _AssignInstance();
                }
                catch
                {
                    Debug.Log(__saveData._ObjectId_);
                    Debug.Log(__saveData._AssemblyQualifiedName_);
                    throw;
                }

                SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;

                Infra.Singleton.RegisterReference(__instance, HandledObjectId);


                if (__instance.GetType().IsAssignableTo(typeof(IGameLoopIntegrator)))
                {
                    SaveAndLoadManager.Singleton.RegisterIntegrator(__instance as IGameLoopIntegrator);
                }
            }
        }


        public override void _AssignInstance()
        {
            base._AssignInstance();

            Type instanceType = Type.GetType(__saveData._AssemblyQualifiedName_);

                __instance = (TSavable)Activator.CreateInstance(instanceType);
        }
    }
}
