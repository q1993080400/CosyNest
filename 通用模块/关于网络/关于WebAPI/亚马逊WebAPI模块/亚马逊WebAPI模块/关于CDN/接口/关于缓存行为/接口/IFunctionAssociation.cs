namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个亚马逊CDN分配的缓存行为的函数关联
/// </summary>
public interface IFunctionAssociation
{
    #region 事件类型
    /// <summary>
    /// 获取函数事件的类型
    /// </summary>
    FunctionEventType EventType { get; set; }
    #endregion
    #region 函数的ARN
    /// <summary>
    /// 获取函数的ARN，
    /// 只能关联已经发布的函数
    /// </summary>
    string FunctionARN { get; set; }
    #endregion
}
