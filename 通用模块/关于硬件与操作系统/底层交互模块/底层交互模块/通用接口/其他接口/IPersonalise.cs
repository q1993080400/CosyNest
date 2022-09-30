using System.IOFrancis.FileSystem;

namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理个性化设置
/// </summary>
public interface IPersonalise
{
    #region 壁纸路径
    /// <summary>
    /// 获取或设置壁纸路径
    /// </summary>
    PathText Wallpaper { get; set; }
    #endregion
}
