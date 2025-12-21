#if NETFRAMEWORK
namespace System.Net.Http;

using System.Text;

/// <summary>
/// 문자열을 기반으로 HTTP 콘텐츠를 제공합니다.
/// </summary>
public class StringContent : ByteArrayContent
{
    private const string DefaultMediaType = "text/plain";

    /// <summary>
    /// <see cref="StringContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 문자열입니다.</param>
    public StringContent(string content)
        : this(content, null, null)
    {
    }

    /// <summary>
    /// <see cref="StringContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 문자열입니다.</param>
    /// <param name="encoding">콘텐츠에 사용할 인코딩입니다.</param>
    public StringContent(string content, Encoding? encoding)
        : this(content, encoding, null)
    {
    }

    /// <summary>
    /// <see cref="StringContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 문자열입니다.</param>
    /// <param name="encoding">콘텐츠에 사용할 인코딩입니다.</param>
    /// <param name="mediaType">콘텐츠의 미디어 형식입니다.</param>
    public StringContent(string content, Encoding? encoding, string? mediaType)
        : base(GetContentByteArray(content, encoding ?? Encoding.UTF8))
    {
        var actualEncoding = encoding ?? Encoding.UTF8;
        var actualMediaType = mediaType ?? DefaultMediaType;

        Headers.ContentType = $"{actualMediaType}; charset={actualEncoding.WebName}";
    }

    private static byte[] GetContentByteArray(string content, Encoding encoding)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        return encoding.GetBytes(content);
    }
}
#endif
