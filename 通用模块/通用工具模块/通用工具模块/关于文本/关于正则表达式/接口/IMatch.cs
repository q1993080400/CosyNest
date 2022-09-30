namespace System.Text.RegularExpressions;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个正则表达式匹配的结果
/// </summary>
public interface IMatch
{
    #region 匹配到的字符
    /// <summary>
    /// 获取匹配到的字符
    /// </summary>
    string Match { get; }
    #endregion
    #region 匹配到的组
    /// <summary>
    /// 获取匹配到的组
    /// </summary>
    IReadOnlyList<IMatch> Groups { get; }

    /*实现本API请遵循以下规范：
      #匹配到的组不包括这个表达式本身，举例说明：
      若有表达式1(2)，匹配文本12，
      那么匹配结果应返回12，组应返回2，而不是12,2*/
    #endregion
    #region 获取匹配组的名称
    /// <summary>
    /// 获取匹配组的名称，
    /// 如果为<see langword="null"/>，代表没有名称
    /// </summary>
    string? Name { get; }
    #endregion
    #region 获取命名匹配组
    #region 说明文档
    /*实现本API请遵循以下规范：
      #命名匹配组只收录显式命名的匹配组，
      也就是说，正则表达式自动命名的匹配组不包含在内*/
    #endregion
    #region 通过字典
    /// <summary>
    /// 这个字典的键是命名匹配组的名称，
    /// 值是所有捕获的命名匹配组
    /// </summary>
    IReadOnlyDictionary<string, IMatch> GroupsNamed { get; }
    #endregion
    #region 通过索引器
    /// <summary>
    /// 通过名称，获取匹配到的命名匹配组
    /// </summary>
    /// <param name="name">命名匹配组的名称</param>
    /// <returns></returns>
    IMatch this[string name]
        => GroupsNamed[name];
    #endregion
    #endregion
}
