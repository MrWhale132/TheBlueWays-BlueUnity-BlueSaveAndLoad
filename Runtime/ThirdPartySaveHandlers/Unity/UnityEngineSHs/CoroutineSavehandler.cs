
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.ThirdPartySaveHandlers.Unity.UnityEngineSHs
{

    [SaveHandler(677698678234523, nameof(Coroutine), typeof(Coroutine))]
    public class CoroutineSavehandler : UnmanagedSaveHandler<Coroutine, CoroutineSaveData>
    {
        public CoroutineHandler _handler;

        public override void Init(object instance)
        {
            base.Init(instance);

            _handler = Infra.Singleton.GetCoroutineHandler(__instance);

            __saveData._delegateInfo = _handler._delegateInfo;
            __saveData._routineState = _handler._routineState;
            __saveData._targetMonoId = _handler._targetMonoId;
        }


        public override void WriteSaveData()
        {
            base.WriteSaveData();

            if (__saveData != null)
            {
                var target = Infra.Singleton.GetObjectById<Object>(_handler._targetMonoId);

                bool targetExists = target != null;

                if (!targetExists || _handler._finished)
                {
                    __saveData = null;
                }
                else
                {
                    Infra.Singleton.KeepAlive(_handler._routineState, HandledObjectId);
                    Infra.Singleton.KeepAlive(_handler._targetMonoId, HandledObjectId);
                }
            }
        }


        ////this logic to use this separate method to ensure target and state are exist can be replaced by giving a higher ored to this savehandler so it comes later
        //// a realy high order because your target can be anything and how would you know what order they might have?
        //public override void RegisterDelegates()
        //{
        //    base.RegisterDelegates();

        //    HandledObjectId = __saveData._ObjectId_;

        //    Type instanceType = Type.GetType(__saveData._AssemblyQualifiedName_);

        //    if (instanceType == null)
        //    {
        //        //debug error
        //        Debug.LogError($"Couldn't load the type that was in the save file. Type name in save file: {__saveData._AssemblyQualifiedName_}. " +
        //            $"Cant do anything so let it go on.");
        //    }

        //    var targetMono = Infra.Singleton.GetObjectById<MonoBehaviour>(__saveData._targetMonoId);

        //    var targetMethod = Infra.Singleton.GetDelegate<Delegate>(__saveData._delegateInfo);

        //    var routineState = Infra.Singleton.GetObjectById<object>(__saveData._routineState);

        //    //todo: out var handler? update: GetCoroutineHandler would needed elsewhere anyway
        //    __instance = targetMono.StartCoroutineFromSaveData(targetMethod, routineState);

        //    _handler = Infra.Singleton.GetCoroutineHandler(__instance);


        //    Infra.Singleton.RegisterReference(__instance, HandledObjectId);
        //}

        public override void _AssignInstance()
        {
            var targetMono = Infra.Singleton.GetObjectById<MonoBehaviour>(__saveData._targetMonoId);

            var targetMethod = Infra.Singleton.GetDelegate<Delegate>(__saveData._delegateInfo);

            var routineState = Infra.Singleton.GetObjectById<object>(__saveData._routineState);

            //todo: out var handler? update: GetCoroutineHandler would needed elsewhere anyway
            __instance = targetMono.StartCoroutineFromSaveData(targetMethod, routineState);

            _handler = Infra.Singleton.GetCoroutineHandler(__instance);
        }

        public override void ReleaseObject()
        {
            base.ReleaseObject();

            _handler = null;
            //todo: double check if anything else could be needed
        }
    }


    public class CoroutineSaveData : SaveDataBase
    {
        public DelegateSaveInfo _delegateInfo;
        public RandomId _routineState;
        public RandomId _targetMonoId;
    }
}
