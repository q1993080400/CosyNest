using System.Localization;

using hyjiacan.py4n;

namespace System;

/// <summary>
/// 有关本地化的扩展方法全部放在这里
/// </summary>
public static class ExtentdLocalization
{
    #region 获取拼音
    #region 传入字符
    /// <summary>
    /// 获取字符的汉语拼音，并返回一个元组，
    /// 它的第一个项是是否可以获取拼音，
    /// 第二个项是获取到的拼音
    /// </summary>
    /// <param name="chat">要获取拼音的字符，
    /// 如果它是一个汉字，获取它的拼音，
    /// 如果它是一个英文字母，获取它的大写形式，
    /// 都不是，则返回<see langword="null"/></param>
    /// <returns></returns>
    public static (bool CanGetPinYin, string? PinYin) GetPinYin(this char @chat)
    {
        var isChinese = PinyinUtil.IsHanzi(@chat);
        var isEnglish = (int)@chat is (>= 65 and <= 90) or (>= 97 and <= 122);
        var pinYin = (isChinese, isEnglish) switch
        {
            (true, _) => Pinyin4Net.GetFirstPinyin(@chat).ToString(),
            (_, true) => @chat.ToString().ToUpper(),
            _ => null
        };
        return (isChinese || isEnglish, pinYin);
    }
    #endregion
    #region 传入字符串
    /// <summary>
    /// 获取字符串的汉语拼音，并返回一个元组，
    /// 它的第一个项是是否可以获取拼音，
    /// 第二个项是获取到的拼音
    /// </summary>
    /// <param name="text">要获取拼音的字符串，
    /// 取它的第一个字符，
    /// 如果它是一个汉字，获取它的拼音，
    /// 如果它是一个英文字母，获取它的大写形式，
    /// 都不是，则返回<see langword="null"/></param>
    /// <returns></returns>
    public static (bool CanGetPinYin, string? PinYin) GetPinYin(this string? text)
        => text.IsVoid() ? (false, null) : text[0].GetPinYin();
    #endregion
    #endregion
    #region 将对象按照拼音首字母分类
    /// <summary>
    /// 将对象按照拼音首字符分类，
    /// 这个分类按照中文的习惯被高度优化过
    /// </summary>
    /// <typeparam name="Obj">要分类的对象类型</typeparam>
    /// <param name="obj">要分类的对象</param>
    /// <param name="getKey">用来获取要分类的键的委托，它会被进一步用来获取拼音</param>
    /// <param name="notPinYin">如果键不能获取拼音，则会使用这个字符串，作为它的拼音</param>
    /// <returns></returns>
    public static GroupInitial<Obj>[] GroupByPinYin<Obj>(this IEnumerable<Obj> obj, Func<Obj, string> getKey, string notPinYin = "#")
        => [.. obj.Select(x=>{
            var key = getKey(x);
            var pinYin=key.GetPinYin().PinYin?[0].ToString().ToUpper() ?? notPinYin;
            return (Key:key,PinYin:pinYin,Value:x);
        }).OrderBy(x => x.PinYin == notPinYin ? 1 : 0).
            ThenBy(x=>x.PinYin).
            ThenBy(x=>x.Key).
            GroupBy(x =>x.PinYin).
            Select(x=>new GroupInitial<Obj>(x.Key,x.Select(x=>x.Value).ToArray()))];
    #endregion
}
