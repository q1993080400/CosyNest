using hyjiacan.py4n;

namespace System.Localization;

/// <summary>
/// 这个类型是一个为中文优化的文本比较器，
/// 它按照拼音进行排序
/// </summary>
sealed class ComparableStringChinese : IComparer<string>
{
    #region 接口实现
    public int Compare(string? x, string? y)
    {
        switch ((x, y))
        {
            case (null, null):
                return 0;
            case ({ }, null):
                return 1;
            case (null, { }):
                return -1;
            default:
                #region 获取拼音的本地函数
                static string GetPinYin(char c)
                    => PinyinUtil.IsHanzi(c) ?
                    Pinyin4Net.GetFirstPinyin(c)[0].ToString() :        //只取拼音首字母
                    $"0{c}";            //非汉字字符和汉字字符分开排列
                #endregion
                var comparer = Comparer<string>.Default;
                foreach (var (xPinYin, yPinYin) in x.Select(GetPinYin).ZipFill(y.Select(GetPinYin), static (x, y) => (x, y)))
                {
                    if (comparer.Compare(xPinYin, yPinYin) is { } c and not 0)
                        return c;
                }
                return 0;
        }
    }
    #endregion
}
