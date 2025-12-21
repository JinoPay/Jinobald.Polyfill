#if NETFRAMEWORK
namespace System.Net.Http;

/// <summary>
/// HTTP 표준 메서드를 나타내는 도우미 클래스입니다.
/// </summary>
public class HttpMethod : IEquatable<HttpMethod>
{
    private static readonly HttpMethod _get = new HttpMethod("GET");
    private static readonly HttpMethod _post = new HttpMethod("POST");
    private static readonly HttpMethod _put = new HttpMethod("PUT");
    private static readonly HttpMethod _delete = new HttpMethod("DELETE");
    private static readonly HttpMethod _head = new HttpMethod("HEAD");
    private static readonly HttpMethod _options = new HttpMethod("OPTIONS");
    private static readonly HttpMethod _trace = new HttpMethod("TRACE");
    private static readonly HttpMethod _patch = new HttpMethod("PATCH");

    private readonly string _method;

    /// <summary>
    /// HTTP GET 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Get => _get;

    /// <summary>
    /// HTTP POST 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Post => _post;

    /// <summary>
    /// HTTP PUT 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Put => _put;

    /// <summary>
    /// HTTP DELETE 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Delete => _delete;

    /// <summary>
    /// HTTP HEAD 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Head => _head;

    /// <summary>
    /// HTTP OPTIONS 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Options => _options;

    /// <summary>
    /// HTTP TRACE 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Trace => _trace;

    /// <summary>
    /// HTTP PATCH 프로토콜 메서드를 나타냅니다.
    /// </summary>
    public static HttpMethod Patch => _patch;

    /// <summary>
    /// HTTP 메서드를 가져옵니다.
    /// </summary>
    public string Method => _method;

    /// <summary>
    /// 지정된 HTTP 메서드를 사용하여 <see cref="HttpMethod"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="method">HTTP 메서드입니다.</param>
    public HttpMethod(string method)
    {
        if (string.IsNullOrEmpty(method))
            throw new ArgumentException("HTTP 메서드는 null이거나 빈 문자열일 수 없습니다.", nameof(method));

        _method = method.ToUpperInvariant();
    }

    /// <summary>
    /// 지정된 <see cref="HttpMethod"/>가 현재 <see cref="HttpMethod"/>와 같은지 여부를 확인합니다.
    /// </summary>
    public bool Equals(HttpMethod? other)
    {
        if (other == null)
            return false;

        return string.Equals(_method, other._method, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 지정된 개체가 현재 개체와 같은지 여부를 확인합니다.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return Equals(obj as HttpMethod);
    }

    /// <summary>
    /// 이 인스턴스의 해시 코드를 반환합니다.
    /// </summary>
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(_method);
    }

    /// <summary>
    /// HTTP 메서드를 나타내는 문자열을 반환합니다.
    /// </summary>
    public override string ToString()
    {
        return _method;
    }

    /// <summary>
    /// 두 <see cref="HttpMethod"/> 개체가 같은지 여부를 확인합니다.
    /// </summary>
    public static bool operator ==(HttpMethod? left, HttpMethod? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// 두 <see cref="HttpMethod"/> 개체가 다른지 여부를 확인합니다.
    /// </summary>
    public static bool operator !=(HttpMethod? left, HttpMethod? right)
    {
        return !(left == right);
    }
}
#endif
