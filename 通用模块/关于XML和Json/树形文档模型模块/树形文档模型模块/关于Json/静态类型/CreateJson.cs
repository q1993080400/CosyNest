using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Maths;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个静态类被用来创建有关Json的对象
    /// </summary>
    public static class CreateJson
    {
        #region 创建序列化器
        #region 创建集合序列化器
        #region 直接创建
        /// <summary>
        /// 创建一个可序列化和反序列化集合的<see cref="SerializationBase{Output}"/>，
        /// 它会尝试使用一切可用的序列化器进行转换
        /// </summary>
        /// <typeparam name="Collections">可序列化和反序列化的集合类型</typeparam>
        /// <param name="getElementType">该委托传入集合的类型，返回集合元素的类型</param>
        /// <param name="createCollections">该委托的第一个参数是集合元素的类型，
        /// 第二个参数是枚举集合元素的枚举器，返回值就是新创建的，包含这些元素的集合</param>
        /// <returns></returns>
        public static SerializationBase<Collections> JsonCollections<Collections>
            (Func<Type, Type> getElementType,
            Func<Type, IEnumerable<object?>, Collections> createCollections)
            where Collections : IEnumerable
            => new SerializationCollections<Collections>(getElementType, createCollections);
        #endregion
        #region 返回数组序列化器
        /// <summary>
        /// 返回一个可序列化和反序列化数组的<see cref="SerializationBase{Output}"/>，
        /// 它会尝试使用一切可用的序列化器进行转换
        /// </summary>
        public static SerializationBase<Array> JsonArray { get; }
        = JsonCollections(
            x => x.GetElementType()!,
            (elementType, elements) =>
            {
                var array = elements.ToArray();
                var len = array.Length;
                var newArray = Array.CreateInstance(elementType, len);
                array.CopyTo(newArray, 0);
                return newArray;
            });
        #endregion
        #endregion
        #region 创建计量单位序列化器
        /// <summary>
        /// 创建一个可以序列化和反序列化<see cref="IUnit{Template}"/>的对象，
        /// 它将<see cref="IUnit{Template}"/>转换为表示公制单位数量的数字，
        /// 然后再执行序列化和反序列化
        /// </summary>
        /// <typeparam name="UT">计量单位的单位类型</typeparam>
        /// <returns></returns>
        public static SerializationBase<IUnit<UT>> JsonUnit<UT>()
            where UT : IUT
            => JsonMap<decimal?, IUnit<UT>>
            (x => x?.ValueMetric ?? 0, x => CreateBaseMath.UnitMetric<UT>(x ?? 0));
        #endregion
        #region 创建Num的序列化器
        /// <summary>
        /// 返回一个可以序列化和反序列化<see cref="Num"/>的对象
        /// </summary>
        public static SerializationBase<Num> JsonNum { get; }
        = JsonMap<decimal, Num>(x => x.Value, x => x);
        #endregion
        #region 创建映射类型序列化器
        /// <summary>
        /// 创建一个序列化器，它可以通过映射类型来序列化和反序列化对象
        /// </summary>
        /// <typeparam name="Map">映射类型的类型</typeparam>
        /// <typeparam name="Output">可序列化的类型</typeparam>
        /// <param name="toMap">将实际类型转换为映射类型的委托</param>
        /// <param name="fromMap">从映射类型转换为实际类型的委托</param>
        /// <returns></returns>
        public static SerializationBase<Output> JsonMap<Map, Output>
            (Func<Output?, Map?> toMap, Func<Map?, Output?> fromMap)
            => new MapSerialization<Map, Output>(toMap, fromMap);
        #endregion
        #endregion 
    }
}
