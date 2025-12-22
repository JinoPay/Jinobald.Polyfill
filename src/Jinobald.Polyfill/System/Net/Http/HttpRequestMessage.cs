using System.Net.Http.Headers;
using System.Text;

#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
///     HTTP 요청 메시지를 나타냅니다.
/// </summary>
public class HttpRequestMessage : IDisposable
{
    private readonly Dictionary<string, object?> _properties;
    private bool _disposed;
    private HttpContent? _content;
    private HttpMethod _method;
    private HttpRequestHeaders? _headers;
    private Uri? _requestUri;
    private Version _version;

    /// <summary>
    ///     <see cref="HttpRequestMessage" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HttpRequestMessage()
        : this(HttpMethod.Get, (Uri?)null)
    {
    }

    /// <summary>
    ///     HTTP 메서드 및 요청 <see cref="Uri" />를 사용하여 <see cref="HttpRequestMessage" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="method">HTTP 메서드입니다.</param>
    /// <param name="requestUri">요청할 <see cref="Uri" />입니다.</param>
    public HttpRequestMessage(HttpMethod method, Uri? requestUri)
    {
        _method = method ?? throw new ArgumentNullException(nameof(method));
        _requestUri = requestUri;
        _version = new Version(1, 1);
        _properties = new Dictionary<string, object?>();
    }

    /// <summary>
    ///     HTTP 메서드 및 요청 <see cref="Uri" />를 사용하여 <see cref="HttpRequestMessage" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="method">HTTP 메서드입니다.</param>
    /// <param name="requestUri">요청할 URI를 나타내는 문자열입니다.</param>
    public HttpRequestMessage(HttpMethod method, string? requestUri)
        : this(method, string.IsNullOrEmpty(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute))
    {
    }

    /// <summary>
    ///     HTTP 메시지의 콘텐츠를 가져오거나 설정합니다.
    /// </summary>
    public HttpContent? Content
    {
        get => _content;
        set
        {
            CheckDisposed();
            _content = value;
        }
    }

    /// <summary>
    ///     HTTP 요청 메시지에 사용할 HTTP 메서드를 가져오거나 설정합니다.
    /// </summary>
    public HttpMethod Method
    {
        get => _method;
        set
        {
            CheckDisposed();
            _method = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    ///     HTTP 요청 헤더의 컬렉션을 가져옵니다.
    /// </summary>
    public HttpRequestHeaders Headers
    {
        get
        {
            if (_headers == null)
            {
                _headers = new HttpRequestHeaders();
            }

            return _headers;
        }
    }

    /// <summary>
    ///     HTTP 요청의 속성 컬렉션을 가져옵니다.
    /// </summary>
    public IDictionary<string, object?> Properties => _properties;

    /// <summary>
    ///     HTTP 요청에 사용되는 <see cref="Uri" />를 가져오거나 설정합니다.
    /// </summary>
    public Uri? RequestUri
    {
        get => _requestUri;
        set
        {
            CheckDisposed();
            _requestUri = value;
        }
    }

    /// <summary>
    ///     HTTP 메시지 버전을 가져오거나 설정합니다.
    /// </summary>
    public Version Version
    {
        get => _version;
        set
        {
            CheckDisposed();
            _version = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    ///     <see cref="HttpRequestMessage" />에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     현재 개체를 나타내는 문자열을 반환합니다.
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Method: ");
        sb.Append(_method);
        sb.Append(", RequestUri: '");
        sb.Append(_requestUri?.ToString() ?? "<null>");
        sb.Append("', Version: ");
        sb.Append(_version);
        sb.Append(", Content: ");
        sb.Append(_content?.GetType().ToString() ?? "<null>");
        sb.Append(", Headers:\r\n");
        sb.Append(Headers.ToString());
        if (_content != null)
        {
            sb.Append(_content.Headers.ToString());
        }

        return sb.ToString();
    }

    /// <summary>
    ///     <see cref="HttpRequestMessage" />에서 사용하는 관리되지 않는 리소스를 해제하고, 필요한 경우 관리되는 리소스도 삭제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _content?.Dispose();
        }

        _disposed = true;
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
#endif
