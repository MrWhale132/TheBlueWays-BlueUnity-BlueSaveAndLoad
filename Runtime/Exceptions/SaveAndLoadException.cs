
using UnityEngine.Scripting;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.Exceptions
{
    [Preserve]
    public class SaveAndLoadException : System.Exception
    {
        public SaveAndLoadException(string message):base(message)
        {
            
        }
    }
}
