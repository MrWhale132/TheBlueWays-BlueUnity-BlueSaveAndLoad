using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Packages.com.theblueway.saveandload.Editor.SaveAndLoad.HandledTypeNameSearchFeature
{
    public class SearchableStringPopup<T> : EditorWindow where T : class
    {
        private List<string> cachedStringRepresentation;
        private List<T> allOptions;
        private List<int> filteredOptions;
        private List<int> cachedAllOptionIndexes;
        private string search = "";
        private string prevSearch = "";
        private Vector2 scroll;
        private Action<T> onSelected;
        private T currentValue;
        private Func<T, string> toString;

        public static void Open(
            List<T> options,
            T currentValue,
            Action<T> onSelected)
        {
            var win = CreateInstance<SearchableStringPopup<T>>();
            win.allOptions = options;
            win.cachedAllOptionIndexes = new();
            for (int i = 0; i < options.Count(); i++) win.cachedAllOptionIndexes.Add(i);
            win.filteredOptions = win.cachedAllOptionIndexes.ToList(); //immutability
            win.currentValue = currentValue;
            win.onSelected = onSelected;
            win.toString = element => element.ToString();

            win.cachedStringRepresentation = win.allOptions.Select(option => win.toString(option)).ToList();

            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            win.ShowAsDropDown(new Rect(mousePos, Vector2.zero), new Vector2(1000, 300));
        }


        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);

            if (EditorGUI.EndChangeCheck())
            {
                if (search != prevSearch)
                {
                    if (string.IsNullOrEmpty(search))
                        filteredOptions = cachedAllOptionIndexes.ToList();
                    else
                    {
                        filteredOptions.Clear();

                        int i = 0;
                        foreach (var option in allOptions)
                        {
                            string stringValue = cachedStringRepresentation[i];

                            if (stringValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                filteredOptions.Add(i);
                            }
                            i++;
                        }
                    }
                    //filteredOptions = string.IsNullOrEmpty(search)
                    //    ? allOptions
                    //    : allOptions
                    //        .Where(o => o.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (int index in filteredOptions)
            {
                var option = allOptions[index];
                bool isCurrent = option == currentValue;
                GUIStyle style = isCurrent ? EditorStyles.boldLabel : EditorStyles.label;

                if (GUILayout.Button(toString(option), style))
                {
                    onSelected?.Invoke(option);
                    Close();
                }
            }

            prevSearch = search;

            EditorGUILayout.EndScrollView();
        }
    }




    public class SearchableStringPopup : EditorWindow
    {
        private IEnumerable<string> allOptions;
        private IEnumerable<string> filteredOptions;
        private string search = "";
        private Vector2 scroll;
        private Action<string> onSelected;
        private string currentValue;

        public static void Open(
            IEnumerable<string> options,
            string currentValue,
            Action<string> onSelected)
        {
            var win = CreateInstance<SearchableStringPopup>();
            win.allOptions = options;
            win.filteredOptions = options;
            win.currentValue = currentValue;
            win.onSelected = onSelected;

            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            win.ShowAsDropDown(new Rect(mousePos, Vector2.zero), new Vector2(1000, 300));
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);
            if (EditorGUI.EndChangeCheck())
            {
                filteredOptions = string.IsNullOrEmpty(search)
                    ? allOptions
                    : allOptions
                        .Where(o => o.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var option in filteredOptions)
            {
                bool isCurrent = option == currentValue;
                GUIStyle style = isCurrent ? EditorStyles.boldLabel : EditorStyles.label;

                if (GUILayout.Button(option, style))
                {
                    onSelected?.Invoke(option);
                    Close();
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
