using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers
{
    [SaveHandler(id: 442699242848637806, handledType: typeof(MeshFilter), order: -9)]
    public class MeshFilterSaveHandler : MonoSaveHandler<MeshFilter, MeshFilterSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            //note: order is important, first .mesh, then .sharedMesh
            __saveData.mesh = GetObjectId(__instance.mesh);
            __saveData.sharedMesh = GetObjectId(__instance.sharedMesh);
            AssetIdMap.ObjectIdToAssetId.TryGetValue(__saveData.sharedMesh, out __saveData.sharedMeshAssetid);
            AssetIdMap.ObjectIdToAssetId.TryGetValue(__saveData.mesh, out __saveData.meshAssetId);
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            //doc:
            //multiple components can use the same asset, on load, the first one creates and registers the asset, the rest gets the reference and returns early
            //important note: the code is prepared for these three scenarios
            //.sharedMesh and .mesh are both null -> nothing happens
            //.sharedMesh has value and .mesh is null -> setting only .sharedMesh
            //.sharedMesh has value and .mesh has value -> we expect both of them to refer to the same object as accessing the .mesh property replaces .sharedMesh too with the same instance
            //
            //true for all cases: lookup the asset it originates from if it has one, or create a new bare instance


            //the below logic is copied to all savehandlers who have similar xy/sharedxy semantics
            //logic id: 39dfz8d7ghbn3jh423lj
            if (AssetIdMap.HasInstance(__saveData.sharedMesh, out Mesh asset))
            {
                __instance.sharedMesh = asset;

                if (__saveData.mesh.IsNotDefault)
                {
#if UNITY_EDITOR
                    AssetIdMap.LogSharedPrivateCopiesError(HandledObjectId, __saveData.sharedMesh, __saveData.mesh);
#else
                    __instance.mesh = asset;
#endif
                }
            }
            else if (__saveData.sharedMeshAssetid.IsNotDefault)
            {
                var assetId = __saveData.meshAssetId;

                var orig = GetAssetById2<Mesh>(assetId, null);

                if (orig != null)
                {
                    var copy = Object.Instantiate(orig); //in editor-time assets returned by asset providers are pointing to the original asset, copy to protect accidental editor-time modifications

                    __instance.sharedMesh = copy;

                    if (__saveData.meshAssetId.IsNotDefault)
                    {
                        copy = __instance.mesh; ///trigger unity's copy mechanism by accessing the .mesh property, which sets .sharedMesh to the newly copied instance too
                    }

                    AssetIdMap.AddInstance(__saveData.sharedMesh, copy);
                }
                else
                {
                    Debug.LogError($"Object: {__saveData._ObjectId_} had an assetid: {assetId} but no asset was found with such id.");
                }
            }
            else if (__saveData.sharedMesh.IsNotDefault)
            {
                var mesh = new Mesh();

                __instance.sharedMesh = mesh;

                if (__saveData.mesh.IsNotDefault)
                    mesh = __instance.mesh;

                AssetIdMap.AddInstance(__saveData.sharedMesh, mesh);
            }


            __instance.hideFlags = __saveData.hideFlags;
        }
    }

    public class MeshFilterSaveData : MonoSaveDataBase
    {
        public RandomId sharedMesh;
        public RandomId mesh;
        public RandomId sharedMeshAssetid;
        public RandomId meshAssetId;
        public UnityEngine.HideFlags hideFlags;
    }
}



//here ai might just halucinated, the second section contradicts what is said in the first section (right below)


//```
//AssetMesh(shared)
//InstanceMesh(optional clone)
//```

//Unity exposes:

//```
//sharedMesh  → AssetMesh
//mesh        → InstanceMesh (or clone of AssetMesh)
//```

//Important invariant:

//> If InstanceMesh exists, `mesh` returns it.
//> If not, `mesh` creates it from sharedMesh.

//---

//# Case 1 — Setting `.sharedMesh`

//```csharp
//renderer.sharedMesh = A;
//```

//Effect:

//```
//AssetMesh = A
//InstanceMesh = null(any previous instance is discarded)
//```

//No cloning.

//Later:

//```csharp
//var m = renderer.mesh;
//```

//→ Unity clones `A` into InstanceMesh.

//---

//# Case 2 — Setting `.mesh`

//```csharp
//renderer.mesh = B;
//```

//Unity does:

//```
//AssetMesh = B
//InstanceMesh = Clone(B)
//```

//Yes — **both are affected**.

//This is the surprising part.

//Setting `.mesh` implicitly sets `.sharedMesh` as well.

//Reason: Unity must know which asset the instance originated from.

//---

//# Case 3 — Set `.mesh` first, then `.sharedMesh`

//```csharp
//renderer.mesh = B;
//renderer.sharedMesh = A;
//```

//Step by step:

//After first line:

//```
//AssetMesh = B
//InstanceMesh = Clone(B)
//```

//After second line:

//```
//AssetMesh = A
//InstanceMesh = null   // instance is destroyed
//```

//So the instance created by `.mesh = B` is **thrown away**.

//Next access to `.mesh` will clone `A`.

//---

//# Case 4 — Set `.sharedMesh` first, then `.mesh`

//```csharp
//renderer.sharedMesh = A;
//renderer.mesh = B;
//```

//After first:

//```
//AssetMesh = A
//InstanceMesh = null
//```

//After second:

//```
//AssetMesh = B
//InstanceMesh = Clone(B)
//```

//So final state uses `B`.

//---

//# Truth table

//| Operation                | AssetMesh | InstanceMesh |
//| ------------------------ | --------- | ------------ |
//| Set sharedMesh = A       | A         | null         |
//| Get mesh                 | A         | Clone(A)     |
//| Set mesh = B             | B         | Clone(B)     |
//| Set mesh then sharedMesh | A         | null         |
//| Set sharedMesh then mesh | B         | Clone(B)     |

//---

//# Core rule

//> `.mesh` always creates an instance and also overwrites `.sharedMesh`.

//> `.sharedMesh` never creates an instance and always destroys existing instance.

//---

//# Common pitfall

//```csharp
//renderer.mesh = myMesh;
//renderer.sharedMesh = myMesh;
//```

//This:

//*Creates clone
//* Immediately destroys clone

//Wasted work.

//---

//# Recommended patterns

//# Analogy

//Think:

//```
//sharedMesh = "which blueprint"
//mesh = "working copy of blueprint"
//```

//Setting working copy also changes which blueprint it came from.

//Setting blueprint throws away any working copy.

//---








/*

            Debug.Log(__instance.sharedMesh == __instance.mesh); -> false
            Debug.Log(__instance.sharedMesh == __instance.mesh); -> true


* `sharedMesh` → asset
* `mesh` → clone of asset

So:

```
sharedMesh == mesh → false
```

Always.

But Unity does something sneakier.

---

# Key hidden behavior

> When you access `renderer.mesh`, Unity **replaces the renderer’s sharedMesh reference with the instance mesh**.

Yes. It mutates internal state.

So after first access:

```
sharedMesh → instance mesh
mesh       → same instance mesh
```

Not:

```
sharedMesh → asset
mesh       → instance
```

---

# Step-by-step timeline

Assume initial state:

```
sharedMesh = AssetMesh
instanceMesh = null
```

---

### First line

```csharp
__instance.sharedMesh == __instance.mesh
```

Evaluation order:

1. Evaluate left side:

```
sharedMesh → AssetMesh
```

2. Evaluate right side:

```
mesh getter called
→ Unity clones AssetMesh
→ instanceMesh = Clone(AssetMesh)
→ returns instanceMesh
```

Comparison:

```
AssetMesh == InstanceMesh → false
```

First log = false

---

### After that call, internal state is now:

```
sharedMesh = InstanceMesh   (important!)
instanceMesh = InstanceMesh
```

Unity **overwrites sharedMesh**.

---

### Second line

```csharp
__instance.sharedMesh == __instance.mesh
```

1. Left side:

```
sharedMesh → InstanceMesh
```

2. Right side:

```
mesh → returns existing InstanceMesh
```

Comparison:

```
InstanceMesh == InstanceMesh → true
```

Second log = true

---

# Why Unity does this

Because Unity wants this invariant:

> After instancing, `sharedMesh` and `mesh` refer to the same underlying object.

So future systems don’t have to branch.

This also mirrors how `material` behaves.

---

# ⚠️ Consequences

1. You permanently lose the original asset reference at renderer level
2. Identity is destroyed
3. You cannot recover the asset from renderer anymore
4. This happens silently

---

# Correct mental model

Think of `.mesh` as:

> “Convert renderer into instance-mesh mode.”

Once done:

```
sharedMesh becomes meaningless for asset identity
```

---

# Safe rules

### If you need asset identity

Never touch:

```csharp
renderer.mesh
```

Only use:

```csharp
renderer.sharedMesh
```

And only before any `.mesh` access.

---

### If you need instance mesh

Call `.mesh` once intentionally and accept that identity is gone.

---

# Same rule applies to materials

```csharp
renderer.material
```

Behaves the same way.

So:

> First comparison = asset vs instance
> Subsequent comparisons = instance vs instance

 */