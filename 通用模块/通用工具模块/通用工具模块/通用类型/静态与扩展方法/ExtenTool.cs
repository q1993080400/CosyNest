using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Maths;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 所有通用的扩展方法全部放在这个静态类中，通常无需专门调用
    /// </summary>
    public static class ExtenTool
    {
        #region 关于对象转化
        #region 转换任意对象
        #region 非泛型方法
        #region 缓存方法属性
        private static MethodInfo? ToMethodField;

        /// <summary>
        /// 该方法缓存<see cref="To{Ret}(object?, bool, LazyPro{Ret}?)"/>的方法对象，
        /// 警告：本方法有关寻找<see cref="MethodInfo"/>的逻辑存在缺陷，可能会产生潜在BUG
        /// </summary>
        private static MethodInfo ToMethod
            => ToMethodField ??= typeof(ExtenTool).GetMethods().First
            (x => x.Name is nameof(To) &&
            x.GetParameters().Length is 3 &&
            x.IsGenericMethod);
        #endregion
        #region 正式方法
        /// <inheritdoc cref="To{Ret}(object?, bool, LazyPro{Ret}?)"/>
        /// <param name="targetType">要转换的对象的类型</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("obj")]
        public static object? To(this object? obj, Type targetType, bool @throw = true)
            => ToMethod.MakeGenericMethod(targetType).Invoke<object>(null, obj, @throw, null);
        #endregion 
        #endregion
        #region 泛型方法
        /// <summary>
        /// 将一个对象转换为其他类型，可选是否在转换失败时抛出异常
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="obj">要转换的对象</param>
        /// <param name="throw">如果这个值为<see langword="true"/>，在转换失败时会抛出异常</param>
        /// <param name="notConvert">如果转换失败，而且不抛出异常，则返回这个值，默认为类型默认值</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("obj")]
        public static Ret? To<Ret>(this object? obj, bool @throw = true, LazyPro<Ret>? notConvert = default)
        {
            var type = typeof(Ret);
            try
            {
                switch (obj)
                {
                    case Ret a:
                        return a;
                    case { } a when Tool.SpecialConversion.TryGetValue((a.GetType(), type), out var convert):    //如果不能直接转换，则尝试调用已注册的特殊转换
                        return (Ret)convert(obj);
                    case IConvertible a:
                        return (Ret)Convert.ChangeType(a,
                             type.IsGenericRealize(typeof(Nullable<>)) ? type.GenericTypeArguments[0] : type);  //如果目标类型是可空值类型，则将其转换为它的泛型类型
                    case null when typeof(Ret).CanNull():
                        return default;
                    default:
                        dynamic? dy = obj;                                                                      //如果上述转换全部失败，则尝试调用该类型自定义的转换
                        return (Ret?)dy;
                }
                #region 说明文档
                /*说明：
                   上面那个switch看上去可以简化为这种形式：
                   
                  if(Obj is IConvertible a)
                  {
                      var toType = type.FullName.StartsWith("System.Nullable") ?          
                           type.GenericTypeArguments[0] : type;                                    
                      return (Ret)Convert.ChangeType(a, toType);
                  }
                  dynamic dy = Obj;
                  return (Ret)dy;
                
                  但实际上不可以，这是因为：
                  假如Obj实现了IConvertible，但是要转换的目标类型是Obj的子类
                  那么程序会错误的调用ChangeType，导致转换失败
                  这种情况虽然非常少见，但如果置之不理，会埋下一个隐患*/
                #endregion
            }
            catch (Exception) when (!@throw)
            {
                return notConvert;
            }
        }
        #endregion 
        #endregion
        #region 转换委托类型
        #region 直接传入新委托的类型
        /// <summary>
        /// 将一个委托转换为另一种委托类型，
        /// 前提条件是这两个委托签名必须相同
        /// </summary>
        /// <param name="oldDelegate">要转换的旧委托</param>
        /// <param name="newDelegateType">新委托的类型</param>
        /// <returns></returns>
        public static Delegate To(this Delegate oldDelegate, Type newDelegateType)
            => Delegate.CreateDelegate(newDelegateType, oldDelegate.Target, oldDelegate.Method);
        #endregion
        #region 使用泛型标明新委托类型
        /// <summary>
        /// 将一个委托转换为另一种委托类型，
        /// 前提条件是这两个委托签名必须相同
        /// </summary>
        /// <typeparam name="NewDel">新委托的类型</typeparam>
        /// <param name="oldDelegate">要转换的旧委托</param>
        /// <returns></returns>
        public static NewDel To<NewDel>(this Delegate oldDelegate)
            where NewDel : Delegate
            => (NewDel)oldDelegate.To(typeof(NewDel));
        #endregion
        #endregion
        #region 将一个枚举转换为和它等效的另一个枚举
        /// <summary>
        /// 将一个枚举转换为和它等效的另一个枚举
        /// </summary>
        /// <typeparam name="To">返回值类型，必须是一个枚举</typeparam>
        /// <param name="fromEnum">要转换的枚举</param>
        /// <returns></returns>
        public static To To<To>(this Enum fromEnum)
            where To : Enum
            => (To)Enum.ToObject(typeof(To), fromEnum);
        #endregion
        #endregion
        #region 关于枚举
        #region 枚举枚举的所有位域
        /// <summary>
        /// 枚举一个枚举的所有位域，
        /// 即便该枚举没有<see cref="FlagsAttribute"/>特性，也不受影响
        /// </summary>
        /// <typeparam name="Obj">待返回位域的枚举类型</typeparam>
        /// <param name="obj">待返回位域的枚举</param>
        /// <returns></returns>
        public static IEnumerable<Obj> AllFlag<Obj>(this Obj obj)
            where Obj : Enum
            => ToolBit.AllFlag(obj.To<int>(), 31).
            Select(x => (Obj)Enum.ToObject(typeof(Obj), x.Power));
        #endregion
        #region 从枚举中删除位域
        /// <summary>
        /// 从枚举中删除位域
        /// </summary>
        /// <typeparam name="Obj">待删除位域的枚举类型</typeparam>
        /// <param name="obj">待删除位域的枚举</param>
        /// <param name="remove">要删除的枚举位域</param>
        /// <returns></returns>
        public static Obj RemoveFlag<Obj>(this Obj obj, params Obj[] remove)
            where Obj : Enum
            => obj.AllFlag().Except(remove).
            Aggregate(default(Obj)!,
                (x, y) => (dynamic)x | y);
        #endregion
        #endregion
        #region 静态构造函数
        static ExtenTool()
        {
            Tool.SpecialConversion.Add
                ((typeof(string), typeof(DateTimeOffset)),
                x => DateTimeOffset.Parse((string)x));
        }
        #endregion
    }
}
