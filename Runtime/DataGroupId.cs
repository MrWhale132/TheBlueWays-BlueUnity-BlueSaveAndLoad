using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [CreateAssetMenu(fileName = "DataGroupId", menuName = "ScriptableObjects/SaveAndLoad/DataGroupId", order = 1)]
    public class DataGroupId : ScriptableObject
    {
        public string dataGroupId;

        //this commented block is from an other script that has nothing to do with this one
        //I saved it here to keep it for reference for future use
//        public RandomId id;
//        public string forEasyReading;

//        public void Awake()
//        {
//            forEasyReading = id.ToString();

//            if (id.IsDefault)
//            {
//                id = RandomId.Get();
//#if UNITY_EDITOR
//                UnityEditor.EditorUtility.SetDirty(this);
//                UnityEditor.AssetDatabase.SaveAssets();
//#endif
//            }
//        }
    }
}
