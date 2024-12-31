namespace System.DataFrancis;

/// <summary>
/// 这个枚举是<see cref="IHasUploadFileServer"/>进行保存的状态
/// </summary>
public enum UploadFileSaveState
{
    /// <summary>
    /// 表示这个文件尚未保存，
    /// 而且没有用于上传的文件名，所以不能保存
    /// </summary>
    NotSave,
    /// <summary>
    /// 表示这个文件尚未保存，
    /// 但是它已经具有用于上传的文件名，
    /// 可以开始执行保存操作
    /// </summary>
    HasUploadFileName,
    /// <summary>
    /// 表示这个文件已经被保存到服务器中
    /// </summary>
    SaveCompleted
}
