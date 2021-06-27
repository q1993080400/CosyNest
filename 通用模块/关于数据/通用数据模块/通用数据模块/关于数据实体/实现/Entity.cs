using System.Collections.Generic;
using System.Design;
using System.Design.Direct;
using System.Linq;
using System.Performance;
using System.Reflection;

namespace System.DataFrancis
{
    /// <summary>
    /// 该类型是所有强类型实体类的基类
    /// </summary>
    public abstract record Entity : DataBase
    {
        #region 有关缓存
        #region 缓存所有属性
        /// <summary>
        /// 该字典缓存实体类中所有公开且可读可写的属性
        /// </summary>
        private static ICache<Type, IEnumerable<PropertyInfo>> CacheProperty { get; }
        = CreatePerformance.CacheThreshold<Type, IEnumerable<PropertyInfo>>
            (type => type.GetProperties().
            Where(x => x.IsAlmighty() && !x.DeclaringType!.IsAssignableFrom(typeof(Entity))).ToArray(), 100);
        #endregion
        #region 缓存架构
        /// <summary>
        /// 这个字典被用来缓存实体类的架构
        /// </summary>
        private static ICache<Type, ISchema> CacheSchema { get; }
        = CreatePerformance.CacheThreshold<Type, ISchema>
            (type => CreateDesign.Schema
            (CacheProperty[type].ToDictionary(x => (x.Name, x.PropertyType), true)), 100);
        #endregion
        #region 提取属性
        /// <summary>
        /// 获取一个实体类所有实例，公开，且可读可写的属性，
        /// 它们一般直接与业务有关
        /// </summary>
        /// <param name="type">实体类的类型，
        /// 如果它不继承自<see cref="Entity"/>，会引发一个异常</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperty(Type type)
            => typeof(Entity).IsAssignableFrom(type) ?
            CacheProperty[type] :
            throw new ArgumentException($"{type}不继承自{nameof(Entity)}");
        #endregion
        #endregion
        #region IData的实现
        #region 用来储存数据的字典
        protected override IRestrictedDictionary<string, object?> Data { get; }
        #endregion
        #region 获取架构
        private ISchema? SchemaFiled;

        public override ISchema? Schema
        {
            get => SchemaFiled ??= CacheSchema[GetType()];
            set => throw new NotSupportedException("本类型是强类型实体类，不允许写入架构约束");
        }
        #endregion
        #region 复制数据
        protected override IDirect CreateSelf()
            => this.GetTypeData().ConstructorCreate<IDirect>();
        #endregion
        #endregion
        #region 构造函数
        public Entity()
        {
            Data = CreateCollection.ReflectionDictionary(this, GetProperty(GetType()));
        }
        #endregion
    }
}
