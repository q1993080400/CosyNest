namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以封装一个格式化字符串，
/// 作为渲染数据的依据
/// </summary>
public interface IRenderHasFormat
{
    #region 格式化字符串
    /// <summary>
    /// 指示渲染所需要的格式化字符串，
    /// 如果为<see langword="null"/>，
    /// 表示遵循默认格式化字符串
    /// </summary>
    string? Format { get; }
    #endregion
}
