
using System.Collections.Generic;
using System.DataFrancis;
using System.Linq;
using System.SafetyFrancis;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore
{
    /// <summary>
    /// 这个静态类可以用来创建通用的ASP.NET对象，
    /// 它们在前端或后端都有用处
    /// </summary>
    public static class CreateASP
    {
        #region 关于Json
        #region 获取序列化IIdentity的对象
        /// <summary>
        /// 获取一个可以序列化和反序列化<see cref="IIdentity"/>的对象
        /// </summary>
        public static SerializationBase<IIdentity> SerializationIdentity { get; }
        #endregion
        #region 常用Json序列化器
        /// <summary>
        /// 返回常用的Json序列化器，按照规范，
        /// 所有Web应用程序都应该添加这些序列化支持，
        /// 如果需要添加或删除本集合的元素，请在本集合被使用前执行这个操作
        /// </summary>
        public static IList<JsonConverter> SerializationCommon { get; } = new List<JsonConverter>();
        #endregion
        #endregion
        #region 获取提取身份验证信息的键名
        /// <summary>
        /// 获取从Cookies中提取身份验证信息的默认键名
        /// </summary>
        public const string AuthenticationKey = "Authentication";
        #endregion
        #region 静态构造函数
        static CreateASP()
        {
            SerializationIdentity = CreateJson.JsonMap<PseudoIIdentity, IIdentity>
                (value => value is null ? null : new()
                {
                    AuthenticationType = value.AuthenticationType,
                    Name = value.Name
                },
                vaule => vaule is null ? null : CreateSafety.Identity(vaule.AuthenticationType, vaule.Name));
            SerializationCommon.Add
                (CreateDataObj.JsonDirect, CreateJson.JsonArray,
                SerializationIdentity, CreateJson.JsonNum);
        }
        #region 私有辅助类
        private class PseudoIIdentity
        {
            public string? AuthenticationType { get; set; }
            public string? Name { get; set; }
        }
        #endregion
        #endregion
    }
}
