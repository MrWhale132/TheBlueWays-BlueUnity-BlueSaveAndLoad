
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.UnityEngineSHs
{

    public class StaticCursorSubtitute : StaticSubtitute<Cursor> { }

    [SaveHandler(88897439587234832, nameof(StaticCursorSubtitute), typeof(StaticCursorSubtitute), isStatic: true)]
    public class StaticCursorSaveHandler : StaticSaveHandlerBase<StaticCursorSubtitute, StaticCursorSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.visible = Cursor.visible;
            __saveData.lockState = Cursor.lockState;
        }
        public override void LoadReferences()
        {
            base.LoadReferences();
            Cursor.visible = __saveData.visible;
            Cursor.lockState = __saveData.lockState;
        }
    }

    public class StaticCursorSaveData : StaticSaveDataBase
    {
        public bool visible;
        public CursorLockMode lockState;
    }
}
