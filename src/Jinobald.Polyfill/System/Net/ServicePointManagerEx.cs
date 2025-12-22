using System.Net.Security;
using System.Reflection;

#if NETFRAMEWORK
namespace System.Net;

/// <summary>
///     ServicePoint 개체의 연결을 관리합니다.
///     TLS 1.2까지 지원하도록 확장된 폴리필 버전입니다.
/// </summary>
/// <remarks>
///     .NET 3.5 및 .NET 4.0에서 TLS 1.1/1.2를 사용하려면 레지스트리 설정이 필요할 수 있습니다.
///     Windows 7 SP1/Server 2008 R2 SP1 이상에서 지원됩니다.
/// </remarks>
public static class ServicePointManagerEx
{
    private static bool _isInitialized;
    private static readonly object _lock = new();
    private static RemoteCertificateValidationCallback? _serverCertificateValidationCallback;
    private static SecurityProtocolType _securityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

    /// <summary>
    ///     서버 인증서 유효성 검사를 위한 콜백을 가져오거나 설정합니다.
    /// </summary>
    public static RemoteCertificateValidationCallback? ServerCertificateValidationCallback
    {
        get => _serverCertificateValidationCallback;
        set
        {
            _serverCertificateValidationCallback = value;
            ApplyServerCertificateValidationCallback();
        }
    }

    /// <summary>
    ///     ServicePoint 개체에서 사용하는 보안 프로토콜을 가져오거나 설정합니다.
    /// </summary>
    public static SecurityProtocolType SecurityProtocol
    {
        get
        {
            EnsureInitialized();
            return _securityProtocol;
        }
        set
        {
            EnsureInitialized();
            lock (_lock)
            {
                _securityProtocol = value;
                ApplySecurityProtocol();
            }
        }
    }

    /// <summary>
    ///     TLS 1.2까지 모든 보안 프로토콜을 활성화합니다.
    /// </summary>
    public static void EnableTls12()
    {
        SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
    }

    /// <summary>
    ///     최신 TLS 프로토콜(TLS 1.2 및 1.3)만 사용하도록 설정합니다.
    /// </summary>
    /// <remarks>
    ///     TLS 1.3은 Windows 10 버전 1903 이상 및 Windows Server 2022에서만 지원됩니다.
    /// </remarks>
    public static void UseModernTlsOnly()
    {
        SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
    }

    /// <summary>
    ///     TLS 1.2만 사용하도록 설정합니다 (권장).
    /// </summary>
    public static void UseTls12Only()
    {
        SecurityProtocol = SecurityProtocolType.Tls12;
    }

    private static void ApplySecurityProtocol()
    {
        try
        {
            // .NET Framework의 실제 ServicePointManager에 값 적용
            // 리플렉션을 사용하여 SecurityProtocol 속성에 접근
            Type? servicePointManagerType = typeof(ServicePoint).Assembly
                .GetType("System.Net.ServicePointManager");

            if (servicePointManagerType != null)
            {
                PropertyInfo? securityProtocolProperty = servicePointManagerType.GetProperty(
                    "SecurityProtocol",
                    BindingFlags.Public | BindingFlags.Static);

                if (securityProtocolProperty != null)
                {
                    // 프레임워크의 SecurityProtocolType으로 변환
                    Type? frameworkSecurityProtocolType = servicePointManagerType.Assembly
                        .GetType("System.Net.SecurityProtocolType");

                    if (frameworkSecurityProtocolType != null)
                    {
                        int value = (int)_securityProtocol;
                        object enumValue = Enum.ToObject(frameworkSecurityProtocolType, value);
                        securityProtocolProperty.SetValue(null, enumValue, null);
                    }
                }
            }
        }
        catch
        {
            // 리플렉션 실패 시 무시 (일부 프레임워크 버전에서는 지원되지 않을 수 있음)
        }
    }

    private static void ApplyServerCertificateValidationCallback()
    {
        try
        {
            Type? servicePointManagerType = typeof(ServicePoint).Assembly
                .GetType("System.Net.ServicePointManager");

            if (servicePointManagerType != null)
            {
                PropertyInfo? callbackProperty = servicePointManagerType.GetProperty(
                    "ServerCertificateValidationCallback",
                    BindingFlags.Public | BindingFlags.Static);

                if (callbackProperty != null)
                {
                    callbackProperty.SetValue(null, _serverCertificateValidationCallback, null);
                }
            }
        }
        catch
        {
            // 리플렉션 실패 시 무시
        }
    }

    private static void EnsureInitialized()
    {
        if (_isInitialized)
        {
            return;
        }

        lock (_lock)
        {
            if (_isInitialized)
            {
                return;
            }

            // 기본값으로 TLS 1.0, 1.1, 1.2 활성화 시도
            try
            {
                _securityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ApplySecurityProtocol();
            }
            catch
            {
                // 실패 시 기본값 유지
                _securityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            }

            _isInitialized = true;
        }
    }
}
#endif
