using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Newtonsoft.Json;
using System;
using UnityEngine;


namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers
{
    [SaveHandler(id: 32989643, dataGroupName: nameof(Transform), typeof(Transform), order:-9)]
    public class TransformSaveHandler : MonoSaveHandler<Transform, TransformSaveData>
    {
        public TransformSaveHandler() { }


        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.localRotation = __instance.localRotation;
            __saveData.localScale = __instance.localScale;
            __saveData.localPosition = __instance.localPosition;

            if (__instance.parent != null)
            {
                __saveData.ParentGOId = Infra.Singleton.GetObjectId(__instance.parent.gameObject, HandledObjectId);
            }
        }


        public override string Serialize()
        {
            return JsonConvert.SerializeObject(__saveData, Formatting.None);
        }




        public override void CreateObject()
        {
            HandledObjectId = __saveData._ObjectId_;

            var go = Infra.Singleton.GetObjectById<GameObject>(__saveData.GameObjectId);
            
            __instance = go.transform;

            Infra.Singleton.RegisterReference(__instance, __saveData._ObjectId_);
        }



        public override void LoadReferences()
        {
            base.LoadReferences();


            //parent id can be null for root objects
            if (!__saveData.ParentGOId.IsDefault)
            {
                GameObject parent = Infra.Singleton.GetObjectById<GameObject>(__saveData.ParentGOId);
                //if (parent == null) Debug.Log(HandledObjectId);
                //if (parent == null) Debug.Log(__saveData._ObjectId_);
                //if (parent == null) Debug.Log(__saveData.GameObjectId);
                __instance.SetParent(parent.transform);
            }
        }



        public override void LoadValues()
        {
            base.LoadValues();

            __instance.localPosition = __saveData.localPosition;
            __instance.localRotation = __saveData.localRotation;
            __instance.localScale = __saveData.localScale;
        }
    }


    public class TransformSaveData : MonoSaveDataBase
    {
        public RandomId ParentGOId;
        public Quaternion localRotation;
        public Vector3 localScale;
        public Vector3 localPosition;
    }













}
