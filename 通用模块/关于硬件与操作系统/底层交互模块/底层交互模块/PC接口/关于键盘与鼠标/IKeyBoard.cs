namespace System.Underlying.PC;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来模拟键盘操作
/// </summary>
public interface IKeyBoard
{
    #region 按下键盘上的键
    /// <summary>
    /// 按下键盘上的键，然后将它们松开
    /// </summary>
    /// <param name="keys">要按下的键</param>
    void Down(params Keys[] keys);

    /*实现本接口请遵循以下规范：
      当要按下多个键时，如果第一个键是Ctrl，
      视为按下组合键，否则视为依次按下这些键并依次松开，
      这是为了与日常使用键盘的习惯相符合*/
    #endregion
}
