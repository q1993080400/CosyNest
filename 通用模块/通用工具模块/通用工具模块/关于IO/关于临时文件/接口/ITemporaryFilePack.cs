namespace System.IOFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个临时文件封装，
/// 在释放掉它的时候，会自动删除这个临时文件
/// </summary>
/// <typeparam name="Obj"></typeparam>
public interface ITemporaryFilePack<out Obj> : IDisposable
    where Obj : class
{
    #region 临时文件对象
    /// <summary>
    /// 获取一个表示这个临时文件的对象
    /// </summary>
    Obj TemporaryObj { get; }
    #endregion
}
