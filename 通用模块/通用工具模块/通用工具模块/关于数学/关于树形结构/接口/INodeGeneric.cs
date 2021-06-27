using System.Collections.Generic;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以作为一个树形结构的节点，
    /// 与<see cref="INode"/>不同的是，该接口的父节点和子节点类型更具体
    /// </summary>
    /// <typeparam name="Node">父节点和子节点的类型</typeparam>
    public interface INode<out Node> : INode
        where Node : INode
    {
        #region 重要说明
        /*警告：
          如果节点的父节点，子节点，或祖先节点具有不同的类型，
          请不要使用本接口，而是直接使用INode，
          否则在访问这些API的时候，会导致类型转换错误，举例说明：
          IDrive的父节点是IFileSystem，子节点却是IDirectory，
          这种情况下不要使用本接口*/
        #endregion
        #region 返回父接口形式
        /// <summary>
        /// 返回本对象的父接口形式，
        /// 通过它可以调用父接口的方法
        /// </summary>
        private INode Base => this;
        #endregion
        #region 有关父节点
        #region 获取父节点
        /// <inheritdoc cref="INode.Father"/>
        new Node? Father => (Node?)Base.Father;
        #endregion
        #region 返回根节点
        /// <inheritdoc cref="INode.Ancestors"/>
        new Node Ancestors
             => (Node)Base.Ancestors;
        #endregion
        #region 枚举所有祖先节点
        /// <summary>
        /// 枚举本节点的所有祖先节点
        /// </summary>
        new IEnumerable<Node> AncestorsAll
             => Base.AncestorsAll.Cast<Node>();
        #endregion
        #endregion
        #region 有关子节点
        #region 获取子节点
        /// <inheritdoc cref="INode.Son"/>
        new IEnumerable<Node> Son => Base.Son.Cast<Node>();
        #endregion
        #region 递归获取所有子节点
        /// <inheritdoc cref="INode.SonAll"/>
        new IEnumerable<Node> SonAll
             => Base.SonAll.Cast<Node>();
        #endregion
        #endregion
    }
}
