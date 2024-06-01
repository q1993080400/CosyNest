﻿using System.DataFrancis;

namespace System.DingDing;

/// <summary>
/// 钉钉OA审批操作类型
/// </summary>
public enum DingDingOAOperationType
{
    /// <summary>
    /// 正常执行任务
    /// </summary>
    [RenderData(Name = "正常执行任务")]
    Normal,
    /// <summary>
    /// 代理人执行任务
    /// </summary>
    [RenderData(Name = "正常执行任务")]
    Agent,
    /// <summary>
    /// 前加签任务
    /// </summary>
    [RenderData(Name = "前加签任务")]
    PreSigningTask,
    /// <summary>
    /// 后加签任务
    /// </summary>
    [RenderData(Name = "后加签任务")]
    PostSigningTask,
    /// <summary>
    /// 转交任务
    /// </summary>
    [RenderData(Name = "转交任务")]
    Transfer,
    /// <summary>
    /// 发起流程实例
    /// </summary>
    [RenderData(Name = "发起流程实例")]
    Launch,
    /// <summary>
    /// 终止（撤销）流程实例
    /// </summary>
    [RenderData(Name = "终止（撤销）流程实例")]
    Termination,
    /// <summary>
    /// 结束流程实例
    /// </summary>
    [RenderData(Name = "结束流程实例")]
    Finish,
    /// <summary>
    /// 添加评论
    /// </summary>
    [RenderData(Name = "添加评论")]
    AddComments,
    /// <summary>
    /// 退回评论
    /// </summary>
    [RenderData(Name = "退回评论")]
    ReturnComment,
    /// <summary>
    /// 抄送
    /// </summary>
    [RenderData(Name = "抄送")]
    CC
}
