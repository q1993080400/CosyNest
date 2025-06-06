﻿namespace System.DingDing;

/// <summary>
/// 钉钉OA审批操作类型
/// </summary>
public enum DingDingOAOperationType
{
    /// <summary>
    /// 正常执行任务
    /// </summary>
    [EnumDescribe(Describe = "正常执行任务")]
    Normal,
    /// <summary>
    /// 代理人执行任务
    /// </summary>
    [EnumDescribe(Describe = "正常执行任务")]
    Agent,
    /// <summary>
    /// 前加签任务
    /// </summary>
    [EnumDescribe(Describe = "前加签任务")]
    PreSigningTask,
    /// <summary>
    /// 后加签任务
    /// </summary>
    [EnumDescribe(Describe = "后加签任务")]
    PostSigningTask,
    /// <summary>
    /// 转交申请
    /// </summary>
    [EnumDescribe(Describe = "转交申请")]
    Transfer,
    /// <summary>
    /// 发起申请
    /// </summary>
    [EnumDescribe(Describe = "发起申请")]
    Launch,
    /// <summary>
    /// 终止（撤销）申请
    /// </summary>
    [EnumDescribe(Describe = "终止（撤销）申请")]
    Termination,
    /// <summary>
    /// 结束申请
    /// </summary>
    [EnumDescribe(Describe = "结束申请")]
    Finish,
    /// <summary>
    /// 添加评论
    /// </summary>
    [EnumDescribe(Describe = "添加评论")]
    AddComments,
    /// <summary>
    /// 退回评论
    /// </summary>
    [EnumDescribe(Describe = "退回评论")]
    ReturnComment,
    /// <summary>
    /// 抄送
    /// </summary>
    [EnumDescribe(Describe = "抄送")]
    CC,
    /// <summary>
    /// 自动执行任务，意义不明
    /// </summary>
    [EnumDescribe(Describe = "自动执行任务")]
    ExexuteTaskAuto
}
