namespace System.IOFrancis.Bit;

#region 用于转换管道的委托
/// <summary>
/// 转换一个<see cref="IBitRead"/>的输出，
/// 并返回一个新的<see cref="IBitRead"/>
/// </summary>
/// <param name="pipe">待转换的<see cref="IBitRead"/></param>
/// <returns></returns>
public delegate IBitRead BitMapping(IBitRead pipe);
#endregion 