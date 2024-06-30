using hyjiacan.py4n;

namespace System;

/// <summary>
/// 有关本地化的扩展方法全部放在这里
/// </summary>
public static class ExtentLocalization
{
    #region 获取拼音
    /// <summary>
    /// 获取字符的汉语拼音，并返回一个元组，
    /// 它的第一个项是是否可以获取拼音，
    /// 第二个项是获取到的拼音
    /// </summary>
    /// <param name="chat">要获取拼音的字符，
    /// 如果它是一个汉字，获取它的拼音，
    /// 如果它是一个英文字母，获取它的大写形式，
    /// 都不是，则获取<see langword="null"/></param>
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
}
