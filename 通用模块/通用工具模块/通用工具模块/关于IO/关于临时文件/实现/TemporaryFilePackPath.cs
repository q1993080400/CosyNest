namespace System.IOFrancis;

/// <summary>
/// 这个类型是<see cref="ITemporaryFilePack{Obj}"/>的实现，
/// 它封装一个临时文件的路径，但是不封装临时文件
/// </summary>
/// <param name="path"></param>
sealed class TemporaryFilePackPath(string path) : ITemporaryFilePack<string>
{
    #region 临时文件路径
    public string TemporaryObj { get; } = path;
    #endregion
    #region 释放对象
    public void Dispose()
    {
        var io = CreateIO.IO(TemporaryObj);
        io?.Delete();
    }
    #endregion 
}
