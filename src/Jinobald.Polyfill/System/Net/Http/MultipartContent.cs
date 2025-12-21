#if NETFRAMEWORK
namespace System.Net.Http;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// MIME 다중 파트 콘텐츠를 제공합니다.
/// </summary>
public class MultipartContent : HttpContent, IEnumerable<HttpContent>
{
    private readonly List<HttpContent> _nestedContent;
    private readonly string _boundary;
    private readonly string _subtype;

    /// <summary>
    /// <see cref="MultipartContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public MultipartContent()
        : this("mixed", GetDefaultBoundary())
    {
    }

    /// <summary>
    /// <see cref="MultipartContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="subtype">다중 파트 콘텐츠의 하위 형식입니다.</param>
    public MultipartContent(string subtype)
        : this(subtype, GetDefaultBoundary())
    {
    }

    /// <summary>
    /// <see cref="MultipartContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="subtype">다중 파트 콘텐츠의 하위 형식입니다.</param>
    /// <param name="boundary">다중 파트 콘텐츠의 경계 문자열입니다.</param>
    public MultipartContent(string subtype, string boundary)
    {
        if (string.IsNullOrWhiteSpace(subtype))
            throw new ArgumentException("하위 형식은 null이거나 빈 문자열일 수 없습니다.", nameof(subtype));
        if (string.IsNullOrWhiteSpace(boundary))
            throw new ArgumentException("경계 문자열은 null이거나 빈 문자열일 수 없습니다.", nameof(boundary));
        if (boundary.Length > 70)
            throw new ArgumentOutOfRangeException(nameof(boundary), "경계 문자열은 70자를 초과할 수 없습니다.");

        _subtype = subtype;
        _boundary = boundary;
        _nestedContent = new List<HttpContent>();

        Headers.ContentType = $"multipart/{subtype}; boundary=\"{boundary}\"";
    }

    /// <summary>
    /// <see cref="MultipartContent"/>에 HTTP 콘텐츠를 추가합니다.
    /// </summary>
    /// <param name="content">추가할 HTTP 콘텐츠입니다.</param>
    public virtual void Add(HttpContent content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        _nestedContent.Add(content);
    }

    /// <summary>
    /// HTTP 콘텐츠를 스트림으로 직렬화합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    protected override void SerializeToStream(Stream stream, TransportContext? context)
    {
        var encoding = Encoding.UTF8;
        var writer = new StreamWriter(stream, encoding);

        foreach (var content in _nestedContent)
        {
            writer.Write("\r\n--");
            writer.Write(_boundary);
            writer.Write("\r\n");

            // 콘텐츠 헤더 작성
            foreach (var header in content.Headers)
            {
                writer.Write(header.Key);
                writer.Write(": ");
                writer.Write(string.Join(", ", new List<string>(header.Value).ToArray()));
                writer.Write("\r\n");
            }
            writer.Write("\r\n");
            writer.Flush();

            // 콘텐츠 본문 작성
            content.CopyTo(stream);
        }

        writer.Write("\r\n--");
        writer.Write(_boundary);
        writer.Write("--\r\n");
        writer.Flush();
    }

    /// <summary>
    /// HTTP 콘텐츠의 바이트 길이를 계산합니다.
    /// </summary>
    /// <param name="length">콘텐츠의 길이(바이트)입니다.</param>
    /// <returns>길이를 계산할 수 있으면 <c>true</c>이고, 그렇지 않으면 <c>false</c>입니다.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
        // 다중 파트 콘텐츠의 길이를 미리 계산하기 어려움
        length = 0;
        return false;
    }

    /// <summary>
    /// 컬렉션을 반복하는 열거자를 반환합니다.
    /// </summary>
    public IEnumerator<HttpContent> GetEnumerator()
    {
        return _nestedContent.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// <see cref="MultipartContent"/>에서 사용하는 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스와 관리되지 않는 리소스를 모두 해제하려면 <c>true</c>이고, 관리되지 않는 리소스만 해제하려면 <c>false</c>입니다.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var content in _nestedContent)
            {
                content.Dispose();
            }
            _nestedContent.Clear();
        }

        base.Dispose(disposing);
    }

    private static string GetDefaultBoundary()
    {
        return "----=NextPart_" + Guid.NewGuid().ToString("N");
    }
}

/// <summary>
/// multipart/form-data MIME 형식을 사용하여 인코딩된 콘텐츠를 제공합니다.
/// </summary>
public class MultipartFormDataContent : MultipartContent
{
    /// <summary>
    /// <see cref="MultipartFormDataContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public MultipartFormDataContent()
        : base("form-data")
    {
    }

    /// <summary>
    /// <see cref="MultipartFormDataContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="boundary">다중 파트 form-data 콘텐츠의 경계 문자열입니다.</param>
    public MultipartFormDataContent(string boundary)
        : base("form-data", boundary)
    {
    }

    /// <summary>
    /// 지정된 이름을 가진 HTTP 콘텐츠를 <see cref="MultipartFormDataContent"/>에 추가합니다.
    /// </summary>
    /// <param name="content">추가할 HTTP 콘텐츠입니다.</param>
    /// <param name="name">콘텐츠의 이름입니다.</param>
    public void Add(HttpContent content, string name)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("이름은 null이거나 빈 문자열일 수 없습니다.", nameof(name));

        content.Headers.Add("Content-Disposition", $"form-data; name=\"{name}\"");
        base.Add(content);
    }

    /// <summary>
    /// 지정된 이름과 파일 이름을 가진 HTTP 콘텐츠를 <see cref="MultipartFormDataContent"/>에 추가합니다.
    /// </summary>
    /// <param name="content">추가할 HTTP 콘텐츠입니다.</param>
    /// <param name="name">콘텐츠의 이름입니다.</param>
    /// <param name="fileName">파일 이름입니다.</param>
    public void Add(HttpContent content, string name, string fileName)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("이름은 null이거나 빈 문자열일 수 없습니다.", nameof(name));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("파일 이름은 null이거나 빈 문자열일 수 없습니다.", nameof(fileName));

        content.Headers.Add("Content-Disposition", $"form-data; name=\"{name}\"; filename=\"{fileName}\"");
        base.Add(content);
    }
}
#endif
