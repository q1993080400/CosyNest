using System.Net;
using System.Text;

namespace System;

public static partial class ExtendNet
{
    //这个部分类专门声明有关Uri的扩展方法

    #region Web编码
    /// <summary>
    /// 将路径编码为兼容Web的格式
    /// </summary>
    /// <param name="path">要编码的路径</param>
    /// <returns></returns>
    public static string ToWebEncode(this StringOperate path)
    {
        #region 本地函数
        static string Fun(string path, char delimiter, Func<string, string>? recursion)
        {
            var split = path.Split(delimiter);
            var part = split.Select(x => recursion?.Invoke(x) ?? WebUtility.UrlEncode(x).Replace("+", "%20")).ToArray();
            return part.Join(delimiter.ToString());
        }
        #endregion
        return Fun(path.Text, '/',
            x => Fun(x, '?',
            x => Fun(x, '#', null)));
    }
    #endregion
}
