#if NETFRAMEWORK
namespace System.Net.Http;

using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// URI로 식별되는 리소스에서 HTTP 요청을 보내고 HTTP 응답을 받기 위한 기본 클래스를 제공합니다.
/// </summary>
public class HttpClient : HttpMessageInvoker
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);
    private static readonly TimeSpan InfiniteTimeout = new TimeSpan(0, 0, 0, 0, -1);

    private Uri? _baseAddress;
    private TimeSpan _timeout;
    private long _maxResponseContentBufferSize;
    private HttpRequestHeaders? _defaultRequestHeaders;
    private volatile bool _disposed;
    private volatile bool _operationStarted;

    /// <summary>
    /// 각 요청과 함께 전송되어야 하는 헤더를 가져옵니다.
    /// </summary>
    public HttpRequestHeaders DefaultRequestHeaders
    {
        get
        {
            if (_defaultRequestHeaders == null)
                _defaultRequestHeaders = new HttpRequestHeaders();
            return _defaultRequestHeaders;
        }
    }

    /// <summary>
    /// 요청을 보낼 때 사용되는 인터넷 리소스의 URI(Uniform Resource Identifier)의 기본 주소를 가져오거나 설정합니다.
    /// </summary>
    public Uri? BaseAddress
    {
        get { return _baseAddress; }
        set
        {
            CheckDisposedOrStarted();
            _baseAddress = value;
        }
    }

    /// <summary>
    /// 요청이 시간 초과되기 전에 대기할 시간 범위를 가져오거나 설정합니다.
    /// </summary>
    public TimeSpan Timeout
    {
        get { return _timeout; }
        set
        {
            if (value != InfiniteTimeout && value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(value));
            CheckDisposedOrStarted();
            _timeout = value;
        }
    }

    /// <summary>
    /// 응답 콘텐츠를 읽을 때 버퍼링할 최대 바이트 수를 가져오거나 설정합니다.
    /// </summary>
    public long MaxResponseContentBufferSize
    {
        get { return _maxResponseContentBufferSize; }
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            CheckDisposedOrStarted();
            _maxResponseContentBufferSize = value;
        }
    }

    /// <summary>
    /// <see cref="HttpClient"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HttpClient()
        : this(new HttpClientHandler(), true)
    {
    }

    /// <summary>
    /// 지정된 핸들러를 사용하여 <see cref="HttpClient"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="handler">HTTP 응답 메시지를 처리하는 핸들러입니다.</param>
    public HttpClient(HttpMessageHandler handler)
        : this(handler, true)
    {
    }

    /// <summary>
    /// 지정된 핸들러를 사용하여 <see cref="HttpClient"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="handler">HTTP 응답 메시지를 처리하는 핸들러입니다.</param>
    /// <param name="disposeHandler">내부 핸들러를 Dispose할지 여부입니다.</param>
    public HttpClient(HttpMessageHandler handler, bool disposeHandler)
        : base(handler, disposeHandler)
    {
        _timeout = DefaultTimeout;
        _maxResponseContentBufferSize = int.MaxValue;
    }

    #region GET 메서드

    /// <summary>
    /// GET 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> GetAsync(string? requestUri)
    {
        return GetAsync(CreateUri(requestUri), HttpCompletionOption.ResponseContentRead, CancellationToken.None);
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri)
    {
        return GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="completionOption">작업이 완료된 것으로 간주되는 시점을 나타냅니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> GetAsync(string? requestUri, HttpCompletionOption completionOption)
    {
        return GetAsync(CreateUri(requestUri), completionOption, CancellationToken.None);
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> GetAsync(string? requestUri, CancellationToken cancellationToken)
    {
        return GetAsync(CreateUri(requestUri), HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="completionOption">작업이 완료된 것으로 간주되는 시점을 나타냅니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        return SendAsync(CreateRequestMessage(HttpMethod.Get, requestUri), completionOption, cancellationToken);
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 문자열로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<string> GetStringAsync(string? requestUri)
    {
        return GetStringAsync(CreateUri(requestUri));
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 문자열로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<string> GetStringAsync(Uri? requestUri)
    {
        return Task.Run(() =>
        {
            var response = GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None).Result;
            response.EnsureSuccessStatusCode();
            return response.Content?.ReadAsString() ?? string.Empty;
        });
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 바이트 배열로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<byte[]> GetByteArrayAsync(string? requestUri)
    {
        return GetByteArrayAsync(CreateUri(requestUri));
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 바이트 배열로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<byte[]> GetByteArrayAsync(Uri? requestUri)
    {
        return Task.Run(() =>
        {
            var response = GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None).Result;
            response.EnsureSuccessStatusCode();
            return response.Content?.ReadAsByteArray() ?? new byte[0];
        });
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 스트림으로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<Stream> GetStreamAsync(string? requestUri)
    {
        return GetStreamAsync(CreateUri(requestUri));
    }

    /// <summary>
    /// GET 요청을 지정된 URI에 보내고 응답 본문을 스트림으로 비동기 작업에서 반환합니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<Stream> GetStreamAsync(Uri? requestUri)
    {
        return Task.Run(() =>
        {
            var response = GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None).Result;
            response.EnsureSuccessStatusCode();
            return response.Content?.ReadAsStream() ?? new MemoryStream();
        });
    }

    #endregion

    #region POST 메서드

    /// <summary>
    /// POST 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content)
    {
        return PostAsync(CreateUri(requestUri), content, CancellationToken.None);
    }

    /// <summary>
    /// POST 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content)
    {
        return PostAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>
    /// POST 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        return PostAsync(CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>
    /// POST 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        var request = CreateRequestMessage(HttpMethod.Post, requestUri);
        request.Content = content;
        return SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    #endregion

    #region PUT 메서드

    /// <summary>
    /// PUT 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content)
    {
        return PutAsync(CreateUri(requestUri), content, CancellationToken.None);
    }

    /// <summary>
    /// PUT 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content)
    {
        return PutAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>
    /// PUT 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        return PutAsync(CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>
    /// PUT 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        var request = CreateRequestMessage(HttpMethod.Put, requestUri);
        request.Content = content;
        return SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    #endregion

    #region DELETE 메서드

    /// <summary>
    /// DELETE 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> DeleteAsync(string? requestUri)
    {
        return DeleteAsync(CreateUri(requestUri), CancellationToken.None);
    }

    /// <summary>
    /// DELETE 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri)
    {
        return DeleteAsync(requestUri, CancellationToken.None);
    }

    /// <summary>
    /// DELETE 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> DeleteAsync(string? requestUri, CancellationToken cancellationToken)
    {
        return DeleteAsync(CreateUri(requestUri), cancellationToken);
    }

    /// <summary>
    /// DELETE 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken)
    {
        return SendAsync(CreateRequestMessage(HttpMethod.Delete, requestUri), HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    #endregion

    #region PATCH 메서드

    /// <summary>
    /// PATCH 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PatchAsync(string? requestUri, HttpContent? content)
    {
        return PatchAsync(CreateUri(requestUri), content, CancellationToken.None);
    }

    /// <summary>
    /// PATCH 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content)
    {
        return PatchAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>
    /// PATCH 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PatchAsync(string? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        return PatchAsync(CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>
    /// PATCH 요청을 지정된 URI에 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="requestUri">요청이 전송되는 URI입니다.</param>
    /// <param name="content">서버로 보낼 HTTP 요청 콘텐츠입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
    {
        var request = CreateRequestMessage(HttpMethod.Patch, requestUri);
        request.Content = content;
        return SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    #endregion

    #region Send 메서드

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="completionOption">작업이 완료된 것으로 간주되는 시점을 나타냅니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
    {
        return SendAsync(request, completionOption, CancellationToken.None);
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="completionOption">작업이 완료된 것으로 간주되는 시점을 나타냅니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        CheckDisposed();
        SetOperationStarted();
        PrepareRequestMessage(request);

        return base.SendAsync(request, cancellationToken);
    }

    #endregion

    /// <summary>
    /// 보류 중인 모든 요청을 취소합니다.
    /// </summary>
    public void CancelPendingRequests()
    {
        CheckDisposed();
        // 현재 구현에서는 각 요청이 독립적으로 처리되므로 특별한 취소 로직이 필요하지 않음
    }

    /// <summary>
    /// <see cref="HttpClient"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
        }

        base.Dispose(disposing);
    }

    private Uri? CreateUri(string? uri)
    {
        if (string.IsNullOrEmpty(uri))
            return null;

        if (Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var result))
            return result;

        return null;
    }

    private HttpRequestMessage CreateRequestMessage(HttpMethod method, Uri? uri)
    {
        var request = new HttpRequestMessage(method, uri);
        return request;
    }

    private void PrepareRequestMessage(HttpRequestMessage request)
    {
        // 기본 주소 적용
        if (_baseAddress != null && request.RequestUri != null && !request.RequestUri.IsAbsoluteUri)
        {
            request.RequestUri = new Uri(_baseAddress, request.RequestUri);
        }
        else if (_baseAddress != null && request.RequestUri == null)
        {
            request.RequestUri = _baseAddress;
        }

        // 기본 헤더 적용
        if (_defaultRequestHeaders != null)
        {
            foreach (var header in _defaultRequestHeaders)
            {
                if (!request.Headers.Contains(header.Key))
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }

    private void SetOperationStarted()
    {
        if (!_operationStarted)
            _operationStarted = true;
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }

    private void CheckDisposedOrStarted()
    {
        CheckDisposed();
        if (_operationStarted)
            throw new InvalidOperationException("이 속성은 작업이 시작된 후에는 수정할 수 없습니다.");
    }
}

/// <summary>
/// HTTP 응답 메시지를 보내고 받는 데 사용되는 기본 형식입니다.
/// </summary>
public class HttpMessageInvoker : IDisposable
{
    private readonly HttpMessageHandler _handler;
    private readonly bool _disposeHandler;
    private volatile bool _disposed;

    /// <summary>
    /// 지정된 <see cref="HttpMessageHandler"/>를 사용하여 <see cref="HttpMessageInvoker"/> 클래스의 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="handler">HTTP 응답 메시지를 처리하는 핸들러입니다.</param>
    public HttpMessageInvoker(HttpMessageHandler handler)
        : this(handler, true)
    {
    }

    /// <summary>
    /// 지정된 <see cref="HttpMessageHandler"/>를 사용하여 <see cref="HttpMessageInvoker"/> 클래스의 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="handler">HTTP 응답 메시지를 처리하는 핸들러입니다.</param>
    /// <param name="disposeHandler">내부 핸들러를 Dispose할지 여부입니다.</param>
    public HttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _disposeHandler = disposeHandler;
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        CheckDisposed();
        return _handler.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// <see cref="HttpMessageInvoker"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="HttpMessageInvoker"/>에서 사용하는 관리되지 않는 리소스를 해제하고, 필요한 경우 관리되는 리소스도 삭제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            if (_disposeHandler)
            {
                _handler.Dispose();
            }
        }
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }
}

/// <summary>
/// HTTP 작업이 완료되는 시점을 나타냅니다.
/// </summary>
public enum HttpCompletionOption
{
    /// <summary>
    /// 콘텐츠를 포함하여 전체 응답을 읽은 후 작업이 완료됩니다.
    /// </summary>
    ResponseContentRead = 0,

    /// <summary>
    /// 응답을 사용할 수 있고 헤더를 읽은 직후 작업이 완료됩니다.
    /// </summary>
    ResponseHeadersRead = 1
}
#endif
