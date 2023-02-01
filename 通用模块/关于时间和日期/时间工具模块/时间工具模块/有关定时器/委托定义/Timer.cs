namespace System.TimeFrancis;

/// <summary>
/// 这个委托是一个定时器，
/// 它可以返回一个在定时器到期时等待完成的<see cref="Task"/>
/// </summary>
/// <returns></returns>
public delegate Task Timer();
