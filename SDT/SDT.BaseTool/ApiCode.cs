using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public enum ApiCode:int
    {
        /// <summary>
        /// 系统繁忙
        /// </summary>
        SystemBusy = -1,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 请求成功，并创建了新资源
        /// </summary>
        Created = 2010,

        /// <summary>
        /// 已接收请求，尚未对其进行处理
        /// </summary>
        Accepted = 2020,

        /// <summary>
        /// 由于不可预知的原因，其元信息可能不正确
        /// </summary>
        NonAuthoritativeInformation = 2030,

        /// <summary>
        /// 请求处理成功，信息有意为空白
        /// </summary>
        NoContent = 2040,

        /// <summary>
        /// 请求处理成功，要求请求者重置相关数据
        /// </summary>
        ResetContent = 2050,

        /// <summary>
        /// 信息或请求只有部分被响应
        /// </summary>
        PartialContent = 2060,

        /// <summary>
        /// 绑定的成员已经在（多状态）响应之前的部分被列举，且未被再次包含
        /// </summary>
        AlreadyReported = 2080,

        /// <summary>
        /// 已经满足了对资源的请求
        /// </summary>
        IMUsed = 2260,

        /// <summary>
        /// 相关数据有多种表示
        /// </summary>
        MultipleChoices = 3000,

        /// <summary>
        /// 相关数据已被移动
        /// </summary>
        Moved = 3010,

        /// <summary>
        /// 相关数据已被重定向至指定位置
        /// </summary>
        Redirect = 3020,

        /// <summary>
        /// 对应当前请求的响应可以在另一个URI上被找到
        /// </summary>
        SeeOther = 3030,

        /// <summary>
        /// 相关数据无需做更改
        /// </summary>
        NotModified = 3040,

        /// <summary>
        /// 相关操作需要使用代理
        /// </summary>
        UseProxy = 3050,

        /// <summary>
        /// 服务器无法解析此请求
        /// </summary>
        BadRequest = 4000,

        /// <summary>
        /// 所请求的资源需要身份验证
        /// </summary>
        Unauthorized = 4010,

        /// <summary>
        /// 服务器拒绝此请求
        /// </summary>
        Forbidden = 4030,

        /// <summary>
        /// 请求的接口不存在
        /// </summary>
        NotFound = 4040,

        /// <summary>
        /// 无效的请求方式
        /// </summary>
        MethodNotAllowed = 4050,

        /// <summary>
        /// 已使用已有的相关信息，不再接受任何的其它资源
        /// </summary>
        NotAcceptable = 4060,

        /// <summary>
        /// 代理需要身份验证
        /// </summary>
        ProxyAuthenticationRequired = 4070,

        /// <summary>
        /// 客户端未在预知的时间内发送请求
        /// </summary>
        RequestTimeout = 4080,

        /// <summary>
        /// 此请求可能会造成冲突
        /// </summary>
        Conflict = 4090,

        /// <summary>
        /// 所请求的资源不再可用
        /// </summary>
        Gone = 4100,

        /// <summary>
        /// 内容长度不能为空
        /// </summary>
        LengthRequired = 4110,

        /// <summary>
        /// 没有满足一个或者多个验证条件
        /// </summary>
        PreconditionFailed = 4120,

        /// <summary>
        /// 提交的数据大小超过了处理范围
        /// </summary>
        RequestEntityTooLarge = 4130,

        /// <summary>
        /// 不支持的资源格式
        /// </summary>
        UnsupportedMediaType = 4150,

        /// <summary>
        /// 客户端已经要求文件的一部分（Byte serving），但服务器不能提供该部分
        /// </summary>
        RequestedRangeNotSatisfiable = 4160,

        /// <summary>
        /// 数据格式正确,但包含的语义错误
        /// </summary>
        UnprocessableEntity = 4220,

        /// <summary>
        /// 当前相关资源被锁定
        /// </summary>
        Locked = 4230,

        /// <summary>
        /// 由于之前某个请求错误，导致当前请求失败
        /// </summary>
        FailedDependency = 4240,

        /// <summary>
        /// 数据或资源发生并发式冲突
        /// </summary>
        PreconditionRequired = 4280,

        /// <summary>
        /// 在给定的时间内发生太多请求
        /// </summary>
        TooManyRequests = 4290,

        /// <summary>
        /// 请求头大小超出范围
        /// </summary>
        RequestHeaderFieldsTooLarge = 4310,

        /// <summary>
        /// 因为法律问题被拒绝
        /// </summary>
        UnavailableForLegalReasons = 4510,

        /// <summary>
        /// 相关数据缺失
        /// </summary>
        DataMissing = 4005,

        /// <summary>
        /// 无效的参数或数据
        /// </summary>
        InvalidData = 4009,

        /// <summary>
        /// 相关数据已过期
        /// </summary>
        DataExpired = 4015,

        /// <summary>
        /// 相关数据重复
        /// </summary>
        DataDuplication = 4025,

        /// <summary>
        /// 数据被限制
        /// </summary>
        DataLimit = 4029,

        /// <summary>
        /// 访问被限制
        /// </summary>
        AccessLimit = 4035,

        /// <summary>
        /// 无权限执行相关操作
        /// </summary>
        NonPrivileged = 4039,

        /// <summary>
        /// 目标用户未经允许
        /// </summary>
        NotAllowed = 4045,

        /// <summary>
        /// 功能暂未开放
        /// </summary>
        NotOpened = 4055,

        /// <summary>
        /// 之前的协议已被更改
        /// </summary>
        SwitchingProtocols = 4065,

        /// <summary>
        /// 已有相同或类似的请求正在处理
        /// </summary>
        Processing = 4075,

        /// <summary>
        /// 自定义约定码1
        /// </summary>
        CustomCode1 = 4661,

        /// <summary>
        /// 自定义约定码2
        /// </summary>
        CustomCode2 = 4662,

        /// <summary>
        /// 自定义约定码3
        /// </summary>
        CustomCode3 = 4663,

        /// <summary>
        /// 自定义约定码4
        /// </summary>
        CustomCode4 = 4664,

        /// <summary>
        /// 自定义约定码5
        /// </summary>
        CustomCode5 = 4665,

        /// <summary>
        /// 自定义约定码6
        /// </summary>
        CustomCode6 = 4666,

        /// <summary>
        /// 自定义约定码7
        /// </summary>
        CustomCode7 = 4667,

        /// <summary>
        /// 自定义约定码8
        /// </summary>
        CustomCode8 = 4668,

        /// <summary>
        /// 自定义约定码9
        /// </summary>
        CustomCode9 = 4669,

        /// <summary>
        /// 服务器发生错误
        /// </summary>
        InternalServerError = 5000,

        /// <summary>
        /// 服务器不支持请求的功能 
        /// </summary>
        NotImplemented = 5010,

        /// <summary>
        /// 网关错误
        /// </summary>
        BadGateway = 5020,

        /// <summary>
        /// 指示服务器暂时不可用，通常由于高负载或维护
        /// </summary>
        ServiceUnavailable = 5030,

        /// <summary>
        /// 网关执行请求超时
        /// </summary>
        GatewayTimeout = 5040,

        /// <summary>
        /// 服务器不支持所请求的版本号
        /// </summary>
        HttpVersionNotSupported = 5050,

        /// <summary>
        /// 服务器存在内部配置错误
        /// </summary>
        VariantAlsoNegotiates = 5060,

        /// <summary>
        /// 服务器无法存储完成请求所必须的内容
        /// </summary>
        InsufficientStorage = 5070,

        /// <summary>
        /// 服务器在处理请求时陷入死循环
        /// </summary>
        LoopDetected = 5080,

        /// <summary>
        /// 获取资源所需要的策略并没有被满足
        /// </summary>
        NotExtended = 5100,

        /// <summary>
        /// 客户端需要进行身份验证才能获得网络访问权限
        /// </summary>
        NetworkAuthenticationRequired = 5110,

        /// <summary>
        /// 发生未知错误
        /// </summary>
        UnknowError = 5200,

        /// <summary>
        /// 目标服务器宕机
        /// </summary>
        WebServerIsDown = 5210
    }
}
