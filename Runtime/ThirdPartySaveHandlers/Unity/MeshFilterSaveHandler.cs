using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnitySHs;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers
{
    [SaveHandler(id: 442699242848637806, handledType: typeof(MeshFilter), order: -8)]
    public class MeshFilterSaveHandler : MonoSaveHandler<MeshFilter, MeshFilterSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            //note: order is important, first .mesh, then .sharedMesh
            __saveData.sharedMesh = GetObjectId(__instance.sharedMesh);
            //if(HandledObjectId.ToString() == "491339562158449630")
            //{
            //    Debug.Log(AssetIdMap.IsMutable(__saveData.sharedMesh));
            //}
            if (AssetIdMap.IsMutable(__saveData.sharedMesh))
            {
                __saveData.mesh = GetObjectId(__instance.mesh);
                __saveData.sharedMesh = GetObjectId(__instance.sharedMesh);
            }

            AssetIdMap.ObjectIdToAssetId.TryGetValue(__saveData.sharedMesh, out __saveData.sharedMeshAssetid);
            AssetIdMap.ObjectIdToAssetId.TryGetValue(__saveData.mesh, out __saveData.meshAssetId);
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            if (__saveData.mesh.IsNotDefault)
            {
                __instance.mesh = GetObjectById<Mesh>(__saveData.mesh);

                var initContext = new MeshInitContext { initFromMesh = __saveData.mesh };

                Infra.S.Register(__instance.mesh, context: initContext);
                Infra.S.Register(__instance.sharedMesh, context: initContext, ifHasntAlready:true);
            }
            else
            {
                __instance.sharedMesh = GetObjectById<Mesh>(__saveData.sharedMesh);
            }

            __instance.hideFlags = __saveData.hideFlags;




            //doc:
            //multiple components can use the same asset, on load, the first one creates and registers the asset, the rest gets the reference and returns early
            //important note: the code is prepared for these three scenarios
            //.sharedMesh and .mesh are both null -> nothing happens
            //.sharedMesh has value and .mesh is null -> setting only .sharedMesh
            //.sharedMesh has value and .mesh has value -> we expect both of them to refer to the same object as accessing the .mesh property replaces .sharedMesh too with the same instance
            //
            //true for all cases: lookup the asset it originates from if it has one, or create a new bare instance


            //the below logic is copied to all savehandlers who have similar xy/sharedxy semantics
            //logic id: 39dfz8d7ghbn3jh423lj

            //            if (AssetIdMap.HasInstance(__saveData.sharedMesh, out Mesh asset))
            //            {
            //                __instance.sharedMesh = asset;

            //                if (__saveData.mesh.IsNotDefault)
            //                {
            //#if UNITY_EDITOR
            //                    AssetIdMap.LogSharedPrivateCopiesError(HandledObjectId, __saveData.sharedMesh, __saveData.mesh);
            //#else
            //                    __instance.mesh = asset;
            //#endif
            //                }
            //            }
            //            else if (__saveData.sharedMeshAssetid.IsNotDefault)
            //            {
            //                var assetId = __saveData.meshAssetId;

            //                var orig = GetAssetById2<Mesh>(assetId, null);

            //                if (orig != null)
            //                {
            //                    var copy = Object.Instantiate(orig); //in editor-time assets returned by asset providers are pointing to the original asset, copy to protect accidental editor-time modifications

            //                    __instance.sharedMesh = copy;

            //                    if (__saveData.meshAssetId.IsNotDefault)
            //                    {
            //                        copy = __instance.mesh; ///trigger unity's copy mechanism by accessing the .mesh property, which sets .sharedMesh to the newly copied instance too
            //                    }

            //                    AssetIdMap.AddInstance(__saveData.sharedMesh, copy);
            //                }
            //                else
            //                {
            //                    Debug.LogError($"Object: {__saveData._ObjectId_} had an assetid: {assetId} but no asset was found with such id.");
            //                }
            //            }
            //            else if (__saveData.sharedMesh.IsNotDefault)
            //            {
            //                var mesh = new Mesh();

            //                __instance.sharedMesh = mesh;

            //                if (__saveData.mesh.IsNotDefault)
            //                    mesh = __instance.mesh;

            //                AssetIdMap.AddInstance(__saveData.sharedMesh, mesh);
            //            }

        }
    }

    public class MeshFilterSaveData : MonoSaveDataBase
    {
        public RandomId sharedMesh;
        public RandomId mesh;
        public RandomId sharedMeshAssetid;
        public RandomId meshAssetId;
        public UnityEngine.HideFlags hideFlags;
    }
}