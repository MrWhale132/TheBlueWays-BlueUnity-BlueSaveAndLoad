using Assets._Project.Scripts.SaveAndLoad;
using UnityEngine;
namespace DevTest
{
    [CustomSaveData(GenerationMode =SaveHandlerGenerationMode.Manual)]
	public class BoundsSaveData : CustomSaveData<Bounds> 
	{
		public override void ReadFrom(in Bounds instance)
		{
		}
		public override void WriteTo(ref Bounds instance)
		{
		}
	}
}