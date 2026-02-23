
using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts.Misc
{
    [CreateAssetMenu(fileName = "RandomIdGenerator", menuName = "Scriptable Objects/Infra/RandomIdGenerator", order = 0)]
    public class RandomIdGenerator:ScriptableObject
    {
        public string output;

        public void Generate()
        {
            output = RandomId.Get().ToString();
        }
    }
}
