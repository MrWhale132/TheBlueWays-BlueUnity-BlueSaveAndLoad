
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public abstract class StaticSubtitute
    {
        public abstract Type SubtitutedType { get; }
    }

    public abstract class StaticSubtitute<T> : StaticSubtitute
    {
        public override Type SubtitutedType => typeof(T);
    }


    public class StaticSaveHandlerBase<TSubstitute, TSaveData> : SaveHandlerGenericBase<TSubstitute, TSaveData>
        where TSubstitute : StaticSubtitute, new()
        where TSaveData : StaticSaveDataBase, new()
    {
        //static handlers are called with null
        public override void Init(object instance)
        {
            instance = new TSubstitute();
            base.Init(instance);
        }

        public override void _SetObjectId()
        {
            HandledObjectId = SaveAndLoadManager.Singleton.GetOrCreateSingletonObjectIdBySaveHandlerId(SaveHandlerId);
            Infra.Singleton.RegisterReference(__instance, HandledObjectId, rootObject: true);
            Infra.Singleton.RegisterStaticSubtitute(__instance, HandledObjectId);


            if (HandledObjectId.IsDefault)
            {
                Debug.LogError(__instance.GetType().FullName);
            }
        }
    }


    public class StaticSaveDataBase : SaveDataBase
    {
    }






    public class GenericWithStaticExampleClass<T> //or generic and static
    {
        static GenericWithStaticExampleClass()
        {
            //todo
            //static ctor called for each type of T when it's first used with that type, you can register savehandlers here
        }

        public static int fieldPerGenArg;
    }
}
