using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个沉降观测站
    /// </summary>
    class SettlementObservatory : SettlementBase, ISettlementObservatory
    {
        #region 高程
        public override IUnit<IUTLength> High
            => Father.To<ISettlement>()!.High + Recording!;
        #endregion
        #region 添加后代
        public ISettlementPoint Add(string name, IUnit<IUTLength> recording)
        {
            var father = Father.To<SettlementPoint>();
            var known = father!.Known;
            var isIntermediary = father.IsIntermediaryDelegate;
            var son = known.TryGetValue(name, out var high) ?
                new SettlementPoint(name, recording, high, this, known, isIntermediary) :
                new SettlementPoint(name, recording, this, known, isIntermediary);
            SonField.AddLast(son);
            return son;
        }
        #endregion
        #region 重写ToString
        public override string ToString()
            => "这是沉降观测站";
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="recording">原始记录</param>
        /// <param name="father">父节点</param>
        public SettlementObservatory(IUnit<IUTLength> recording, ISettlementPoint father)
        {
            this.Recording = recording;
            this.Father = father;
        }
        #endregion
    }
}
