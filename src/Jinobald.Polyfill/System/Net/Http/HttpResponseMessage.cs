#if NETFRAMEWORK
namespace System.Net.Http;

using System.Net;
using System.Net.Http.Headers;

/// <summary>
/// HTTP 응답 메시지를 나타냅니다.
/// </summary>
public class HttpResponseMessage : IDisposable
{
    private HttpStatusCode _statusCode;
    private HttpContent? _content;
    private HttpResponseHeaders? _headers;
    private string? _reasonPhrase;
    private HttpRequestMessage? _requestMessage;
    private Version _version;
    private bool _disposed;

    /// <summary>
    /// HTTP 메시지 버전을 가져오거나 설정합니다.
    /// </summary>
    public Version Version
    {
        get { return _version; }
        set
        {
            CheckDisposed();
            _version = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// HTTP 응답의 상태 코드를 가져오거나 설정합니다.
    /// </summary>
    public HttpStatusCode StatusCode
    {
        get { return _statusCode; }
        set
        {
            CheckDisposed();
            _statusCode = value;
        }
    }

    /// <summary>
    /// 서버에서 상태 코드와 함께 전송된 이유 구를 가져오거나 설정합니다.
    /// </summary>
    public string? ReasonPhrase
    {
        get { return _reasonPhrase ?? GetDefaultReasonPhrase(_statusCode); }
        set
        {
            CheckDisposed();
            _reasonPhrase = value;
        }
    }

    /// <summary>
    /// HTTP 응답 헤더의 컬렉션을 가져옵니다.
    /// </summary>
    public HttpResponseHeaders Headers
    {
        get
        {
            if (_headers == null)
                _headers = new HttpResponseHeaders();
            return _headers;
        }
    }

    /// <summary>
    /// HTTP 응답 메시지의 콘텐츠를 가져오거나 설정합니다.
    /// </summary>
    public HttpContent? Content
    {
        get { return _content; }
        set
        {
            CheckDisposed();
            _content = value;
        }
    }

    /// <summary>
    /// 이 응답 메시지를 생성한 요청 메시지를 가져오거나 설정합니다.
    /// </summary>
    public HttpRequestMessage? RequestMessage
    {
        get { return _requestMessage; }
        set
        {
            CheckDisposed();
            _requestMessage = value;
        }
    }

    /// <summary>
    /// HTTP 응답이 성공했는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsSuccessStatusCode
    {
        get { return (int)_statusCode >= 200 && (int)_statusCode <= 299; }
    }

    /// <summary>
    /// <see cref="HttpResponseMessage"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HttpResponseMessage()
        : this(HttpStatusCode.OK)
    {
    }

    /// <summary>
    /// 지정된 <see cref="HttpStatusCode"/>를 사용하여 <see cref="HttpResponseMessage"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="statusCode">HTTP 응답의 상태 코드입니다.</param>
    public HttpResponseMessage(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        _version = new Version(1, 1);
    }

    /// <summary>
    /// HTTP 응답이 성공했는지 확인하고 성공하지 않은 경우 예외를 throw합니다.
    /// </summary>
    /// <returns>호출 체인을 위한 HTTP 응답 메시지입니다.</returns>
    public HttpResponseMessage EnsureSuccessStatusCode()
    {
        if (!IsSuccessStatusCode)
        {
            var message = $"응답 상태 코드가 성공을 나타내지 않습니다: {(int)_statusCode} ({ReasonPhrase}).";
            throw new HttpRequestException(message);
        }

        return this;
    }

    /// <summary>
    /// 현재 개체를 나타내는 문자열을 반환합니다.
    /// </summary>
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("StatusCode: ");
        sb.Append((int)_statusCode);
        sb.Append(", ReasonPhrase: '");
        sb.Append(ReasonPhrase ?? string.Empty);
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
    /// <see cref="HttpResponseMessage"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="HttpResponseMessage"/>에서 사용하는 관리되지 않는 리소스를 해제하고, 필요한 경우 관리되는 리소스도 삭제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _content?.Dispose();
        }

        _disposed = true;
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }

    private static string? GetDefaultReasonPhrase(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.Continue: return "Continue";
            case HttpStatusCode.SwitchingProtocols: return "Switching Protocols";
            case HttpStatusCode.OK: return "OK";
            case HttpStatusCode.Created: return "Created";
            case HttpStatusCode.Accepted: return "Accepted";
            case HttpStatusCode.NoContent: return "No Content";
            case HttpStatusCode.ResetContent: return "Reset Content";
            case HttpStatusCode.PartialContent: return "Partial Content";
            case HttpStatusCode.MultipleChoices: return "Multiple Choices";
            case HttpStatusCode.MovedPermanently: return "Moved Permanently";
            case HttpStatusCode.Found: return "Found";
            case HttpStatusCode.SeeOther: return "See Other";
            case HttpStatusCode.NotModified: return "Not Modified";
            case HttpStatusCode.TemporaryRedirect: return "Temporary Redirect";
            case HttpStatusCode.BadRequest: return "Bad Request";
            case HttpStatusCode.Unauthorized: return "Unauthorized";
            case HttpStatusCode.Forbidden: return "Forbidden";
            case HttpStatusCode.NotFound: return "Not Found";
            case HttpStatusCode.MethodNotAllowed: return "Method Not Allowed";
            case HttpStatusCode.NotAcceptable: return "Not Acceptable";
            case HttpStatusCode.RequestTimeout: return "Request Timeout";
            case HttpStatusCode.Conflict: return "Conflict";
            case HttpStatusCode.Gone: return "Gone";
            case HttpStatusCode.LengthRequired: return "Length Required";
            case HttpStatusCode.PreconditionFailed: return "Precondition Failed";
            case HttpStatusCode.RequestEntityTooLarge: return "Request Entity Too Large";
            case HttpStatusCode.RequestUriTooLong: return "Request-URI Too Long";
            case HttpStatusCode.UnsupportedMediaType: return "Unsupported Media Type";
            case HttpStatusCode.InternalServerError: return "Internal Server Error";
            case HttpStatusCode.NotImplemented: return "Not Implemented";
            case HttpStatusCode.BadGateway: return "Bad Gateway";
            case HttpStatusCode.ServiceUnavailable: return "Service Unavailable";
            case HttpStatusCode.GatewayTimeout: return "Gateway Timeout";
            case HttpStatusCode.HttpVersionNotSupported: return "HTTP Version Not Supported";
            default: return null;
        }
    }
}
#endif
