#if NETFRAMEWORK
namespace System.Net;

/// <summary>
///     보안 소켓 계층(SSL) 또는 전송 계층 보안(TLS) 프로토콜을 지정합니다.
/// </summary>
[Flags]
public enum SecurityProtocolType
{
    /// <summary>
    ///     SSL 3.0 보안 프로토콜을 사용합니다.
    /// </summary>
    Ssl3 = 48,

    /// <summary>
    ///     TLS 1.0 보안 프로토콜을 사용합니다.
    /// </summary>
    Tls = 192,

    /// <summary>
    ///     TLS 1.1 보안 프로토콜을 사용합니다.
    /// </summary>
    Tls11 = 768,

    /// <summary>
    ///     TLS 1.2 보안 프로토콜을 사용합니다.
    /// </summary>
    Tls12 = 3072,

    /// <summary>
    ///     TLS 1.3 보안 프로토콜을 사용합니다.
    /// </summary>
    Tls13 = 12288,

    /// <summary>
    ///     시스템 기본값을 사용합니다.
    /// </summary>
    SystemDefault = 0
}
#endif
