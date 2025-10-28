
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Colliders
{
    //[SaveHandler(32547568658771, nameof(SphereCollider), typeof(SphereCollider))]
    public class SphereColliderSaveHandler:ColliderSavehandler<SphereCollider, SphereColliderSaveData>
    {
        public SphereColliderSaveHandler()
        {

        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Center = __instance.center;
            __saveData.Radius = __instance.radius;
        }

        public override void LoadValues()
        {
            base.LoadValues();

            __instance.center = __saveData.Center;
            __instance.radius = __saveData.Radius;
        }
    }

    public class SphereColliderSaveData : ColliderSaveData
    {
        public Vector3 Center;
        public float Radius;
    }
}
