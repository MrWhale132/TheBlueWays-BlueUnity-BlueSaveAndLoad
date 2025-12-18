using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System.Reflection;
using UnityEngine;

namespace DevTest
{
    [SaveHandler(737179718849734247, "WaitForSeconds", typeof(UnityEngine.WaitForSeconds))]
	public class WaitForSecondsSaveHandler : UnmanagedSaveHandler<UnityEngine.WaitForSeconds, WaitForSecondsSaveData> 
	{
		public static FieldInfo m_Seconds;

        public override void WriteSaveData()
		{
			base.WriteSaveData();

			__saveData.m_Seconds = (float)m_Seconds.GetValue(__instance);
		}

		public override void _AssignInstance()
		{
			__instance = new WaitForSeconds(__saveData.m_Seconds);
		}

        static WaitForSecondsSaveHandler()
        {
            m_Seconds = typeof(WaitForSeconds).GetField("m_Seconds", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }


	public class WaitForSecondsSaveData : SaveDataBase 
	{
		public float m_Seconds;

    }


}