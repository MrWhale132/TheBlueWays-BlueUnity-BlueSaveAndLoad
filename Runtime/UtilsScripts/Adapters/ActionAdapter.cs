
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using System;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.UtilsScripts.Adapters
{

    public class ActionAdapter<T>
    {
        public Action<T> Action { get; set; }
        //public List<Action<T>> Events { get; set; }

        //injected hooks
        public Action<Action<T>> Add;
        public Action<Action<T>> Remove;


        public ActionAdapter()
        {
            //Events = new();
        }

        public ActionAdapter(Action<Action<T>> add, Action<Action<T>> remove) : this()
        {
            Add = add; Remove = remove;
        }

        public void Invoke(T arg)
        {
            Action?.Invoke(arg);
        }

        public static ActionAdapter<T> operator +(ActionAdapter<T> adapter, Action<T> del)
        {
            adapter.Add?.Invoke(del);
            adapter.Action += del;
            //adapter.Events.Add(del);
            return adapter;
        }
        public static ActionAdapter<T> operator -(ActionAdapter<T> adapter, Action<T> del)
        {
            adapter.Remove?.Invoke(del);
            adapter.Action -= del;
            //adapter.Events.Remove(del);
            return adapter;
        }
    }

    [SaveHandler(242423434529384, "ActionAdapter`1", typeof(ActionAdapter<>))]
    public class ActionAdapterSaveHandler<T> : UnmanagedSaveHandler<ActionAdapter<T>, ActionAdapterSaveData<T>>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Add = Infra.Singleton.GetInvocationList(__instance.Add);
            __saveData.Remove = Infra.Singleton.GetInvocationList(__instance.Remove);
            __saveData.Action = Infra.Singleton.GetInvocationList(__instance.Action);
            //__saveData.Events.Clear();
            //foreach(var action in __instance.Events)
            //{
            //    __saveData.Events.Add(Infra.Singleton.GetDelegateSaveInfo(action));
            //}
        }
        public override void LoadValues()
        {
            base.LoadValues();

            __instance.Add = Infra.Singleton.GetDelegate<Action<Action<T>>>(__saveData.Add);
            __instance.Remove = Infra.Singleton.GetDelegate<Action<Action<T>>>(__saveData.Remove);
            __instance.Action = Infra.Singleton.GetDelegate<Action<T>>(__saveData.Action);

            //__instance.Events = new(capacity: __saveData.Events.Count);
            //foreach(var info in __saveData.Events)
            //{
            //    __instance.Events.Add(Infra.Singleton.GetDelegate<Action<T>>(info));
            //}
        }
    }

    public class ActionAdapterSaveData<T> : SaveDataBase
    {
        public InvocationList Action = new();
        public InvocationList Add = new();
        public InvocationList Remove = new();
    }
}
