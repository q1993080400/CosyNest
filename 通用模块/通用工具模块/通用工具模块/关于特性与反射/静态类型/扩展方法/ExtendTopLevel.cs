using System.Reflection;

namespace System;

public static partial class ExtendReflection
{
    //该部分类专门声明比Type更高层次的类型，如模块，程序集等的反射

    #region 递归返回程序集中的所有类型和引用类型
    /// <summary>
    /// 递归返回程序集中的所有类型以及它所引用的所有类型，
    /// 注意：不返回未加载的类型，以及编译器生成的类型
    /// </summary>
    /// <param name="assembly">要返回类型的程序集</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetTypeAll(this Assembly assembly)
    {
        var assembliesDictionary = AppDomain.CurrentDomain.GetAssemblies().
            ToDictionary(x => x.GetName().FullName, x => x);
        #region 本地函数
        IEnumerable<Type> Fun(Assembly assembly)
        {
            foreach (var item in assembly.GetTypes().Where(x => !x.IsCompilerGenerated()))
            {
                yield return item;
            }
            foreach (var item in assembly.GetReferencedAssemblies())
            {
                var fullName = item.FullName;
                var subordinate = assembliesDictionary.TryGetValue(fullName).Value;
                if (subordinate is null)
                    continue;
                assembliesDictionary.Remove(fullName);
                foreach (var recursion in Fun(subordinate))
                {
                    yield return recursion;
                }
            }
        }
        #endregion
        return Fun(assembly).ToArray();
    }

    /*警告：
      不要随便调用本方法，对于大型项目的顶层程序集，
      本方法可能会返回几万个类型*/
    #endregion
}
