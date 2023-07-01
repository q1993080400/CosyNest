namespace System.Reflection;

partial class TypeData
{
    //这个类型专门用来储存关于属性，字段和方法的反射

    #region 关于字段
    #region 枚举所有字段
    private IEnumerable<FieldInfo>? FieldField;

    public IEnumerable<FieldInfo> Fields
        => FieldField ??= InitialEnum<FieldInfo>();
    #endregion
    #region 按名称索引字段
    private ILookup<string, FieldInfo>? FieldDictionaryField;

    public ILookup<string, FieldInfo> FieldDictionary
        => FieldDictionaryField ??= Fields.ToLookup(x => x.Name);
    #endregion
    #region 按类型索引字段
    private ILookup<Type, FieldInfo>? FieldDictionaryTypeField;

    public ILookup<Type, FieldInfo> FieldDictionaryType
        => FieldDictionaryTypeField ??= Fields.ToLookup(x => x.FieldType);
    #endregion
    #region 筛选字段
    public FieldInfo FindField(string name, Type? declaringType = null)
        => Find(FieldDictionary, name, declaringType);
    #endregion
    #endregion
    #region 关于属性
    #region 枚举所有属性
    private IEnumerable<PropertyInfo>? PropertyFiled;

    public IEnumerable<PropertyInfo> Propertys
        => PropertyFiled ??= InitialEnum<PropertyInfo>().Where(x => !x.IsIndexing()).ToArray();
    #endregion
    #region 按名称索引
    private ILookup<string, PropertyInfo>? PropertyDictionaryField;

    public ILookup<string, PropertyInfo> PropertyDictionary
        => PropertyDictionaryField ??= Propertys.ToLookup(x => x.Name);
    #endregion
    #region 按类型索引
    private ILookup<Type, PropertyInfo>? PropertyDictionaryTypeField;

    public ILookup<Type, PropertyInfo> PropertyDictionaryType
        => PropertyDictionaryTypeField ??= Propertys.ToLookup(x => x.PropertyType);
    #endregion
    #region 筛选属性
    public PropertyInfo FindProperty(string name, Type? declaringType = null)
        => Find(PropertyDictionary, name, declaringType);
    #endregion
    #region 枚举所有全能属性
    private IEnumerable<PropertyInfo>? AlmightyPropertysField;

    public IEnumerable<PropertyInfo> AlmightyPropertys
        => AlmightyPropertysField ??= Propertys.Where(x => x.IsAlmighty()).ToArray();
    #endregion
    #endregion
    #region 关于索引器
    #region 枚举所有索引器
    private IEnumerable<PropertyInfo>? IndexingField;

    public IEnumerable<PropertyInfo> Indexings
        => IndexingField ??= InitialEnum<PropertyInfo>().Where(x => x.IsIndexing()).ToArray();
    #endregion
    #region 按签名索引索引器
    private ILookup<IMethodSignature, PropertyInfo>? IndexingDictionaryField;

    public ILookup<IMethodSignature, PropertyInfo> IndexingDictionary
        => IndexingDictionaryField ??= Indexings.ToLookup(x =>
          CreateReflection.MethodSignature(x.PropertyType, x.GetIndexParameters().GetParType()));
    #endregion
    #region 筛选索引器
    public PropertyInfo FindIndexing(IMethodSignature signature, Type? declaringType = null)
        => Find(IndexingDictionary, signature, declaringType);
    #endregion
    #endregion
    #region 关于方法
    #region 枚举所有方法
    private IEnumerable<MethodInfo>? MethodField;

    public IEnumerable<MethodInfo> Methods
        => MethodField ??= InitialEnum<MethodInfo>();
    #endregion
    #region 按照名称索引方法
    private ILookup<string, MethodInfo>? MethodDictionaryField;

    public ILookup<string, MethodInfo> MethodDictionary
        => MethodDictionaryField ??= Methods.ToLookup(x => x.Name);
    #endregion
    #region 按签名索引方法
    private ILookup<IMethodSignature, MethodInfo>? MethodDictionarySignatureField;

    public ILookup<IMethodSignature, MethodInfo> MethodDictionarySignature
        => MethodDictionarySignatureField ??= Methods.ToLookup(x => x.GetSignature());
    #endregion
    #region 筛选方法
    public MethodInfo FindMethod(string name, IMethodSignature? signature = null, Type? declaringType = null)
        => Find(MethodDictionary, name, declaringType,
            x => signature is null || x.IsSame(signature));
    #endregion
    #endregion
}
