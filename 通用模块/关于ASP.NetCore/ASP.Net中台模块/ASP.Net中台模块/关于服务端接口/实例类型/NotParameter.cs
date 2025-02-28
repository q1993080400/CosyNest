namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型专门用于<see cref="IServerSearch{Parameter, Obj}.Search(Parameter)"/>的参数，
/// 它指示这个接口实际上不需要搜索参数，也不支持搜索，
/// 而是直接返回所有对象
/// </summary>
public sealed class NotParameter
{
}
