#if NETFRAMEWORK
namespace System.Net.Http;

using System.IO;

/// <summary>
/// 바이트 배열을 기반으로 HTTP 콘텐츠를 제공합니다.
/// </summary>
public class ByteArrayContent : HttpContent
{
    private readonly byte[] _content;
    private readonly int _offset;
    private readonly int _count;

    /// <summary>
    /// <see cref="ByteArrayContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 바이트 배열입니다.</param>
    public ByteArrayContent(byte[] content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        _content = content;
        _offset = 0;
        _count = content.Length;
    }

    /// <summary>
    /// <see cref="ByteArrayContent"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="content">HTTP 콘텐츠로 사용할 바이트 배열입니다.</param>
    /// <param name="offset">HTTP 콘텐츠로 사용할 <paramref name="content"/>의 오프셋(바이트)입니다.</param>
    /// <param name="count">HTTP 콘텐츠로 사용할 <paramref name="content"/>에서 <paramref name="offset"/>부터 시작하는 바이트 수입니다.</param>
    public ByteArrayContent(byte[] content, int offset, int count)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        if (offset < 0 || offset > content.Length)
            throw new ArgumentOutOfRangeException(nameof(offset));
        if (count < 0 || count > content.Length - offset)
            throw new ArgumentOutOfRangeException(nameof(count));

        _content = content;
        _offset = offset;
        _count = count;
    }

    /// <summary>
    /// HTTP 콘텐츠를 스트림으로 직렬화합니다.
    /// </summary>
    /// <param name="stream">대상 스트림입니다.</param>
    /// <param name="context">전송 컨텍스트입니다.</param>
    protected override void SerializeToStream(Stream stream, TransportContext? context)
    {
        stream.Write(_content, _offset, _count);
    }

    /// <summary>
    /// HTTP 콘텐츠의 바이트 길이를 계산합니다.
    /// </summary>
    /// <param name="length">콘텐츠의 길이(바이트)입니다.</param>
    /// <returns>항상 <c>true</c>입니다.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
        length = _count;
        return true;
    }
}
#endif
