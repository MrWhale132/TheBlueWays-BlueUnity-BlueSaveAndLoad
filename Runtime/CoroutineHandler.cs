
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    public class CoroutineHandler
    {
        public Coroutine _routine;
        public DelegateSaveInfo _delegateInfo;
        public RandomId _routineState;
        public RandomId _targetMonoId;
        public bool _finished;
    }

    public static class MonoBehaviourExtensions
    {

        public static Coroutine StartSavableCoroutine<T>(this MonoBehaviour mono, Func<T, IEnumerator> routine, T state) where T : class, new() 
        {
            var targetId = Infra.Singleton.GetObjectId(mono, Infra.Singleton.GlobalReferencing);
            var stateId = Infra.Singleton.GetObjectId(state, Infra.Singleton.GlobalReferencing);
            var delegateSaveInfo = Infra.Singleton.GetDelegateSaveInfo(routine);

            var savableRoutine = new CoroutineHandler()
            {
                _delegateInfo = delegateSaveInfo,
                _routineState = stateId,
                _targetMonoId = targetId
            };
            

            IEnumerator Wrap()
            {
                var coroutine = mono.StartCoroutine(routine(state));
                savableRoutine._routine = coroutine;

                yield return coroutine;
                savableRoutine._finished = true;
            }

            Coroutine wrapper = mono.StartCoroutine(Wrap());

            Infra.Singleton.RegisterCoroutine(wrapper, savableRoutine);

            return wrapper;
        }


        public static Coroutine StartCoroutineFromSaveData(this MonoBehaviour mono, Delegate coroutine, object state)
        {
            var targetId = Infra.Singleton.GetObjectId(mono, Infra.Singleton.GlobalReferencing);
            var stateId = Infra.Singleton.GetObjectId(state, Infra.Singleton.GlobalReferencing);
            var delegateSaveInfo = Infra.Singleton.GetDelegateSaveInfo(coroutine);

            var routine = (IEnumerator)coroutine.DynamicInvoke(state);


            var routineHandler = new CoroutineHandler
            {
                _delegateInfo = delegateSaveInfo,
                _targetMonoId = targetId,
                _routineState = stateId,
            };


            IEnumerator Wrap()
            {
                var coroutine = mono.StartCoroutine(routine);
                routineHandler._routine = coroutine;

                yield return coroutine;
                routineHandler._finished = true;
            }


            Coroutine wrapper = mono.StartCoroutine(Wrap());

            Infra.Singleton.RegisterCoroutine(wrapper, routineHandler);

            return wrapper;
        }



        public static void StopSavedCoroutine(this MonoBehaviour mono, Coroutine coroutine)
        {
            var coroutineData = Infra.Singleton.GetCoroutineHandler(coroutine);

            mono.StopCoroutine(coroutineData._routine);

            coroutineData._finished = true;
        }


        public static void StopAllSavedCoroutine(this MonoBehaviour mono)
        {
            var routines = Infra.Singleton.GetAllCoroutinesByMono(mono);

            foreach ( var routine in routines)
            {
                mono.StopSavedCoroutine(routine);
            }
        }
    }
}
