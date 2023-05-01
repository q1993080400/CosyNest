namespace System.Maths.Tree;

/// <summary>
/// 这个类型是<see cref="INodeContent{Obj}"/>的实现，
/// 可以视为一个封装了特定内容的树形结构节点
/// </summary>
/// <typeparam name="Obj">节点所封装的内容类型</typeparam>
class NodeContent<Obj> : INodeContent<Obj>
{
    #region 接口实现
    #region 获取节点所封装的内容
    public Obj Content { get; }
    #endregion
    #region 获取父节点
    public INode? Father { get; }
    #endregion
    #region 获取子节点
    public IEnumerable<INode> Son { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="father">该节点的父节点</param>
    /// <param name="content">该节点所包含的内容</param>
    /// <param name="getSon">传入包含内容，获取所有直接子节点的委托</param>
    public NodeContent(INodeContent<Obj>? father, Obj content, Func<Obj, IEnumerable<Obj>> getSon)
    {
        Content = content;
        Father = father;
        Son = getSon(content).Select(x => new NodeContent<Obj>(this, x, getSon));
    }
    #endregion
}
