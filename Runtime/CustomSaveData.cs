
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.UtilScripts;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    //T should only be a struct
    public abstract class CustomSaveData<TStruct> : CustomSaveData ///where T : struct, cant because of <see cref="Data{T}"/>
    {
        public CustomSaveData()
        {
            
        }

        public abstract void ReadFrom(in TStruct instance);
        public abstract void WriteTo(ref TStruct instance);
        public void ReadFrom(TStruct instance)
        {
            ReadFrom(in instance);
        }
        public void WriteTo(TStruct instance)
        {
            WriteTo(ref instance);
        }



        public RandomId GetObjectId(object obj)
        {
            return Infra.Singleton.GetObjectId(obj, Infra.Singleton.GlobalReferencing);
        }
        //todo:cleanup
        //public RandomId GetAssetId(UnityEngine.Object asset)
        //{
        //    return AddressableDb.Singleton.GetAssetIdByAssetName(asset);
        //}

        public InvocationList GetInvocationList<T>(T del) where T : Delegate
        {
            return Infra.Singleton.GetInvocationList(del);
        }


        //properties cant use the .AssignById(ref) version because they cant be ref-ed
        public T GetObjectById<T>(RandomId id)
        {
            return Infra.Singleton.GetObjectById<T>(id);
        }

        //public T GetAssetById<T>(RandomId id, T fallback) where T : UnityEngine.Object
        //{
        //    var asset = AddressableDb.Singleton.GetAssetByIdOrFallback<T>(fallback, ref id);

        //    return asset;
        //}

        public T GetDelegate<T>(InvocationList list) where T : Delegate
        {
            return Infra.Singleton.GetDelegate<T>(list);
        }
    }

    public class CustomSaveData { }
}
