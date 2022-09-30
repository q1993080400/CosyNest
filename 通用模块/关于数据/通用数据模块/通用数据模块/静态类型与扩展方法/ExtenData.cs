using System.DataFrancis;
using System.DataFrancis.Verify;

namespace System;

/// <summary>
/// 有关数据的扩展方法
/// </summary>
public static class ExtenData
{
    #region 为数据管道添加验证功能
    /// <summary>
    /// 为数据管道增加验证功能，
    /// 并返回支持验证的数据管道
    /// </summary>
    /// <param name="pipe">原数据管道</param>
    /// <param name="verify">用来验证的委托，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns></returns>
    public static IDataPipe UseVerify(this IDataPipe pipe, DataVerify? verify = null)
        => new DataPipeVerify(pipe, verify ??= CreateDataObj.DataVerifyDefault);
    #endregion
}
