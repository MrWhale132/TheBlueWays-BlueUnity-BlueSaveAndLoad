
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers
{
    [SaveHandler(id: 442699242848637806, dataGroupName: nameof(MeshFilter), typeof(MeshFilter), order: -8)]
    public class MeshFilterSaveHandler:MonoSaveHandler<MeshFilter, MeshFilterSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.sharedMesh = GetObjectId(__instance.sharedMesh);
            __saveData.mesh = GetObjectId(__instance.mesh);
            __saveData.hideFlags = __instance.hideFlags;
        }
        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.sharedMesh = GetObjectById<Mesh>(__saveData.sharedMesh);
            __instance.mesh = GetObjectById<Mesh>(__saveData.mesh);
            __instance.hideFlags = __saveData.hideFlags;
        }
    }

    public class MeshFilterSaveData : MonoSaveDataBase
    {
        public RandomId sharedMesh;
        public RandomId mesh;
        public UnityEngine.HideFlags hideFlags;
    }
}
