namespace System.DataFrancis;

/// <summary>
/// 对数据进行验证，并返回验证结果
/// </summary>
/// <param name="obj">待验证的数据实体</param>
/// <returns></returns>
public delegate VerificationResults DataVerify(object obj);
