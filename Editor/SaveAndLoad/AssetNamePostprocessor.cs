using Assets._Project.Scripts.Infrastructure;
using UnityEditor;
using UnityEngine;
//https://discussions.unity.com/t/trying-to-add-new-data-to-fbx-imports-is-absolutely-miserable/906116/6

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Editor.SaveAndLoad
{
    class AssetNamePostprocessor : AssetPostprocessor
    {
        void OnPostprocessModel(GameObject g)
        {
            // Access imported assets through the importer
            var importer = assetImporter as ModelImporter;
            if (importer == null)
                return;
            Debug.Log($"Post-processing model: {g.name} at: {importer.assetPath}");

                var infra = g.AddComponent<GOInfra>();
            infra.AddInfraToAllChildren();

            if (g.name == "Column01")
            {
                //Debug.Log(g.scene.name);
                //return;
                //// Get all sub-assets created by the importer
                //var objs = AssetDatabase.LoadAllAssetsAtPath(importer.assetPath);
                //objs = AssetDatabase.LoadAllAssetRepresentationsAtPath(importer.assetPath);
                //objs = g.GetComponentsInChildren<Component>();

                //Debug.Log("Found sub-assets: " + objs.Length);
                //foreach (var obj in objs)
                //{
                //    Debug.Log($"Processing sub-asset: {obj.name} of type {obj.GetType()}");
                //    if (obj is MeshFilter meshfilter)
                //    {
                //        var mesh = meshfilter.sharedMesh;

                //        // Example: prefix meshes with the model name
                //        //if(mesh.name.StartsWith("NEW_"))
                //        //    continue;
                //        //mesh.name = $"NEW_{mesh.name}";
                //        //EditorUtility.SetDirty(mesh);
                //        Debug.Log($"Renamed mesh to: {mesh.name}");
                //    }
                //    else if (obj is Animation anim)
                //    {
                //        Debug.Log($"Animation: {anim.name}");
                //    }
                //}
            }

            Debug.Log("Finished post-processing model.");
        }

        private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
        {
            Debug.Log($"Post-processing animation clip: {clip.name} in root: {root.name}");

            if(clip.name =="Take 001")
            {

                // Example: rename the clip
                clip.name = clip.name+"_"+root.name;
                EditorUtility.SetDirty(clip);
                Debug.Log($"Renamed animation clip to: {clip.name}");
            }
        }

        //private void OnPreprocessAsset()
        //{
        //    Debug.Log($"Preprocessing asset at path: {assetImporter.assetPath}");
        //}
    }
}