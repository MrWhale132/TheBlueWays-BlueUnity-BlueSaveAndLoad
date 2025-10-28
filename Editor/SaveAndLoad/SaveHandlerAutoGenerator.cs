using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static SaveAndLoadCodeGenWindow;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Unity.Collections;




[CreateAssetMenu(fileName = "SaveHandlerAutoGenerator", menuName = "Scriptable Objects/SaveAndLoad/SaveHandlerAutoGenerator")]
public class SaveHandlerAutoGenerator : ScriptableObject
{
    [NonSerialized]
    public SaveAndLoadManager.Service _saveAndLoadService = new();






    public class CsFileBuilder
    {
        public string GeneratedTypeName;
        public string GeneratedTypeText;
        public string NameSpace;
        public HashSet<string> NameSpaceNames = new();


        public string BuildFile()
        {
            var namespaces = NameSpaceNames.StringJoin(_NewLine);

            string file = _CsFileTemplate
                .Replace(AdditionalNameSpaces, namespaces)
                .Replace(FileNameSpace, NameSpace)
                .Replace(NameSpaceContent, GeneratedTypeText)
                ;

            var indented = Indent(file);
            //Debug.Log(file);
            //Debug.Log(indented);
            return indented;
        }


        public string Indent(string file)
        {
            return CodeGenUtils.Indent(file);
        }




        public string _CsFileTemplate =
        "//auto-generated" + _NewLine +
        AdditionalNameSpaces + _NewLine +
        _NewLine +
        $"namespace {FileNameSpace}" +
        _NewLine +
        "{" +
        _NewLine +
        //SaveHandlerClass +
        //_NewLine +
        //SaveDataClass +
        NameSpaceContent +
        _NewLine +
        "}" +
        "";
    }


    public class CodeGenerationResult
    {
        public CsFileBuilder StaticHandlerInfo;
        public CsFileBuilder StaticSaveDataInfo;
        public CsFileBuilder HandlerInfo;
        public CsFileBuilder SaveDataInfo;
    }



    public CodeGenerationResult GenerateSavingAndLoadingCode(TypeReport typeReport)
    {
        CodeGenerationResult result;

        if (typeReport.ReportedType.IsClass || typeReport.ReportedType.IsInterface)
        {
            result = GenerateSaveHandler(typeReport);
        }
        else
            result = GenerateCustomSaveData(typeReport);

        return result;
    }






    public CodeGenerationResult GenerateCustomSaveData(TypeReport typeReport)
    {
        var result = GenerateStaticSaveHandler(typeReport);

        if (typeReport.ReportedType.IsStatic())
            return result;


        //string nameSpaceContent = _CustomSaveDataTemplate;

        //string csFile = _CsFileTemplate.Replace(NameSpaceContent, nameSpaceContent);


        var templates = new List<string>()
        {
            _CustomSaveDataTemplate,
        };


        string saveDataAccessor = "";
        string instanceAccessor = "instance.";


        var generatedTypes = GenerateCommonCode(typeReport, templates, saveDataAccessor, instanceAccessor,isStatic: false);

        var CustomSaveDataInfo = generatedTypes[0];

        string typeName = FlattenTypeNameIfNested(typeReport.ReportedType);

        CustomSaveDataInfo.GeneratedTypeName = $"{typeName}CustomSaveData";

        result.HandlerInfo = CustomSaveDataInfo;

        return result;
    }




    public CodeGenerationResult GenerateStaticSaveHandler(TypeReport typeReport)
    {
        var result = new CodeGenerationResult();


        bool isStatic = typeReport.ReportedType.IsStatic();

        var staticReport = isStatic ? typeReport : typeReport.StaticReport;

        Type staticType = staticReport.ReportedType;

        //Debug.Log(staticReport.FieldsReport.ValidFields.Count);
        string typeName = FlattenTypeNameIfNested(staticType);

        //todo
        string subtituteClassName = $"Static{typeName}Subtitute";




        //attribute
        string staticDataGroupId = subtituteClassName;

        string handledTypeText = subtituteClassName;

        if (staticType.IsGenericType)
        {
            int genParamCount = staticType.GetGenericArguments().Length;

            staticDataGroupId += $"`{genParamCount}";
            handledTypeText += "<" + new string(',', genParamCount - 1) + ">";
        }

        string generationMode = $"{nameof(SaveHandlerGenerationMode)}.{SaveHandlerGenerationMode.FullAutomata}";

        string staticHandlerOfText = $"staticHandlerOf: typeof({CodeGenUtils.ToTypeDefinitionText(staticType, withNameSpace: true)})";

        var parameters = new List<string> {
        staticHandlerOfText
        };

        string additionalParamList = parameters.Count > 0 ?
            ", " + string.Join(", ", parameters) : "";


        string id = GetOrCreateSaveHandlerId(staticType, isStatic: true);


        string attribute = _SaveHandlerAttributeTemplate
            .Replace(SaveHandlerId, id)
            .Replace(DataGroupId, staticDataGroupId)
            .Replace(HandledType, handledTypeText)
            .Replace(GenerationMode, generationMode)
            .Replace(AdditionalParamList, additionalParamList)
            ;



        var templates = new List<string>()
        {
            _StaticSaveHandlerAndSubtituteTemplate,
            _StaticSaveDataTemplate,
        };

        string saveDataAccessor = nameof(SaveHandlerGenericBase<int, SaveDataBase>.__saveData) + ".";
        string instanceAccessor = CodeGenUtils.ToTypeReferenceText(staticType, withNameSpace: true) + ".";


        var generatedTypes = GenerateCommonCode(staticReport, templates, saveDataAccessor, instanceAccessor, isStatic: true);

        var staticHandlerInfo = generatedTypes[0];
        var staticSaveDataInfo = generatedTypes[1];

        ///todo: this logic is duplicated from <see cref="GenerateCommonCode"/>
        staticHandlerInfo.GeneratedTypeName = $"Static{typeName}SaveHandler";
        staticSaveDataInfo.GeneratedTypeName = $"Static{typeName}SaveData";

        foreach (var builder in generatedTypes)
        {
            builder.GeneratedTypeText = builder.GeneratedTypeText
                .Replace(SaveHandlerAttribute, attribute)
                .Replace(BaseClassName, nameof(StaticSaveHandlerBase<StaticInfraSubtitute, StaticSaveDataBase>))
                .Replace(SaveDataBaseClassName, nameof(StaticSaveDataBase))
                ;
        }



        result.StaticHandlerInfo = staticHandlerInfo;
        result.StaticSaveDataInfo = staticSaveDataInfo;

        return result;
    }




    public string GetOrCreateSaveHandlerId(Type type, bool isStatic)
    {
        var attribute = _saveAndLoadService.GetSaveHandlerAttributeOfType_Editor(type, isStatic);

        if (attribute != null)
            return attribute.Id.ToString();

        //Debug.LogWarning($"Creating new SaveHandlerId for {(isStatic? "static":"")} {type.FullName} because it does not have SaveHandlerAttribute defined.");

        return RandomId.Get().ToString();
    }




    public CodeGenerationResult GenerateSaveHandler(TypeReport typeReport)
    {
        var result = GenerateStaticSaveHandler(typeReport);

        if (typeReport.ReportedType.IsStatic())
            return result;


        Type typeToHandle = typeReport.ReportedType;


        string saveDataBaseClassName;
        string baseClass;

        if (typeof(UnityEngine.Object).IsAssignableFrom(typeToHandle))
        {
            //Gameobject is missing because it's not auto generated, it is handled manually
            if (typeof(Component).IsAssignableFrom(typeToHandle))
            {
                saveDataBaseClassName = nameof(MonoSaveDataBase);
                baseClass = nameof(MonoSaveHandler<Component, MonoSaveDataBase>);
            }
            else if (typeof(ScriptableObject).IsAssignableFrom(typeToHandle))
            {
                saveDataBaseClassName = nameof(SaveDataBase);
                baseClass = nameof(ScriptableSaveHandlerBase<ScriptableObject, SaveDataBase>);
            }
            else ///if <see cref="IsAsset(Type)"/>
            {
                saveDataBaseClassName = nameof(AssetSaveData);
                baseClass = nameof(AssetSaveHandlerBase<UnityEngine.Object, AssetSaveData>);
            }
        }
        else
        {
            saveDataBaseClassName = nameof(SaveDataBase);
            baseClass = nameof(UnmanagedSaveHandler<object, SaveDataBase>);
        }



        //attribute
        string dataGroupId = typeToHandle.Name;

        var generationMode = $"{nameof(SaveHandlerGenerationMode)}.{SaveHandlerGenerationMode.FullAutomata}";


        string id = GetOrCreateSaveHandlerId(typeToHandle, isStatic: false);


        var attribute = _SaveHandlerAttributeTemplate
            .Replace(SaveHandlerId, id)
            .Replace(DataGroupId, dataGroupId)
            .Replace(HandledType, CodeGenUtils.ToTypeDefinitionText(typeToHandle, withNameSpace: true))
            .Replace(GenerationMode, generationMode)
            .Replace(AdditionalParamList, "")
            ;



        //string nameSpaceContent = _SaveHandlerTemplate + _NewLine + _NewLine + _SaveDataTemplate;

        //string csFile = _CsFileTemplate.Replace(NameSpaceContent, nameSpaceContent);

        var templates = new List<string>()
        {
            _SaveHandlerTemplate,
            _SaveDataTemplate,
        };


        var saveDataAccessor = nameof(SaveHandlerGenericBase<int, SaveDataBase>.__saveData) + ".";
        var instanceAccessor = nameof(SaveHandlerGenericBase<int, SaveDataBase>.__instance) + ".";


        var generatedTypes = GenerateCommonCode(typeReport, templates, saveDataAccessor, instanceAccessor, isStatic: false);

        var handlerInfo = generatedTypes[0];
        var saveDataInfo = generatedTypes[1];

        string typeName = FlattenTypeNameIfNested(typeToHandle);

        ///todo: this logic is duplicated from <see cref="GenerateCommonCode"/>
        handlerInfo.GeneratedTypeName = $"{typeName}SaveHandler";
        saveDataInfo.GeneratedTypeName = $"{typeName}SaveData";

        foreach (var builder in generatedTypes)
        {
            builder.GeneratedTypeText = builder.GeneratedTypeText
                .Replace(SaveHandlerAttribute, attribute)
                .Replace(BaseClassName, baseClass)
                .Replace(SaveDataBaseClassName, saveDataBaseClassName)
                ;
        }


        result.HandlerInfo = handlerInfo;
        result.SaveDataInfo = saveDataInfo;

        return result;
    }





    public IReadOnlyList<CsFileBuilder> GenerateCommonCode(TypeReport typeReport, IEnumerable<string> csFileTemplates, string saveDataAccessor, string instanceAccessor, bool isStatic)
    {
        Type typeToHandle = typeReport.ReportedType;


        List<string> additionalNameSpaces = new()
        {
            typeToHandle.Namespace, //thus can be null
            "Assets._Project.Scripts.UtilScripts",
            "Assets._Project.Scripts.Infrastructure",
            "Assets._Project.Scripts.SaveAndLoad",
            "Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases",
            "Assets._Project.Scripts.SaveAndLoad.SavableDelegates",
            "System.Collections.Generic",
            "System",
            "System.Reflection",
        };




        //todo
        //if static or has static fields



        List<string> saveDataFields = new();
        List<string> writeDataFields = new();
        List<string> readDataFields = new();



        void GenerateDeclareWriteAndLoadField(Type type, MemberInfo memberInfo)
        {
            string saveDataField = "";
            string writeData = "";
            string readData = "";

            string fieldName = memberInfo.Name;

            var typeReference = CodeGenUtils.ToTypeReferenceText(type, withNameSpace: true);


            if (type.IsPrimitive || type.IsEnum || type == typeof(string) || _saveAndLoadService.HasSerializer_Editor(type)
                || type == typeof(LayerMask))// this madafaka's json converter doesnt work somewhy, so I make it an exception
            {
                saveDataField = $"public {typeReference} {fieldName};";

                writeData = $"{saveDataAccessor}{fieldName} = {instanceAccessor}{fieldName};";

                readData = $"{instanceAccessor}{fieldName} = {saveDataAccessor}{fieldName};";
            }
            else if (type.IsStruct())
            {
                if (_saveAndLoadService.HasCustomSaveData_Editor(type, out Type customSaveDataType))
                {
                    saveDataField = $"public {CodeGenUtils.ToTypeReferenceText(customSaveDataType, withNameSpace: true)} {fieldName} = new();";

                    writeData = $"{saveDataAccessor}{fieldName}.{nameof(CustomSaveData<int>.ReadFrom)}({instanceAccessor}{fieldName});";

                    readData = $"{saveDataAccessor}{fieldName}.{nameof(CustomSaveData<int>.WriteTo)}({instanceAccessor}{fieldName});";

                    //additionalNameSpaces.Add(customSaveDataType.Namespace);
                }
            }
            else if (typeof(Delegate).IsAssignableFrom(type))
            {

                saveDataField = $"public {nameof(InvocationList)} {fieldName} = new();";

                writeData = $"{saveDataAccessor}{fieldName} = {nameof(SaveHandlerBase.GetInvocationList)}({instanceAccessor}{fieldName});";

                readData = $"{instanceAccessor}{fieldName} = {nameof(SaveHandlerBase.GetDelegate)}<{typeReference}>({saveDataAccessor}{fieldName});";
            }
            else if (type.IsGenericParameter)
            {
                saveDataField = $"public Data<{type.Name}> {fieldName};";
                writeData = $"{saveDataAccessor}{fieldName}.Value = {instanceAccessor}{fieldName};";
                readData = $"{instanceAccessor}{fieldName} = {saveDataAccessor}{fieldName}.Value;";
            }
            else //ref type: class, array
            {
                saveDataField = $"public RandomId {fieldName};";

                if (IsAsset(type))
                {
                    writeData = $"{saveDataAccessor}{fieldName} = {nameof(SaveHandlerBase.GetAssetId)}({instanceAccessor}{fieldName});";
                    readData = $"{instanceAccessor}{fieldName} = {nameof(SaveHandlerBase.GetAssetById)}({saveDataAccessor}{fieldName}, {instanceAccessor}{fieldName});";
                }
                else
                {
                    writeData = $"{saveDataAccessor}{fieldName} = {nameof(SaveHandlerBase.GetObjectId)}({instanceAccessor}{fieldName});";
                    readData = $"{instanceAccessor}{fieldName} = {nameof(SaveHandlerBase.GetObjectById)}<{typeReference}>({saveDataAccessor}{fieldName});";
                }
            }

            saveDataFields.Add(saveDataField);
            writeDataFields.Add(writeData);
            readDataFields.Add(readData);
        }



        //fields and props

        foreach (FieldInfoReport fieldReport in typeReport.FieldsReport.ValidFields)
            GenerateDeclareWriteAndLoadField(fieldReport.FieldInfo.FieldType, fieldReport.FieldInfo);

        foreach (var property in typeReport.Properties)
            GenerateDeclareWriteAndLoadField(property.PropertyType, property);



        //events

        string targetTypeReference = CodeGenUtils.ToTypeReferenceText(typeToHandle, withNameSpace: true);
        string targetTypeDefinition = CodeGenUtils.ToTypeDefinitionText(typeToHandle, withNameSpace: true);


        foreach (var evt in typeReport.Events)
        {
            string fieldName = evt.Name;
            string eventTypeTypeReference = CodeGenUtils.ToTypeReferenceText(evt.EventHandlerType, withNameSpace: true);

            string saveDataField = $"public {nameof(InvocationList)} {fieldName} = new();";


            var getInvocationList = nameof(SaveHandlerGenericBase<int, SaveDataBase>.GetInvocationList);
            var getDelegate = nameof(SaveHandlerGenericBase<int, SaveDataBase>.GetDelegate);


            string writeData = $"{saveDataAccessor}{fieldName} = {getInvocationList}(nameof({targetTypeReference}.{evt.Name}));";

            string readData = $"var {fieldName}Del = {getDelegate}<{eventTypeTypeReference}>({saveDataAccessor}{fieldName});" +_NewLine;
            readData += $"if({fieldName}Del != null)"+_NewLine;
            readData += $"{instanceAccessor}{fieldName} += {fieldName}Del;";


            saveDataFields.Add(saveDataField);
            writeDataFields.Add(writeData);
            readDataFields.Add(readData);
        }



        //method registration


        var dictEntries = new List<string>();

        var idToMethodLookUpLines = new List<string>();
        var idToGenMethodDefLookUpLines = new List<string>();


        foreach (var method in typeReport.Methods)
        {
            string methodSignature = CodeGenUtils.GetMethodSignature(method);
            var id = RandomId.Get();

            string entry = $"{{\"{methodSignature}\", {id}}}";

            dictEntries.Add(entry);


            Func<ParameterInfo, bool> canNotBeUsedAsGenericParameter = (p) => p.ParameterType.IsByRef || p.ParameterType.IsPointer || p.ParameterType.IsByRefLike;

            if (method.IsGenericMethod || method.GetParameters().Any(canNotBeUsedAsGenericParameter) || canNotBeUsedAsGenericParameter(method.ReturnParameter))
            {
                string line = $"{id} => {CodeGenUtils.GenerateGetMethodCode(method)}";
                idToGenMethodDefLookUpLines.Add(line);
            }
            else
            {
                string delegateType = "Action";

                try
                {
                    var argNames = method.GetParameters().Select(p => CodeGenUtils.ToTypeReferenceText(p.ParameterType, withNameSpace: true)).ToList();

                    if (method.ReturnType != typeof(void))
                    {
                        delegateType = "Func";

                        argNames.Add(CodeGenUtils.ToTypeReferenceText(method.ReturnType, withNameSpace: true));
                    }

                    string argListText = argNames.Count > 0 ?
                        "<" + string.Join(", ", argNames) + ">" : "";

                    
                    string targetReference = isStatic? targetTypeReference : $"(({targetTypeReference})instance)";
                    string line = $"{id} => new Func<object, Delegate>((instance) => new {delegateType}{argListText}({targetReference}.{method.Name}))";

                    idToMethodLookUpLines.Add(line);

                }
                catch
                {
                    Debug.Log(typeReport.ReportedType.FullName + " " + method.Name);
                    Debug.Log(method.IsGenericMethod);
                    foreach(var p in method.GetParameters())
                    {
                        Debug.Log(p.ParameterType.CleanAssemblyQualifiedName() + " " + canNotBeUsedAsGenericParameter(p));
                    }
                        Debug.Log(method.ReturnParameter.ParameterType.FullName + " " + canNotBeUsedAsGenericParameter(method.ReturnParameter));
                    throw;
                }
            }
        }



        idToMethodLookUpLines.Add($"_ => {nameof(Infra)}.{nameof(Infra.Singleton)}.{nameof(Infra.Singleton.GetIdToMethodMapForType)}" +
                                        $"(_typeReference.BaseType)(id),");

        idToGenMethodDefLookUpLines.Add($"_ => {nameof(Infra)}.{nameof(Infra.Singleton)}.{nameof(Infra.Singleton.GetMethodInfoIdToMethodMapForType)}" +
                                                $"(_typeReference.BaseType)(id),");


        string methodSignaturesToMethodIds = string.Join("," + _NewLine, dictEntries);

        string idToMethodLookUp = string.Join("," + _NewLine, idToMethodLookUpLines);
        string idToGenMethodDefLookUp = string.Join("," + _NewLine, idToGenMethodDefLookUpLines);







        var genericParameterList = CodeGenUtils.GetGenericParameterListText(typeToHandle);


        string genericConstraints = typeToHandle.IsGenericType ?
            _NewLine + CodeGenUtils.GetGenericParameterConstraintsText(typeToHandle)
            : "";


        string fieldList = string.Join(_NewLine, saveDataFields);

        string writingFields = string.Join(_NewLine, writeDataFields);

        string readingFields = string.Join(_NewLine, readDataFields);


        additionalNameSpaces = additionalNameSpaces.Where(ns => ns != null).Distinct()
            .Select(ns => $"using {ns};").ToList();

        string additionalNameSpaces2 = string.Join(_NewLine, additionalNameSpaces);


        string fileNameSpace = typeToHandle.Namespace ?? "Assets._Project.Scripts.SaveAndLoad.SaveHandlers.DevTest";

        //remove fater testing
        fileNameSpace = "DevTest";


        IEnumerable<string> csFiles = csFileTemplates.Select(template =>
            template
            //.Replace(AdditionalNameSpaces, additionalNameSpaces2)
            //.Replace(FileNameSpace, fileNameSpace)
            .Replace(GeneratedTypeName, FlattenTypeNameIfNested(typeToHandle))
            .Replace(TargetTypeReference, targetTypeReference)
            .Replace(TargetTypeDefinition, targetTypeDefinition)
            .Replace(GenericParameterList, genericParameterList)
            .Replace(GenericConstraints, genericConstraints)
            .Replace(FieldList, fieldList)
            .Replace(WritingFields, writingFields)
            .Replace(ReadingFields, readingFields)
            .Replace(MethodSignaturesToMethodIds, methodSignaturesToMethodIds)
            .Replace(IdToMethodLookUp, idToMethodLookUp)
            .Replace(IdToGenMethodDefLookUp, idToGenMethodDefLookUp)

        );

        var builders = csFiles.Select(csFile => new CsFileBuilder
        {
            NameSpaceNames = additionalNameSpaces.Where(ns => ns != null).ToHashSet(),
            GeneratedTypeText = csFile,
        }).ToList();


        //Debug.LogWarning(typeToHandle.FullName);
        //foreach(var builder in builders)
        //{
        //    Debug.Log(builder.GeneratedTypeText);
        //}


        return builders;
    }



    public string FlattenTypeNameIfNested(Type type)
    {
        return CodeGenUtils.ToFlatName(type);
    }



    //todo: assetdb should decide what is asset and what is not. quick fix for now
    public bool IsAsset(Type type)
    {
        return typeof(UnityEngine.Object).IsAssignableFrom(type)
                && !typeof(UnityEngine.Component).IsAssignableFrom(type)
                && !typeof(UnityEngine.GameObject).IsAssignableFrom(type)
                && !typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type);
    }


    class MyClass
    {
        public string name;
    }
    [SaveHandler(34343, nameof(MyClass), typeof(MyClass))]
    class MyClassSaveHandler : UnmanagedSaveHandler<MyClass, MyClassSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.name = __instance.name;
        }
    }
    class MyClassSaveData : SaveDataBase
    {
        public string name;
    }
    #region Struct Generation



    #endregion

    #region Class Generation


    public const string AdditionalNameSpaces = "<AdditionalNameSpaces>";
    public const string SaveHandlerClass = "<SaveHandlerClass>";
    public const string SaveDataClass = "<SaveDataClass>";
    public const string FileNameSpace = "<FileNameSpace>";
    public const string NameSpaceContent = "<NameSpaceContent>";


    [NonSerialized]
    public string _CsFileTemplate =
        "//auto-generated" + _NewLine +
        AdditionalNameSpaces + _NewLine +
        _NewLine +
        $"namespace {FileNameSpace}" +
        _NewLine +
        "{" +
        _NewLine +
        //SaveHandlerClass +
        //_NewLine +
        //SaveDataClass +
        NameSpaceContent +
        _NewLine +
        "}" +
        "";





    public const string SaveHandlerAttribute = "<SaveHandlerAttribute>";
    public const string SaveHandlerClassName = "<SaveHandlerClassName>";
    public const string BaseClassName = "<BaseClassName>";
    public const string GenericParameterList = "<GenericParameterList>";
    public const string GenericConstraints = "<GenericConstraints>";
    public const string WritingFields = "<WritingFields>";
    public const string ReadingFields = "<ReadingFields>";
    public const string MethodSignaturesToMethodIds = "<MethodSignaturesToMethodIds>";
    public const string IdToMethodLookUp = "<IdToMethodLookUp>";
    public const string IdToGenMethodDefLookUp = "<IdToGenMethodDefLookUp>";


    [NonSerialized]
    public string _SaveHandlerTemplate =
        SaveHandlerAttribute + _NewLine +
        $"public class {GeneratedTypeName}SaveHandler{GenericParameterList} : " +
        $"{BaseClassName}<{TargetTypeReference}, {GeneratedTypeName}SaveData{GenericParameterList}> {GenericConstraints}" + _NewLine +
        "{" + _NewLine +
        $"public override void WriteSaveData()" + _NewLine +
        "{" + _NewLine +
        "base.WriteSaveData();" +
        _NewLine +
        WritingFields + _NewLine +
        "}" + _NewLine +
        _NewLine +
        $"public override void LoadReferences()" + _NewLine +
        "{" + _NewLine +
        "base.LoadReferences();" +
        _NewLine +
        ReadingFields + _NewLine +
        "}" + _NewLine +
        _NewLine +
        $"static {GeneratedTypeName}SaveHandler()" + _NewLine +
        "{" + _NewLine +
        "Dictionary<string, long> methodToId = new()" + _NewLine +
        "{" + _NewLine +
        MethodSignaturesToMethodIds + _NewLine +
        "};" +
        _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodSignatureToMethodIdMap) + "(_typeReference, methodToId);" + _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodIdToMethodMap) + "(_typeReference, _idToMethod);" + _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodIdToMethodInfoMap) + "(_typeReference, _idToMethodInfo);" + _NewLine +
        "}" +
        _NewLine +
        $"static Type _typeReference = typeof({TargetTypeReference});" + _NewLine +
        $"static Type _typeDefinition = typeof({TargetTypeDefinition});" + _NewLine +
        "static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;" +
        _NewLine +
        "public static Func<object, Delegate> _idToMethod(long id)" + _NewLine +
        "{" + _NewLine +
        "Func<object, Delegate> method = id switch" + _NewLine +
        "{" + _NewLine +
        IdToMethodLookUp + _NewLine +
        "};" + _NewLine +
        "return method;" + _NewLine +
        "}" +
        _NewLine +
        "public static MethodInfo _idToMethodInfo(long id)" + _NewLine +
        "{" + _NewLine +
        "MethodInfo methodDef = id switch" + _NewLine +
        "{" + _NewLine +
        IdToGenMethodDefLookUp + _NewLine +
        "};" + _NewLine +
        "return methodDef;" + _NewLine +
        "}" + _NewLine +
        "}" + _NewLine +
        "";





    public const string SaveHandlerId = "<SaveHandlerId>";
    public const string DataGroupId = "<DataGroupId>";
    public const string HandledType = "<HandledType>";
    //public const string IsStatic = "<IsStatic>";
    public const string GenerationMode = "<GenerationMode>";
    public const string AdditionalParamList = "<AdditionalParamList>";

    [NonSerialized]
    public string _SaveHandlerAttributeTemplate = $"[SaveHandler({SaveHandlerId}, \"{DataGroupId}\", typeof({HandledType}), generationMode: {GenerationMode}{AdditionalParamList})]";






    public const string SaveDataClassName = "<SaveDataClassName>";
    public const string SaveDataBaseClassName = "<SaveDataBaseClassName>";
    public const string FieldList = "<FieldList>";

    [NonSerialized]
    public string _SaveDataTemplate =
        $"public class {GeneratedTypeName}SaveData{GenericParameterList} : {SaveDataBaseClassName} {GenericConstraints}" +
        _NewLine +
        "{" +
        _NewLine +
        $"{FieldList}" +
        _NewLine +
        "}" + _NewLine+
        "";






    [NonSerialized]
    public string _StaticSaveHandlerAndSubtituteTemplate =
        $"public class Static{GeneratedTypeName}Subtitute{GenericParameterList} : StaticSubtitute {GenericConstraints}" + _NewLine +
        "{" + _NewLine +
        $"public override Type SubtitutedType => typeof({TargetTypeReference});" + _NewLine +
        "}" +
        _NewLine +
        _NewLine +
        SaveHandlerAttribute + _NewLine +
        $"public class Static{GeneratedTypeName}SaveHandler{GenericParameterList} : " +
        $"StaticSaveHandlerBase<Static{GeneratedTypeName}Subtitute{GenericParameterList}, Static{GeneratedTypeName}SaveData{GenericParameterList}> {GenericConstraints}" + _NewLine +
        "{" + _NewLine +
        $"public override void WriteSaveData()" + _NewLine +
        "{" + _NewLine +
        "base.WriteSaveData();" +
        _NewLine +
        WritingFields + _NewLine +
        "}" + _NewLine +
        _NewLine +
        $"public override void LoadReferences()" + _NewLine +
        "{" + _NewLine +
        "base.LoadReferences();" +
        _NewLine +
        ReadingFields + _NewLine +
        "}" + _NewLine +
        $"static Static{GeneratedTypeName}SaveHandler()" + _NewLine +
        "{" + _NewLine +
        "Dictionary<string, long> methodToId = new()" + _NewLine +
        "{" + _NewLine +
        MethodSignaturesToMethodIds + _NewLine +
        "};" +
        _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodSignatureToMethodIdMap) + "(_typeReference, methodToId);" + _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodIdToMethodMap) + "(_typeReference, _idToMethod);" + _NewLine +
        nameof(Infra) + "." + nameof(Infra.Singleton) + "." + nameof(Infra.Singleton.AddMethodIdToMethodInfoMap) + "(_typeReference, _idToMethodInfo);" + _NewLine +
        "}" +
        _NewLine +
        $"static Type _typeReference = typeof({TargetTypeReference});" + _NewLine +
        $"static Type _typeDefinition = typeof({TargetTypeDefinition});" + _NewLine +
        "static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;" +
        _NewLine +
        "public static Func<object, Delegate> _idToMethod(long id)" + _NewLine +
        "{" + _NewLine +
        "Func<object, Delegate> method = id switch" + _NewLine +
        "{" + _NewLine +
        IdToMethodLookUp + _NewLine +
        "};" + _NewLine +
        "return method;" + _NewLine +
        "}" +
        _NewLine +
        "public static MethodInfo _idToMethodInfo(long id)" + _NewLine +
        "{" + _NewLine +
        "MethodInfo methodDef = id switch" + _NewLine +
        "{" + _NewLine +
        IdToGenMethodDefLookUp + _NewLine +
        "};" + _NewLine +
        "return methodDef;" + _NewLine +
        "}" + _NewLine +
        "}" + _NewLine +
        "";




    [NonSerialized]
    public string _StaticSaveDataTemplate =
        $"public class Static{GeneratedTypeName}SaveData{GenericParameterList} : StaticSaveDataBase {GenericConstraints}" + _NewLine +
        "{" + _NewLine +
        $"{FieldList}" +
        _NewLine +
        "}" + _NewLine +
        "";





    [NonSerialized]
    public string _CustomSaveDataTemplate =
        $"public class {GeneratedTypeName}SaveData{GenericParameterList} : " +
        $"CustomSaveData<{TargetTypeReference}> {GenericConstraints}" +
        _NewLine +
        "{" +
        _NewLine +
        $"{FieldList}" +
        _NewLine +
        $"public override void ReadFrom(in {TargetTypeReference} instance)" +
        _NewLine +
        "{" +
        _NewLine +
        $"{WritingFields}" +
        _NewLine +
        "}" +
        _NewLine +
        $"public override void WriteTo(ref {TargetTypeReference} instance)" +
        _NewLine +
        "{" +
        _NewLine +
        $"{ReadingFields}" +
        _NewLine +
        "}" +
        _NewLine +
        //$"public static implicit operator {TargetTypeReference}({CustomSaveDataClassDefinition} saveData)" +
        //"{" +
        //"}" +
        "}" +
        "";

    public const string CustomSaveDataClassDefinition = "<CustomSaveDataClassDefinition>";
    public const string CustomSaveDataBaseClassDefinition = "<CustomSaveDataBaseClassDefinition>";
    public const string TargetTypeReference = "<TargetTypeReference>";
    public const string TargetTypeDefinition = "<TargetTypeDefinition>";
    public const string GeneratedTypeName = "<GeneratedTypeName>";
    public const string FieldName = "<FieldName>";



    public static string _NewLine = Environment.NewLine;


    #endregion
}
