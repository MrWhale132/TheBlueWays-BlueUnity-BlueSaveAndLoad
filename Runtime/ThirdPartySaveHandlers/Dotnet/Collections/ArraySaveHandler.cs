using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Dotnet.Collections
{
    [SaveHandler(897213743298234, "Array1d", typeof(Array), arrayDimension: 1)]
    public class ArraySaveHandler<TElement> : UnmanagedSaveHandler<TElement[], ArraySaveData<TElement>>
    {
        public override void Init(object instance)
        {
            base.Init(instance);

            __saveData.elements = new Data<TElement>[__instance.Length];

            for (int i = 0; i < __instance.Length; i++)
            {
                __saveData.elements[i] = new Data<TElement>
                {
                    ReferencedBy = HandledObjectId,
                };
            }
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();

            for (int i = 0; i < __saveData.elements.Length; i++)
            {
                __saveData.elements[i].Value = __instance[i];
            }
        }


        public override void _AssignInstance()
        {
            __instance = new TElement[__saveData.elements.Length];
        }

        public override void LoadPhase1()
        {
            base.LoadPhase1();


            for (int i = 0; i < __saveData.elements.Length; i++)
            {
                __instance[i] = __saveData.elements[i].Value;
            }
        }
    }


    public class ArraySaveData<T> : SaveDataBase
    {
        public Data<T>[] elements;
    }



    //maybe we can use this if somewhy we would need to save Array type
    //public class ArraySaveHandler : UnmanagedSaveHandler<Array, ArraySaveData>
    //{
    //    public ArraySaveHandler()
    //    {

    //    }

    //    public Array __arrayToSave;

    //    public bool __elementTypeHasSaveHandler;


    //    public override void Init(object instance)
    //    {
    //        base.Init(instance);
    //        Type elementType = instance.GetType().GetElementType();

    //        __elementTypeHasSaveHandler = SaveAndLoadManager.Singleton.HasSaveHandlerForType(elementType);

    //        __saveData.ElementTypeHasSaveHandler = __elementTypeHasSaveHandler;


    //        if (__elementTypeHasSaveHandler)
    //        {
    //            __arrayToSave = CreateWithStructure(__instance, typeof(RandomId));
    //        }
    //        else
    //        {
    //            __arrayToSave = __instance;
    //        }
    //    }



    //    public static Array CreateWithStructure(Array blueprint, Type elementType = null)
    //    {
    //        if (blueprint == null)
    //            throw new ArgumentNullException(nameof(blueprint));


    //        int[] lengths = GetArrayLengths(blueprint);

    //        elementType = elementType ?? blueprint.GetType().GetElementType();

    //        Array clone = Array.CreateInstance(elementType, lengths);

    //        return clone;
    //    }



    //    public static Array CloneRectangularArray(Array source)
    //    {
    //        if (source == null)
    //            throw new ArgumentNullException(nameof(source));

    //        int[] lengths = GetArrayLengths(source);

    //        Array clone = Array.CreateInstance(source.GetType().GetElementType(), lengths);

    //        int[] indices = new int[source.Rank];
    //        CopyRecursive(source, clone, indices, 0);

    //        return clone;
    //    }

    //    public static int[] GetArrayLengths(Array array)
    //    {

    //        int rank = array.Rank;
    //        int[] lengths = new int[rank];
    //        for (int i = 0; i < rank; i++)
    //            lengths[i] = array.GetLength(i);

    //        return lengths;
    //    }



    //    private static void CopyRecursive(Array source, Array target, int[] indices, int dimension)
    //    {
    //        int length = source.GetLength(dimension);
    //        for (int i = 0; i < length; i++)
    //        {
    //            indices[dimension] = i;
    //            if (dimension < source.Rank - 1)
    //            {
    //                CopyRecursive(source, target, indices, dimension + 1);
    //            }
    //            else
    //            {
    //                object value = source.GetValue(indices);
    //                target.SetValue(value, indices);
    //            }
    //        }
    //    }


    //    public static void SetArrayValues(Array array, Action<Array, int[]> valueSetter)
    //    {
    //        SetRecursive(array, valueSetter, new int[array.Rank], 0);
    //    }

    //    private static void SetRecursive(Array source, Action<Array, int[]> valueSetter, int[] indices, int dimension)
    //    {
    //        int length = source.GetLength(dimension);
    //        for (int i = 0; i < length; i++)
    //        {
    //            indices[dimension] = i;
    //            if (dimension < source.Rank - 1)
    //            {
    //                SetRecursive(source, valueSetter, indices, dimension + 1);
    //            }
    //            else
    //            {
    //                valueSetter(source, indices);
    //            }
    //        }
    //    }



    //    public override void WriteSaveData()
    //    {
    //        base.WriteSaveData();


    //        if (__elementTypeHasSaveHandler)
    //        {
    //            Action<Array, int[]> setter = (array, indices) =>
    //            {
    //                RandomId id = Infra.Singleton.GetObjectId(__instance.GetValue(indices), HandledObjectId);


    //                array.SetValue(id, indices);
    //            };

    //            SetArrayValues(__arrayToSave, setter);
    //        }
    //        else
    //        {
    //            ///<see cref="__arrayToSave"/> should be == to <see cref="__instance"/>
    //        }

    //        __saveData.Elements = new JRaw(JsonConvert.SerializeObject(__arrayToSave));
    //    }



    //    public override void _AssignInstance()
    //    {
    //        __elementTypeHasSaveHandler = __saveData.ElementTypeHasSaveHandler;
    //        Type arrayType = Type.GetType(__saveData._AssemblyQualifiedName_);


    //        if (__elementTypeHasSaveHandler)
    //        {
    //            Type elementType = arrayType.GetElementType();

    //            Type idArrayType = typeof(RandomId).MakeArrayType(arrayType.GetArrayRank());
    //            __arrayToSave = (Array)JsonConvert.DeserializeObject(__saveData.Elements.ToString(), idArrayType);

    //            __instance = CreateWithStructure(__arrayToSave, elementType);
    //        }
    //        else
    //        {
    //            __instance = (Array)JsonConvert.DeserializeObject(__saveData.Elements.ToString(), arrayType);
    //        }
    //    }


    //    public override void LoadPhase1()
    //    {
    //        base.LoadPhase1();


    //        if (__elementTypeHasSaveHandler)
    //        {
    //            Action<Array, int[]> setter = (array, indices) =>
    //            {
    //                RandomId id = (RandomId)__arrayToSave.GetValue(indices);
    //                object instance = Infra.Singleton.GetObjectById<object>(id);
    //                array.SetValue(instance, indices);
    //            };

    //            SetArrayValues(__instance, setter);
    //        }
    //    }
    //}


    //public class ArraySaveData : SaveDataBase
    //{
    //    public bool ElementTypeHasSaveHandler;
    //    public JRaw Elements;
    //}
}
