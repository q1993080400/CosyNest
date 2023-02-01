namespace System.SafetyFrancis.Authentication;

/// <summary>
/// 这个委托可以将密码从明文转换为密文
/// </summary>
/// <param name="password">待转换的密码明文</param>
/// <returns></returns>
public delegate Task<string> PasswordConversion(string password);