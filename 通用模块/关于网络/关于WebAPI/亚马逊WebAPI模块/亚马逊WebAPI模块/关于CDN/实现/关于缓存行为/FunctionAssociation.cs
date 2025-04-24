using System.Design;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IFunctionAssociation"/>的实现，
/// 可以视为一个亚马逊CDN分配的函数关联
/// </summary>
sealed record FunctionAssociation : IFunctionAssociation, ICreate<IFunctionAssociation>
{
    #region 静态抽象方法实现：创建对象
    public static IFunctionAssociation Create()
        => new FunctionAssociation();
    #endregion
    #region 事件类型
    public FunctionEventType EventType { get; set; }
    #endregion
    #region 函数的ARN
    public string FunctionARN { get; set; } = "";
    #endregion
}
