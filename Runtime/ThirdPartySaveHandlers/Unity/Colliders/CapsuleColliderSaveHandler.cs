
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Colliders
{
    //[SaveHandler(3984329875123, nameof(CapsuleCollider), typeof(CapsuleCollider) )]
    public class CapsuleColliderSaveHandler:ColliderSavehandler<CapsuleCollider, CapsuleColliderSaveData>
    {
        public CapsuleColliderSaveHandler()
        {

        }


        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Center = __instance.center;
            __saveData.Radius = __instance.radius;
            __saveData.Height = __instance.height;
            __saveData.Direction = __instance.direction;
        }


        public override void LoadValues()
        {
            base.LoadValues();

            __instance.center = __saveData.Center;
            __instance.radius = __saveData.Radius;
            __instance.height = __saveData.Height;
            __instance.direction = __saveData.Direction;
        }
    }

    public class CapsuleColliderSaveData : ColliderSaveData
    {
        public Vector3 Center;
        public float Radius;
        public float Height;
        public int Direction;
    }
}
