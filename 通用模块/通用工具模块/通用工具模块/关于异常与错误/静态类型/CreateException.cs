namespace System;

/// <summary>
/// 这个静态类可以用来创建常用异常
/// </summary>
public static class CreateException
{
    #region 返回NotSupportedException
    #region 由于不支持指定功能所引发
    /// <summary>
    /// 返回一个由于不支持指定的功能所引发的异常
    /// </summary>
    /// <param name="function">对不受支持的功能的说明</param>
    /// <returns></returns>
    public static NotSupportedException NotSupported(string? function = null)
        => new("不支持" + (function ?? "此功能"));
    #endregion
    #region 由于不支持移除事件而引发
    #region 说明文档
    /*问：某一事件只支持注册，不支持移除，
      这似乎是一个比较奇怪的设计，请问它在哪些情况下会出现？
      答：举一个比较常见的例子：
      假设某一事件不是直接注册委托，而是通过高阶函数进行注册，举例说明：

      add => Watcher.Deleted += (_, e) => value!(e.FullPath);

      在这种情况下，即便是通过相同的高阶函数进行转换，
      注册进去和想要移除的仍然是两个不同的委托，
      这会导致注销事件的功能失效，为了避免难以排查的异常，
      不如直接禁止访问remove访问器

      事实上，在注销事件的时候需要特别注意委托的相等性，
      否则会导致注销无效，以下两个委托是不相等的：

      Action a = () => { };
      Action b = () => { };

      而这两个委托才是相等的：

      static void Fun()
      {

      }

      Action a = Fun;
      Action b = Fun;*/
    #endregion
    #region 正式方法
    /// <summary>
    /// 返回由于不支持移除事件所引发的异常
    /// </summary>
    public static NotSupportedException NotSupportedEventRemove
        => new("本事件只支持注册，不支持移除");
    #endregion
    #endregion
    #endregion
}
