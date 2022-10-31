namespace System.NetFrancis.Http;

/// <summary>
/// 这个记录可以作为一个绝对Uri
/// </summary>
public sealed record UriAbsolute : UriRelatively
{
    #region 隐式类型转换
    public static implicit operator UriAbsolute(string uriComplete)
        => new(uriComplete);
    #endregion
    #region 关于Uri
    #region 获取协议部分
    /// <summary>
    /// 获取Uri的协议部分
    /// </summary>
    public string Agreement
        => UriBase[0..UriBase.IndexOf(":")];
    #endregion
    #region 获取基础URI
    private readonly string UriBaseField = "";

    /// <summary>
    /// 获取请求的目标的基础Uri部分，
    /// 通过它可以找到服务器主机
    /// </summary>
    public string UriBase
    {
        get => UriBaseField;
        init => UriBaseField = value.TrimEnd('/');
    }
    #endregion
    #region 获取完整Uri
    public override string UriComplete
        => UriBase + base.UriComplete;
    #endregion
    #endregion
    #region 构造函数
    #region 指定完整绝对Uri
    /// <summary>
    /// 使用指定的完整绝对Uri初始化对象
    /// </summary>
    /// <param name="uriComplete">完整的绝对Uri，可能包含参数</param>
    public UriAbsolute(string uriComplete)
    {
        var (_, uri, parameters) = ToolNet.ExtractionParameters(uriComplete);
        this.UriBase = uri;
        UriParameters = (parameters ?? CreateCollection.EmptyDictionary<string, string>())!;
    }
    #endregion
    #region 无参数构造函数
    public UriAbsolute()
    {

    }
    #endregion
    #endregion
}
