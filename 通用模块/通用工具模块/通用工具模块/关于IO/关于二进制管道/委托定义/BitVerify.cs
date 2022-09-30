namespace System.IOFrancis.Bit;

#region 用于验证管道的委托
/// <summary>
/// 验证一个管道内部的二进制数据
/// </summary>
/// <param name="pipe">待验证的管道</param>
/// <returns>如果验证通过，返回<see langword="true"/>，否则返回<see langword="false"/></returns>
public delegate Task<bool> BitVerify(IBitRead pipe);
#endregion 