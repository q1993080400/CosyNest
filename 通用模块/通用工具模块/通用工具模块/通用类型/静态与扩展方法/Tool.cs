using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class Tool
    {
        #region 注册特殊转换方法
        /// <summary>
        /// 这个字典索引类型的特殊转换方法，
        /// 它可以被<see cref="ExtenTool.To{Ret}(object?, bool, LazyPro{Ret}?)"/>方法所识别，
        /// 字典的键是转换的原类型和目标类型，值是将原类型的对象转换为目标类型的委托
        /// </summary>
        public static IAddOnlyDictionary<(Type From, Type To), Func<object, object>> SpecialConversion { get; }
        = new ConcurrentDictionary<(Type From, Type To), Func<object, object>>().FitDictionary(false);
        #endregion
        #region 拷贝对象
        /// <summary>
        /// 通过反射拷贝对象，并返回它的副本
        /// </summary>
        /// <typeparam name="Ret">拷贝的返回值类型</typeparam>
        /// <param name="obj">被拷贝的对象</param>
        /// <param name="isShallow">如果这个值为真，则执行浅拷贝，否则执行深拷贝</param>
        /// <param name="exception">出现在这个集合中的字段或自动属性名将作为例外，不会被拷贝</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("obj")]
        public static Ret? Copy<Ret>(Ret? obj, bool isShallow = true, params string[] exception)
        {
            if (obj is null)
                return default;
            var type = obj.GetTypeData();
            var @new = type.ConstructorCreate<Ret>();
            var field = type.Fields.Where(x => !x.IsStatic);               //不拷贝静态属性
            if (exception.Length > 0)
            {
                var fieldName = exception.Union(exception).Select(x => $"<{x}>k__BackingField").ToHashSet();        //获取属性，以及该自动属性对应的字段名称
                field = field.Where(x => !fieldName.Contains(x.Name));
            }
            field.ForEach(x =>
            {
                var value = x.GetValue(obj);
                x.SetValue(@new,
                    isShallow || value is ValueType ? value : Copy(value, isShallow));
            });
            return @new!;
        }

        /*说明文档：
           例外的成员如果是属性，则必须是自动属性才能不被拷贝，
           这是因为自动属性所封装的字段都有固定格式的名称，
           而自己封装的属性的字段名称不确定
           
           obj必须拥有无参数构造函数，
           但如果Ret是obj的父类，可以没有无参数构造函数，
           这是因为程序实际上是在obj的类型中搜索构造函数*/
        #endregion
        #region 将两个对象的引用交换
        /// <summary>
        /// 将两个对象的引用交换
        /// </summary>
        /// <typeparam name="Obj">要交换的对象类型</typeparam>
        /// <param name="a">第一个对象</param>
        /// <param name="b">第二个对象</param>
        public static void Exchange<Obj>(ref Obj a, ref Obj b)
        {
            Obj c = b;
            b = a;
            a = c;
        }
        #endregion
        #region 调用异步事件
        /// <summary>
        /// 异步调用一个委托的所有调用列表，
        /// 这个方法通常用于异步事件
        /// </summary>
        /// <param name="delegate">要异步调用的委托</param>
        /// <param name="parameters">委托的参数列表</param>
        /// <exception cref="NotSupportedException">委托的任意一个调用列表不返回<see cref="Task"/>，
        /// 或返回<see langword="null"/></exception>
        /// <returns>一个<see cref="Task"/>，它用于等待委托的所有调用列表执行完毕</returns>
        public static Task AsyncInvoke(Delegate @delegate, params object[] parameters)
            => Task.WhenAll(@delegate.GetInvocationList().
                Select(x => x.DynamicInvoke(parameters) is Task t ?
                t : throw new NotSupportedException($"该委托的返回值不是{nameof(Task)}")));
        #endregion
    }
}
