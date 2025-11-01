using System;
using System.IO;
using UnityEditor;
using UnityEngine;



[InitializeOnLoad]
public static class CodegenWatcher
{
    private static FileSystemWatcher watcher;
    private static DateTime lastEventTime;

    static CodegenWatcher()
    {
        EnsureWatcher();
        AppDomain.CurrentDomain.DomainUnload += (_, __) => DisposeWatcher();
    }

    private static void EnsureWatcher()
    {
        if (watcher != null) return;

        // Watch your Scripts folder (change as needed)
        string path = Path.Combine(Application.dataPath/*, "Scripts"*/);
        if (!Directory.Exists(path)) return;

        watcher = new FileSystemWatcher(path, "*.cs")
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
        };

        watcher.Changed += (s, e) => SaveAndLoadCodeGenWindow._eventQueue.Enqueue(e.ToDto()); ;
        watcher.Created += (s, e) => SaveAndLoadCodeGenWindow._eventQueue.Enqueue(e.ToDto()); ;
        watcher.Deleted += (s, e) => SaveAndLoadCodeGenWindow._eventQueue.Enqueue(e.ToDto()); ;
        //watcher.Renamed += OnRenamed;

        //Debug.Log("[CodegenWatcher] Watching " + path);
    }


    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        // Debounce rapid events
        //if ((DateTime.Now - lastEventTime).TotalMilliseconds < 200)
        //{
        //Debug.LogError($"[CodegenWatcher] Duplicate event: {e.ChangeType}: {e.FullPath}");
            
        //    return;
        //}

        lastEventTime = DateTime.Now;

        // Run your codegen logic
        //Debug.Log($"[CodegenWatcher] {e.ChangeType}: {e.FullPath}");


    }

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        Debug.Log($"[CodegenWatcher] Renamed: {e.OldFullPath} -> {e.FullPath}");
        RunCodegen(e.FullPath);
    }

    private static void RunCodegen(string changedFile)
    {
    }

    private static void DisposeWatcher()
    {
        if (watcher != null)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            watcher = null;
            //Debug.Log("[CodegenWatcher] Disposed");
        }
    }
}
