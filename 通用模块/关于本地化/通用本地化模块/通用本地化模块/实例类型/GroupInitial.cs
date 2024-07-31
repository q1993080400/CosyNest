namespace System.Localization;

/// <summary>
/// 这个记录封装了首字母还有按照这个首字母分类的对象
/// </summary>
/// <typeparam name="Obj">封装的对象类型</typeparam>
/// <param name="Initial">首字母</param>
/// <param name="Content">要封装的对象，它们都共享相同的首字母</param>
public sealed record GroupInitial<Obj>(string Initial, IEnumerable<Obj> Content)
{
}
