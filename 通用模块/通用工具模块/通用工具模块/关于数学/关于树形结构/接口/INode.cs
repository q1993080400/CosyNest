using System.Collections.Generic;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个树形结构的节点
    /// </summary>
    public interface INode
    {
        #region 有关父节点
        #region 获取父节点
        /// <summary>
        /// 获取这个节点的父节点，
        /// 如果是根节点，则返回<see langword="null"/>
        /// </summary>
        INode? Father { get; }
        #endregion
        #region 返回根节点
        /// <summary>
        /// 返回本节点的根节点，
        /// 如果本节点已经是根节点，则返回自身
        /// </summary>
        INode Ancestors
            => Father?.Ancestors ?? this;
        #endregion
        #region 枚举所有祖先节点
        /// <summary>
        /// 枚举本节点的所有祖先节点
        /// </summary>
        IEnumerable<INode> AncestorsAll
        {
            get
            {
                var node = this;
                while (true)
                {
                    node = node!.Father;
                    if (node is null)
                        yield break;
                    yield return node;
                }
            }
        }
        #endregion
        #region 返回节点的深度
        /// <summary>
        /// 返回节点的深度，
        /// 也就是与根节点的距离
        /// </summary>
        int Depth
            => Father is null ? 0 : Father.Depth + 1;
        #endregion
        #endregion
        #region 有关子节点
        #region 获取子节点
        /// <summary>
        /// 获取这个节点的直接子节点
        /// </summary>
        IEnumerable<INode> Son { get; }
        #endregion
        #region 递归获取所有子节点
        /// <summary>
        /// 获取一个遍历所有直接与间接子节点的枚举器
        /// </summary>
        /// <returns></returns>
        IEnumerable<INode> SonAll
        {
            get
            {
                foreach (var son in Son)
                {
                    yield return son;
                    foreach (var grandson in son.SonAll)
                        yield return grandson;
                }
            }
        }
        #endregion
        #endregion
    }
}
