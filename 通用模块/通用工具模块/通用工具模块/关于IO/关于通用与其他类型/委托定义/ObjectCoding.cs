namespace System.IOFrancis.Bit;

#region 读取对象
/// <summary>
/// 这个委托可以从二进制管道中读取对象，
/// 它比直接读取字节数组更加高层
/// </summary>
/// <typeparam name="Obj">要读取的对象类型</typeparam>
/// <param name="bitRead">用来获取二进制数据的管道</param>
/// <returns></returns>
public delegate IAsyncEnumerable<Obj> ObjRead<out Obj>(IBitRead bitRead);
#endregion
#region 写入对象
/// <summary>
/// 这个委托可以向二进制管道写入对象，
/// 它比直接写入字节数组更加高层
/// </summary>
/// <typeparam name="Obj">要写入的对象类型</typeparam>
/// <param name="bitWrite">要写入对象的二进制管道</param>
/// <param name="obj">要写入的对象</param>
/// <returns></returns>
public delegate Task ObjWrite<in Obj>(IBitWrite bitWrite, Obj obj);
#endregion