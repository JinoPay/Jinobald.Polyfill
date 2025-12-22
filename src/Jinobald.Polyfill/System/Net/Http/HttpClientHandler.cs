using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
#if NET40 || NET45 || NET451 || NET452
using System.Threading.Tasks;
#endif

#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
///     <see cref="HttpClient" />에서 사용하는 기본 메시지 핸들러입니다.
/// </summary>
public class HttpClientHandler : HttpMessageHandler
{
    private bool _allowAutoRedirect = true;
    private bool _disposed;
    private bool _preAuthenticate;
    private bool _useCookies = true;
    private bool _useProxy = true;
    private CookieContainer? _cookieContainer;
    private DecompressionMethods _automaticDecompression = DecompressionMethods.None;

    private Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>?
        _serverCertificateCustomValidationCallback;

    private ICredentials? _credentials;
    private int _maxAutomaticRedirections = 50;
    private IWebProxy? _proxy;
    private long _maxRequestContentBufferSize = int.MaxValue;
    private TimeSpan _timeout = TimeSpan.FromSeconds(100);
    private X509CertificateCollection? _clientCertificates;

    /// <summary>
    ///     <see cref="HttpClientHandler" /> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HttpClientHandler()
    {
        // TLS 1.2 활성화
        ServicePointManagerEx.EnableTls12();
    }

    /// <summary>
    ///     핸들러가 리디렉션 응답을 따르는지 여부를 가져오거나 설정합니다.
    /// </summary>
    public bool AllowAutoRedirect
    {
        get => _allowAutoRedirect;
        set
        {
            CheckDisposed();
            _allowAutoRedirect = value;
        }
    }

    /// <summary>
    ///     핸들러가 요청과 함께 인증 정보를 보내는지 여부를 가져오거나 설정합니다.
    /// </summary>
    public bool PreAuthenticate
    {
        get => _preAuthenticate;
        set
        {
            CheckDisposed();
            _preAuthenticate = value;
        }
    }

    /// <summary>
    ///     핸들러가 쿠키를 사용하여 쿠키를 저장하고 요청을 보낼 때 이 쿠키를 사용할지 여부를 가져오거나 설정합니다.
    /// </summary>
    public bool UseCookies
    {
        get => _useCookies;
        set
        {
            CheckDisposed();
            _useCookies = value;
        }
    }

    /// <summary>
    ///     핸들러가 프록시를 사용하여 요청을 보낼지 여부를 가져오거나 설정합니다.
    /// </summary>
    public bool UseProxy
    {
        get => _useProxy;
        set
        {
            CheckDisposed();
            _useProxy = value;
        }
    }

    /// <summary>
    ///     서버에서 보낸 쿠키를 저장하는 데 사용되는 쿠키 컨테이너를 가져오거나 설정합니다.
    /// </summary>
    public CookieContainer CookieContainer
    {
        get
        {
            if (_cookieContainer == null)
            {
                _cookieContainer = new CookieContainer();
            }

            return _cookieContainer;
        }
        set
        {
            CheckDisposed();
            _cookieContainer = value;
        }
    }

    /// <summary>
    ///     자동 압축 해제에 사용되는 압축 해제 방법의 형식을 가져오거나 설정합니다.
    /// </summary>
    public DecompressionMethods AutomaticDecompression
    {
        get => _automaticDecompression;
        set
        {
            CheckDisposed();
            _automaticDecompression = value;
        }
    }

    /// <summary>
    ///     서버 인증서 유효성 검사를 위한 콜백 메서드를 가져오거나 설정합니다.
    /// </summary>
    public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>?
        ServerCertificateCustomValidationCallback
    {
        get => _serverCertificateCustomValidationCallback;
        set
        {
            CheckDisposed();
            _serverCertificateCustomValidationCallback = value;
        }
    }

    /// <summary>
    ///     이 핸들러에서 사용하는 인증 정보를 가져오거나 설정합니다.
    /// </summary>
    public ICredentials? Credentials
    {
        get => _credentials;
        set
        {
            CheckDisposed();
            _credentials = value;
        }
    }

    /// <summary>
    ///     핸들러가 따르는 최대 리디렉션 수를 가져오거나 설정합니다.
    /// </summary>
    public int MaxAutomaticRedirections
    {
        get => _maxAutomaticRedirections;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CheckDisposed();
            _maxAutomaticRedirections = value;
        }
    }

    /// <summary>
    ///     핸들러에서 사용하는 프록시 정보를 가져오거나 설정합니다.
    /// </summary>
    public IWebProxy? Proxy
    {
        get => _proxy;
        set
        {
            CheckDisposed();
            _proxy = value;
        }
    }

    /// <summary>
    ///     요청 콘텐츠를 버퍼링할 때 사용할 최대 요청 콘텐츠 버퍼 크기(바이트)를 가져오거나 설정합니다.
    /// </summary>
    public long MaxRequestContentBufferSize
    {
        get => _maxRequestContentBufferSize;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CheckDisposed();
            _maxRequestContentBufferSize = value;
        }
    }

    /// <summary>
    ///     요청에 사용할 클라이언트 인증서의 컬렉션을 가져옵니다.
    /// </summary>
    public X509CertificateCollection ClientCertificates
    {
        get
        {
            if (_clientCertificates == null)
            {
                _clientCertificates = new X509CertificateCollection();
            }

            return _clientCertificates;
        }
    }

    /// <summary>
    ///     <see cref="HttpClientHandler" />에서 사용하는 관리되지 않는 리소스를 해제합니다.
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

    private static void CopyStream(Stream source, Stream destination)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
        }
    }

    private HttpResponseMessage CreateResponseMessage(HttpWebResponse webResponse, HttpRequestMessage request)
    {
        var response = new HttpResponseMessage(webResponse.StatusCode)
        {
            ReasonPhrase = webResponse.StatusDescription,
            Version = webResponse.ProtocolVersion,
            RequestMessage = request
        };

        // 응답 헤더 복사
        foreach (string headerName in webResponse.Headers)
        {
            string? headerValue = webResponse.Headers[headerName];
            if (!string.IsNullOrEmpty(headerValue))
            {
                response.Headers.TryAddWithoutValidation(headerName, headerValue);
            }
        }

        // 응답 콘텐츠 설정
        Stream? responseStream = webResponse.GetResponseStream();
        if (responseStream != null)
        {
            // 스트림을 메모리로 복사 (WebResponse는 곧 dispose될 수 있으므로)
            var memoryStream = new MemoryStream();
            CopyStream(responseStream, memoryStream);
            memoryStream.Position = 0;

            response.Content = new StreamContent(memoryStream);

            // 콘텐츠 헤더 복사
            string contentType = webResponse.ContentType;
            if (!string.IsNullOrEmpty(contentType))
            {
                response.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            }

            long contentLength = webResponse.ContentLength;
            if (contentLength >= 0)
            {
                response.Content.Headers.ContentLength = contentLength;
            }

            string contentEncoding = webResponse.ContentEncoding;
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                response.Content.Headers.TryAddWithoutValidation("Content-Encoding", contentEncoding);
            }
        }

        // 쿠키 업데이트
        if (_useCookies && _cookieContainer != null && webResponse.Cookies.Count > 0)
        {
            foreach (Cookie cookie in webResponse.Cookies)
            {
                _cookieContainer.Add(cookie);
            }
        }

        return response;
    }

    private HttpResponseMessage SendCore(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CheckDisposed();

        if (request.RequestUri == null)
        {
            throw new InvalidOperationException("요청 URI가 설정되지 않았습니다.");
        }

        HttpWebRequest webRequest = CreateWebRequest(request);

        try
        {
            // 요청 본문 쓰기
            if (request.Content != null)
            {
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    request.Content.CopyTo(requestStream);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            // 응답 받기
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            return CreateResponseMessage(webResponse, request);
        }
        catch (WebException ex) when (ex.Response is HttpWebResponse errorResponse)
        {
            return CreateResponseMessage(errorResponse, request);
        }
        catch (WebException ex)
        {
            throw new HttpRequestException(ex.Message, ex);
        }
    }

    private HttpWebRequest CreateWebRequest(HttpRequestMessage request)
    {
        var webRequest = (HttpWebRequest)WebRequest.Create(request.RequestUri!);

        webRequest.Method = request.Method.Method;
        webRequest.AllowAutoRedirect = _allowAutoRedirect;
        webRequest.AutomaticDecompression = _automaticDecompression;
        webRequest.PreAuthenticate = _preAuthenticate;
        webRequest.Timeout = (int)_timeout.TotalMilliseconds;

        if (_maxAutomaticRedirections > 0)
        {
            webRequest.MaximumAutomaticRedirections = _maxAutomaticRedirections;
        }

        if (_useCookies && _cookieContainer != null)
        {
            webRequest.CookieContainer = _cookieContainer;
        }

        if (_useProxy && _proxy != null)
        {
            webRequest.Proxy = _proxy;
        }

        if (_credentials != null)
        {
            webRequest.Credentials = _credentials;
        }

        if (_clientCertificates != null && _clientCertificates.Count > 0)
        {
            foreach (X509Certificate cert in _clientCertificates)
            {
                webRequest.ClientCertificates.Add(cert);
            }
        }

        // 서버 인증서 유효성 검사 콜백 설정
        if (_serverCertificateCustomValidationCallback != null)
        {
            HttpRequestMessage requestCopy = request;
            Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? callbackCopy =
                _serverCertificateCustomValidationCallback;

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                var cert2 = certificate as X509Certificate2;
                if (cert2 == null && certificate != null)
                {
                    cert2 = new X509Certificate2(certificate);
                }

                return callbackCopy(requestCopy, cert2, chain, sslPolicyErrors);
            };
        }

        // 요청 헤더 복사
        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            string? headerName = header.Key;
            string headerValue = string.Join(", ", new List<string>(header.Value).ToArray());

            try
            {
                switch (headerName.ToLowerInvariant())
                {
                    case "accept":
                        webRequest.Accept = headerValue;
                        break;
                    case "connection":
                        if (headerValue.IndexOf("keep-alive", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            webRequest.KeepAlive = true;
                        }
                        else if (headerValue.IndexOf("close", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            webRequest.KeepAlive = false;
                        }

                        break;
                    case "content-length":
                        // 콘텐츠 길이는 자동으로 설정됨
                        break;
                    case "content-type":
                        webRequest.ContentType = headerValue;
                        break;
                    case "expect":
                        if (headerValue.IndexOf("100-continue", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            webRequest.ServicePoint.Expect100Continue = true;
                        }

                        break;
                    case "host":
                        // Host 속성은 .NET 4.0 이상에서만 사용 가능, 헤더로 직접 설정
                        webRequest.Headers["Host"] = headerValue;
                        break;
                    case "if-modified-since":
                        if (DateTime.TryParse(headerValue, out DateTime ifModifiedSince))
                        {
                            webRequest.IfModifiedSince = ifModifiedSince;
                        }

                        break;
                    case "referer":
                        webRequest.Referer = headerValue;
                        break;
                    case "transfer-encoding":
                        if (headerValue.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            webRequest.SendChunked = true;
                        }

                        break;
                    case "user-agent":
                        webRequest.UserAgent = headerValue;
                        break;
                    default:
                        webRequest.Headers[headerName] = headerValue;
                        break;
                }
            }
            catch
            {
                // 일부 헤더는 설정할 수 없음
            }
        }

        // 콘텐츠 헤더 복사
        if (request.Content != null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
            {
                string? headerName = header.Key;
                string headerValue = string.Join(", ", new List<string>(header.Value).ToArray());

                try
                {
                    if (headerName.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        webRequest.ContentType = headerValue;
                    }
                    else if (headerName.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                    {
                        if (long.TryParse(headerValue, out long contentLength))
                        {
                            webRequest.ContentLength = contentLength;
                        }
                    }
                    else
                    {
                        webRequest.Headers[headerName] = headerValue;
                    }
                }
                catch
                {
                    // 일부 헤더는 설정할 수 없음
                }
            }
        }

        return webRequest;
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    /// <summary>
    ///     HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

#if NET40 || NET45 || NET451 || NET452
        return TaskEx.Run(() => SendCore(request, cancellationToken));
#else
        return Task.Run(() => SendCore(request, cancellationToken));
#endif
    }
}
#endif
