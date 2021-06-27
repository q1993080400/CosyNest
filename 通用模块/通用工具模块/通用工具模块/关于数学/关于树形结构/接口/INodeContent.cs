namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个封装了特定内容的树形结构节点
    /// </summary>
    /// <typeparam name="Obj">节点封装的内容的类型</typeparam>
    public interface INodeContent<out Obj> : INode<INodeContent<Obj>>
    {
        #region 获取节点的内容
        /// <summary>
        /// 获取节点的内容
        /// </summary>
        Obj Content { get; }
        #endregion
    }
}
