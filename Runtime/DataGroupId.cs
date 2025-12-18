using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [CreateAssetMenu(fileName = "DataGroupId", menuName = "Scriptable Objects/SaveAndLoad/DataGroupId", order = 1)]
    public class DataGroupId : ScriptableObject
    {
        public string dataGroupId;
    }
}
