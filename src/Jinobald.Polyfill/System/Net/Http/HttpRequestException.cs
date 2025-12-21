#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
/// <see cref="HttpClient"/> 및 <see cref="HttpMessageHandler"/> 클래스에서 throw하는 기본 클래스입니다.
/// </summary>
public class HttpRequestException : Exception
{
    /// <summary>
    /// <see cref="HttpRequestException"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HttpRequestException()
        : base()
    {
    }

    /// <summary>
    /// 오류를 설명하는 특정 메시지를 사용하여 <see cref="HttpRequestException"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="message">오류를 설명하는 메시지입니다.</param>
    public HttpRequestException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// 오류를 설명하는 특정 메시지와 이 예외의 원인인 내부 예외에 대한 참조를 사용하여 <see cref="HttpRequestException"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="message">오류를 설명하는 메시지입니다.</param>
    /// <param name="inner">현재 예외의 원인인 내부 예외입니다.</param>
    public HttpRequestException(string? message, Exception? inner)
        : base(message, inner)
    {
    }
}
#endif
