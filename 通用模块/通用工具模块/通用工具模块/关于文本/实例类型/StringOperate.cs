namespace System.Text;

/// <summary>
/// 这个类型封装了一个<see cref="string"/>，
/// 它可以用于字符串的特化扩展方法
/// </summary>
public sealed record StringOperate
{
    #region 说明文档
    /*问：这个类型有什么意义？
      答：有很多关于字符串的扩展方法是高度特化的，
      例如判断电话号码，转换Uri等等，如果将它们作为string的扩展方法，
      会让string看起来非常臃肿，但是不这么做又很不方便，
      所以作者声明了本类型，可以先通过string的扩展方法获取本类型，
      然后再通过本类型的扩展方法去执行这些操作，兼顾了两方面的优点*/
    #endregion
    #region 封装的字符串
    /// <summary>
    /// 获取封装的字符串，
    /// 扩展方法可以通过这个属性获取操作的目标
    /// </summary>
    public required string Text { get; init; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Text;
    #endregion
}
