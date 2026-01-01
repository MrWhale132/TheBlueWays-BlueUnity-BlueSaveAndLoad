
using UnityEngine;

namespace Packages.com.theblueway.saveandload.Samples
{
    [DefaultExecutionOrder(100)]
    public class BootstrapManager:MonoBehaviour
    {
        public void Start()
        {
            MySceneManager.Singleton.OnBootstrapCompleted();
        }
    }
}
