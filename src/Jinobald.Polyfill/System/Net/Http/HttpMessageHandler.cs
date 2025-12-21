#if NETFRAMEWORK
namespace System.Net.Http;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// HTTP 응답 메시지를 처리하는 형식의 기본 형식입니다.
/// </summary>
public abstract class HttpMessageHandler : IDisposable
{
    /// <summary>
    /// <see cref="HttpMessageHandler"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    protected HttpMessageHandler()
    {
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    protected internal abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

    /// <summary>
    /// <see cref="HttpMessageHandler"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <see cref="HttpMessageHandler"/>에서 사용하는 관리되지 않는 리소스를 해제하고, 필요한 경우 관리되는 리소스도 삭제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected virtual void Dispose(bool disposing)
    {
    }
}

/// <summary>
/// HTTP 응답을 처리하는 내부 핸들러라고 하는 다른 핸들러에 HTTP 응답 처리를 위임하는 형식입니다.
/// </summary>
public abstract class DelegatingHandler : HttpMessageHandler
{
    private HttpMessageHandler? _innerHandler;
    private volatile bool _disposed;

    /// <summary>
    /// HTTP 응답 메시지를 처리하는 내부 핸들러를 가져오거나 설정합니다.
    /// </summary>
    public HttpMessageHandler? InnerHandler
    {
        get { return _innerHandler; }
        set
        {
            CheckDisposed();
            _innerHandler = value;
        }
    }

    /// <summary>
    /// <see cref="DelegatingHandler"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    protected DelegatingHandler()
    {
    }

    /// <summary>
    /// 특정 내부 핸들러를 사용하여 <see cref="DelegatingHandler"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="innerHandler">HTTP 응답 메시지를 처리하는 내부 핸들러입니다.</param>
    protected DelegatingHandler(HttpMessageHandler innerHandler)
    {
        InnerHandler = innerHandler;
    }

    /// <summary>
    /// HTTP 요청을 비동기 작업으로 내부 핸들러에 보냅니다.
    /// </summary>
    /// <param name="request">보낼 HTTP 요청 메시지입니다.</param>
    /// <param name="cancellationToken">작업을 취소하기 위한 취소 토큰입니다.</param>
    /// <returns>비동기 작업을 나타내는 작업 개체입니다.</returns>
    protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_innerHandler == null)
            throw new InvalidOperationException("내부 핸들러가 설정되지 않았습니다.");

        return _innerHandler.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// <see cref="DelegatingHandler"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            _innerHandler?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }
}
#endif
