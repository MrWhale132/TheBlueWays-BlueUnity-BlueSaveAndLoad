#if UNITY_EDITOR
using UnityEditor;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;


namespace Assets._Project.Scripts
{
    [InitializeOnLoad]
    public static class EditorJsonSettingsInitializer
    {
        static EditorJsonSettingsInitializer()
        {
            RegisterAllConverters();
        }

        /// <summary>
        /// duplicate logic from <see cref="Assets._Project.Scripts.Infrastructure.Infra.CreateDefaultJsonSerializerSettings"/>
        /// </summary>
        public static void RegisterAllConverters()
        {
            var converters = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes())
                .Where(t => !t.IsAbstract && typeof(JsonConverter).IsAssignableFrom(t))
                .Select(t => (JsonConverter)Activator.CreateInstance(t))
                .ToList();

            //UnityEngine.Debug.Log($"[Editor] Found {converters.Count} JsonConverters in the assembly.");

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = converters,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
            };
        }
    }
}
#endif
