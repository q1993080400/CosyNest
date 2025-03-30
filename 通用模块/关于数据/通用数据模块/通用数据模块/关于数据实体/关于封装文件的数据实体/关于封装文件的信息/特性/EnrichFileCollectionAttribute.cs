namespace System.DataFrancis;

/// <summary>
/// 这个特性指示某个容纳<see cref="IHasReadOnlyPreviewFile"/>的集合不能自身不能为<see langword="null"/>，
/// 而且必须存在至少一个元素
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class EnrichFileCollectionAttribute : Attribute
{
}
