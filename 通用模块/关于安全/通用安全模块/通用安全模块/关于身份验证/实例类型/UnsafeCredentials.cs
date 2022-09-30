namespace System.SafetyFrancis.Authentication;

/// <summary>
/// 代表一个不安全的凭据，
/// 它直接封装了用户名和密码
/// </summary>
/// <param name="ID">凭据的用户名</param>
/// <param name="Password">凭据的密码</param>
public record UnsafeCredentials(string ID, string Password)
{
    #region 重要说明
    /*注意：根据net规范，不应该明文存储密码，
      因此本类型的使用应该被严格控制在以下范围：
      1.完全受信任的环境
      2.学习和测试
      3.确实需要明文储存密码的场合，
      但作者仍然认为需要有更好的办法来解决这个问题，
      在找到了新办法以后，请不要在这个场合继续使用本类型*/
    #endregion
    #region 用户名
    /// <summary>
    /// 获取凭据的用户名
    /// </summary>
    public string ID { get; init; } = ID;
    #endregion
    #region 密码
    /// <summary>
    /// 获取凭据的密码
    /// </summary>
    public string Password { get; init; } = Password;
    #endregion
}
