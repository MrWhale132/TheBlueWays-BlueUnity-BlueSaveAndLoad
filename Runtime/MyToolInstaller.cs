#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
//todo: this should be in an Editor folder
namespace Assets._Project.Scripts.SaveAndLoad
{
    //not using it now, but leaving it here for future reference. Aslo extend this a little bit to check for updates or if it is already copied.
    //[InitializeOnLoad]
    public static class MyToolInstaller
    {
        static MyToolInstaller()
        {
            const string dest = "Assets/MyTool/";
            const string src = "Packages/com.blue.codegen/";

            if (!AssetDatabase.IsValidFolder(dest))
            {
                FileUtil.CopyFileOrDirectory(src, dest);
                AssetDatabase.Refresh();
                Debug.Log("MyTool installed to Assets/MyTool/");
            }
        }
    }

}
#endif
