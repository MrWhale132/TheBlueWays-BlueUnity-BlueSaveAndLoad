
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Colliders
{
    //[SaveHandler(342398654961,nameof(BoxCollider),typeof(BoxCollider))]
    public class BoxColliderSaveHandler: ColliderSavehandler<BoxCollider, BoxColliderSaveData>
    {
        public BoxColliderSaveHandler()
        {
            
        }


        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Center = __instance.center;
            __saveData.Size = __instance.size;
        }


        public override void LoadValues()
        {
            base.LoadValues();

            __instance.center = __saveData.Center;
            __instance.size = __saveData.Size;
        }
    }

    public class BoxColliderSaveData : ColliderSaveData
    {
        public Vector3 Center;
        public Vector3 Size;
    }
}
