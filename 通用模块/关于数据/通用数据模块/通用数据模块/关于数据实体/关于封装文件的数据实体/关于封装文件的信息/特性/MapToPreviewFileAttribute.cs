namespace System.DataFrancis;

/// <summary>
/// 这个特性指示某个类型被间接映射为可预览文件，
/// 它要求该类型实现<see cref="ICreate{Obj, Parameter}"/>和<see cref="IProjection{Obj}"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MapToPreviewFileAttribute : Attribute
{
}
