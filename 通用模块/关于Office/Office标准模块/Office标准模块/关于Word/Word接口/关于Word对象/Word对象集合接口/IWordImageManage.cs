namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理Word图片
/// </summary>
public interface IWordImageManage : IOfficeObjectManage<IWordImage>
{
    #region 添加图片
    /// <summary>
    /// 添加图片，
    /// 并返回添加后的图片
    /// </summary>
    /// <param name="path">图片的路径</param>
    /// <param name="pos">指示放置图片的位置</param>
    /// <returns></returns>
    IWordImage Add(string path, IWordPos pos);
    #endregion
}
