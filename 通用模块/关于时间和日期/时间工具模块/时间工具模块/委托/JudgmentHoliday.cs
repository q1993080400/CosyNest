namespace System.TimeFrancis;

/// <summary>
/// 判断一个日期是否是假日
/// </summary>
/// <param name="dateOnly">要判断的日期</param>
/// <returns>如果是假日，返回<see langword="true"/>，
/// 是工作日，返回<see langword="false"/></returns>
public delegate Task<bool> JudgmentHoliday(DateOnly dateOnly);