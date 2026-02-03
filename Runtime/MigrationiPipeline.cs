
using Assets._Project.Scripts.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime
{
    public class MigrationiPipeline
    {
        public Dictionary<int, Func<object, object, MigrationContext, object>> _migrationsPerAppVersion;


        public static MigrationiPipeline From(Type migrationContainer)
        {
            IEnumerable<MethodInfo> methods = migrationContainer.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
                                                .Where(m => m.IsDefined(typeof(MigrationAttribute)));


            Dictionary<int, Func<object, object, MigrationContext, object>> migrations = new();

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<MigrationAttribute>();

                Delegate del = Delegate.CreateDelegate(typeof(Func<object, object, MigrationContext, object>), method.IsStatic ? null : Activator.CreateInstance(migrationContainer), method);

                Func<object, object, MigrationContext, object> migration = (Func<object, object, MigrationContext, object>)del;

                migrations.Add(attr.AppVersion, migration);
            }

            return new MigrationiPipeline() { _migrationsPerAppVersion = migrations };
        }


        public object Migrate(object data, object current, int toVersion, MigrationContext context, out bool didMigrate)
        {
            if (_migrationsPerAppVersion.TryGetValue(toVersion, out var migration))
            {
                var migrated = migration(data, current, context);
                didMigrate = true;
                return migrated;
            }
            else
            {
                didMigrate = false;
                return current;
            }
        }
    }
}
