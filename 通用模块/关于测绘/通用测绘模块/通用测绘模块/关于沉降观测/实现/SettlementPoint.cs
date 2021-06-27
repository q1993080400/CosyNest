using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是<see cref="ISettlementPoint"/>的实现，
    /// 可以作为一个沉降观测点
    /// </summary>
    class SettlementPoint : SettlementBase, ISettlementPoint
    {
        #region 名称
        public string Name { get; }
        #endregion
        #region 索引高程已知的沉降点
        /// <summary>
        /// 索引本次沉降观测中，
        /// 高程已知的点的名称的高程
        /// </summary>
        public IReadOnlyDictionary<string, IUnit<IUTLength>> Known { get; }
        #endregion
        #region 高程
        #region 已知高程
        /// <summary>
        /// 返回这个点已知的固定高程，
        /// 如果它不是已知的，则返回<see langword="null"/>
        /// </summary>
        private IUnit<IUTLength>? HighKnown { get; }
        #endregion
        #region 原始高程
        /// <summary>
        /// 获取原始高程，
        /// 它直接经过计算得出，未经平差，也不考虑固定点的情况
        /// </summary>
        private IUnit<IUTLength> HighOriginal
            => Father.To<ISettlement>()!.High - Recording!;
        #endregion
        #region 正式高程
        public override IUnit<IUTLength> High
            => HighKnown ??
            Closed?.High ??
            HighOriginal - ClosedDifference;
        #endregion
        #endregion
        #region 是否为已知点
        public bool IsKnown
            => HighKnown is { } || Closed is { };
        #endregion
        #region 是否为转点
        #region 正式属性
        public bool IsIntermediary { get; }
        #endregion
        #region 用于判断的委托
        /// <summary>
        /// 该委托传入点的名称，然后判断是否为转点
        /// </summary>
        public Func<string, bool> IsIntermediaryDelegate { get; }
        #endregion
        #endregion
        #region 关于闭合/附合
        #region 闭合/附合点
        /// <summary>
        /// 返回与这个点闭合或附合的点，
        /// 如果没有闭合或附合，则为<see langword="null"/>
        /// </summary>
        public ISettlementPoint? Closed { get; private set; }
        #endregion
        #region 闭合/附合差
        public IUnit<IUTLength> ClosedDifference { get; set; } = IUnit<IUTLength>.Zero;
        #endregion
        #region 刷新闭合
        /// <summary>
        /// 调用这个方法以刷新闭合站和闭合差
        /// </summary>
        private void RefreshClosed()
        {
            #region 用于枚举闭合环的本地函数
            SettlementPoint[] Closed()
            {
                var list = new LinkedList<SettlementPoint>();
                foreach (var item in this.To<INode>().AncestorsAll.OfType<SettlementPoint>())
                {
                    list.AddLast(item);
                    if (item.Name == Name || (IsKnown && item.IsKnown))        //检查是否为闭合或附合
                        return list.ToArray();
                }
                return CreateCollection.Empty(list);
            }
            #endregion
            var closed = Closed();
            if (closed.Any())
            {
                this.Closed = closed[^1];
                var difference = (HighOriginal - this.Closed.High) / closed.Length;
                closed[..^1].Append(this).ForEach(x => x.ClosedDifference = difference);
            }
        }
        #endregion
        #endregion 
        #region 添加后代观测站
        public ISettlementObservatory Add(IUnit<IUTLength> recording)
        {
            var son = new SettlementObservatory(recording, this);
            SonField.AddLast(son);
            return son;
        }
        #endregion
        #region 重写ToString
        public override string ToString() => Name;
        #endregion
        #region 构造函数
        #region 创建固定点
        /// <summary>
        /// 创建一个固定点，它可能是基准点或附合点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="recording">记录</param>
        /// <param name="high">已知高程</param>
        /// <param name="father">父观测站</param>
        /// <param name="known">索引所有已知沉降点</param>
        /// <param name="isIntermediaryDelegate">该委托传入点的名称，然后判断是否为转点</param>
        public SettlementPoint(string name, IUnit<IUTLength>? recording,
            IUnit<IUTLength> high, ISettlementObservatory? father,
            IReadOnlyDictionary<string, IUnit<IUTLength>> known,
            Func<string, bool> isIntermediaryDelegate)
        {
            this.Name = name;
            this.HighKnown = high;
            this.Father = father;
            this.Known = known;
            this.Recording = recording;
            this.IsIntermediaryDelegate = isIntermediaryDelegate;
            if (father is { })
                RefreshClosed();
        }
        #endregion
        #region 创建起始点
        /// <summary>
        /// 这个构造函数专门用于创建沉降观测的起始点，
        /// 它是整个沉降观测的第一个点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="high">已知高程</param>
        /// <param name="known">索引所有已知沉降点</param>
        /// <param name="isIntermediaryDelegate">该委托传入点的名称，然后判断是否为转点</param>
        public SettlementPoint(string name, IUnit<IUTLength> high,
            IReadOnlyDictionary<string, IUnit<IUTLength>> known,
            Func<string, bool> isIntermediaryDelegate)
            : this(name, null, high, null, known, isIntermediaryDelegate)
        {

        }
        #endregion
        #region 创建普通点
        /// <summary>
        /// 创建一个普通沉降点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="recording">记录</param>
        /// <param name="father">父观测站</param>
        /// <param name="known">索引所有已知沉降点</param>
        /// <param name="isIntermediaryDelegate">该委托传入点的名称，然后判断是否为转点</param>
        public SettlementPoint(string name, IUnit<IUTLength>? recording,
            ISettlementObservatory father,
            IReadOnlyDictionary<string, IUnit<IUTLength>> known,
            Func<string, bool> isIntermediaryDelegate)
        {
            this.Name = name;
            this.Recording = recording;
            this.Father = father;
            this.Known = known;
            this.IsIntermediaryDelegate = isIntermediaryDelegate;
            IsIntermediary = IsIntermediaryDelegate(Name);
            RefreshClosed();
        }
        #endregion
        #endregion
    }
}
